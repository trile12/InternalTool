using EposToolV5.Presentation.Models;
using JsonApiSerializer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Media.Protection.PlayReady;

namespace EposToolV5.ViewModels;

public partial class SyncOrderViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LogInfo> _orderInfos;

    [ObservableProperty]
    private ObservableCollection<LogInfo> _filteredOrders;

    [ObservableProperty]
    private string _orderCount;

    [ObservableProperty]
    private string _filterText;

    [ObservableProperty]
    private bool _isDropTargetVisible;

    [ObservableProperty]
    private string _currentPath;

    private readonly IJSRuntime _jsRuntime;
    public SyncOrderViewModel()
    {
        OrderInfos = new ObservableCollection<LogInfo>();
        FilteredOrders = new ObservableCollection<LogInfo>();
        OrderCount = "0";
        IsDropTargetVisible = false;
    }

    partial void OnFilterTextChanged(string value)
    {
        FilterOrders();
    }

    public record LogFile(string Name, string Content);

    [RelayCommand]
    private async Task BrowseFiles()
    {
#if __WASM__
        try
        {
            // Gọi JavaScript function pickLogFile
            var logfiles = await _jsRuntime.InvokeAsync<LogFile[]>("pickLogFile");

            if (logfiles != null && logfiles.Length > 0)
            {
                Refresh();
                foreach (var file in logfiles)
                {
                    Console.WriteLine($"[WASM] File: {file.Name}");
                    //await ProcessLogFile(file.Content); // Hàm này bạn tự định nghĩa
                }
            }

            return; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JS error: {ex.Message}");
            return;
        }
#endif

        // Desktop/WinApp logic
        var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
        filePicker.FileTypeFilter.Add(".log");
        filePicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
        filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

        var files = await filePicker.PickMultipleFilesAsync();
        if (files != null && files.Count > 0)
        {
            Refresh();
            foreach (var file in files)
            {
                CurrentPath = Path.GetDirectoryName(file.Path);
                await ReadLogFileAsync(file);
            }
        }
    }


    [RelayCommand]
    private async Task SyncOrder()
    {
        foreach (var order in FilteredOrders.Where(x => !x.IsSuccessful))
        {
            //order.IsSuccessful = await RestCreateOrderAsync(order);
            order.IsSuccessful = await SendWithHttpClientAsync(order);
        }
    }

    [RelayCommand]
    private void Refresh()
    {
        OrderInfos.Clear();
        FilteredOrders.Clear();
        FilterText = string.Empty;
        OrderCount = "0";
    }

    private async Task<bool> RestCreateOrderAsync(LogInfo logInfo)
    {
        if (logInfo == null)
            return false;

        var client = new RestClient(logInfo.Endpoint);
        var request = new RestRequest();

        request.Method = Method.Post;

        request.AddHeader("Authorization", logInfo.AuthorizationToken);
        request.AddHeader("Content-Type", "application/vnd.api+json");
        request.AddHeader("Accept", "application/vnd.api+json");
        //request.AddParameter("application/vnd.api+json", logInfo.JsonData, ParameterType.RequestBody);
        request.AddStringBody(logInfo.JsonData, DataFormat.Json);

        var response = await client.ExecuteAsync(request);
        return response.IsSuccessful;
    }

    private static readonly HttpClient _client = new HttpClient();
    private async Task<bool> SendWithHttpClientAsync(LogInfo logInfo)
    {
        if (logInfo == null) return false;

        var request = new HttpRequestMessage(HttpMethod.Post, logInfo.Endpoint);

        request.Headers.TryAddWithoutValidation("Authorization", logInfo.AuthorizationToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

        var content = new StringContent(logInfo.JsonData, Encoding.UTF8);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");
        request.Content = content;


        try
        {
            var response = await _client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {response.StatusCode}, Response: {responseContent}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending request: {ex.Message}");
            return false;
        }
    }

    public async Task HandleDroppedFiles(IReadOnlyList<Windows.Storage.StorageFile> files)
    {
        if (files != null && files.Count > 0)
        {
            Refresh();
            foreach (var file in files)
            {
                CurrentPath = Path.GetDirectoryName(file.Path);
                await ReadLogFileAsync(file);
            }
        }
        IsDropTargetVisible = false;
    }

    private async Task ReadLogFileAsync(Windows.Storage.StorageFile file)
    {
        try
        {
            string fileContent = await Windows.Storage.FileIO.ReadTextAsync(file);
            string[] lines = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                ParseLogLine(line);
            }

            var checkOrders = OrderInfos.Where(x => !string.IsNullOrEmpty(x.OrderId)).ToList();
            OrderInfos = new ObservableCollection<LogInfo>(checkOrders);
            FilteredOrders = new ObservableCollection<LogInfo>(OrderInfos);
            OrderCount = OrderInfos.Count.ToString();
        }
        catch (Exception ex)
        {
            // Show error dialog in Uno Platform
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Error",
                Content = $"Error reading log file: {ex.Message}",
                CloseButtonText = "OK"
            };

            // Make sure to use App.MainWindow for proper modal dialog display
            await dialog.ShowAsync();
        }
    }

    private string authorizationPattern = @"Authorization: (\S+)";
    private string endpointPattern = @"(https://\S+?\.(livedevs|eposdata)\.com/api)";

    private void ParseLogLine(string line)
    {
        if (line.Contains("/api"))
        {
            var orderInfo = new LogInfo();
            string authorizationToken = ExtractValue(line, authorizationPattern);
            string endpointUrl = ExtractValue(line, endpointPattern);

            orderInfo.AuthorizationToken = authorizationToken;
            orderInfo.Endpoint = endpointUrl;
            OrderInfos.Add(orderInfo);
        }

        if (line.Contains("RequestOrderSync") && line.Contains("[ORDER] Sync Request Data"))
        {
            int startIndex = line.IndexOf("{");
            int endIndex = line.LastIndexOf("}");

            if (startIndex != -1 && endIndex != -1)
            {
                string jsonData = line.Substring(startIndex, endIndex - startIndex + 1);

                try
                {
                    var requestData = JsonConvert.DeserializeObject<JObject>(jsonData);

                    string orderId = requestData["data"]["id"].ToString();
                    string orderNo = requestData["data"]["attributes"]["order_no"].ToString();
                    string orderStatus = requestData["data"]["attributes"]["status"].ToString();
                    string shiftId = requestData["data"]["attributes"]["shift_id"].ToString();

                    var orderModel = new RestOrder();
                    var order = OrderInfos.LastOrDefault(x => string.IsNullOrEmpty(x.OrderId));
                    // Origin Json Data
                    var newJsonData = requestData.ToString();

                    var settings = new JsonApiSerializerSettings();
                    orderModel = JsonConvert.DeserializeObject<RestOrder>(jsonData, settings);
                    var lineItemTaxs = orderModel.LineItems.SelectMany(x => x.Taxes).ToList();

                    foreach (var tax in orderModel.OrderTaxes)
                    {
                        var lineTax = lineItemTaxs.FirstOrDefault(x => x.SourceId == tax.SourceId);
                        if (lineTax != null)
                        {
                            tax.Priority = lineTax.Priority;
                        }
                    }
                    newJsonData = JsonConvert.SerializeObject(orderModel, settings);

                    if (order != null)
                    {
                        order.OrderId = orderId;
                        order.OrderNo = orderNo;
                        order.JsonData = newJsonData;
                        order.ShiftId = shiftId;
                        order.OrderStatus = orderStatus;
                        order.IsVoidOrder = orderStatus == "void";
                        order.Endpoint += "/operations/orders";
                        order.Order = orderModel;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }
        }
    }

    private string ExtractValue(string input, string pattern)
    {
        Regex regex = new Regex(pattern);
        Match match = regex.Match(input);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
    }

    private void FilterOrders()
    {
        if (string.IsNullOrEmpty(FilterText))
        {
            FilteredOrders = new ObservableCollection<LogInfo>(OrderInfos);
            OrderCount = OrderInfos.Count.ToString();
            return;
        }

        List<string> filterInput = FilterText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var filtered = FilterStrings(OrderInfos, filterInput);
        filtered = DistinctByOrderIdAndStatus(filtered);
        FilteredOrders = new ObservableCollection<LogInfo>(filtered);
        OrderCount = FilteredOrders.Count.ToString();
    }

    private static List<LogInfo> FilterStrings(ObservableCollection<LogInfo> inputList, List<string> numbersToFilter)
    {
        List<LogInfo> filteredList = new List<LogInfo>();
        foreach (var input in inputList)
        {
            if (numbersToFilter.Any(number => input.OrderId.Contains(number) || input.OrderNo.Contains(number) || input.ShiftId.Contains(number)))
            {
                filteredList.Add(input);
            }
        }

        return filteredList;
    }

    private List<LogInfo> DistinctByOrderIdAndStatus(List<LogInfo> orders)
    {
        return orders
            .GroupBy(o => new { o.OrderId, o.OrderStatus })
            .Select(g => g.First())
            .ToList();
    }
}

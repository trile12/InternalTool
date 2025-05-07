using InternalTool.ViewModels;

namespace InternalTool.Views;

public sealed partial class SyncOrderPage : Page
{
    public SyncOrderViewModel ViewModel { get; }
    public SyncOrderPage()
    {
        this.InitializeComponent();
        ViewModel = (App.Current as App)?.GetHost().Services.GetRequiredService<SyncOrderViewModel>();
        DataContext = ViewModel;
    }

    private void OrdersListView_DragEnter(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            ViewModel.IsDropTargetVisible = true;
        }
    }

    private void OrdersListView_DragLeave(object sender, DragEventArgs e)
    {
        ViewModel.IsDropTargetVisible = false;
    }

    private async void OrdersListView_Drop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
        {
            var items = await e.DataView.GetStorageItemsAsync();
            var files = items.OfType<Windows.Storage.StorageFile>().Where(f => f.FileType.Equals(".log", StringComparison.OrdinalIgnoreCase)).ToList();

            if (files.Count > 0)
            {
                await ViewModel.HandleDroppedFiles(files);
            }
        }

        ViewModel.IsDropTargetVisible = false;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        // Setup drag and drop events
        OrdersListView.DragEnter += OrdersListView_DragEnter;
        OrdersListView.DragLeave += OrdersListView_DragLeave;
        OrdersListView.Drop += OrdersListView_Drop;
    }
}

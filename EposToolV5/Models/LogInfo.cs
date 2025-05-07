namespace EposToolV5.Presentation.Models;

public class LogInfo : ObservableObject
{
    public RestOrder Order { get; set; }
    private bool _isSuccessful;
    private bool _isVoidOrder;

    public string OrderId { get; set; }
    public string OrderNo { get; set; }
    public string AuthorizationToken { get; set; }
    public string Endpoint { get; set; }
    public string JsonData { get; set; }
    public string OrderStatus { get; set; }
    public string ShiftId { get; set; }

    public bool IsVoidOrder
    {
        get { return _isVoidOrder; }
        set
        {
            if (_isVoidOrder != value)
            {
                _isVoidOrder = value;
                OnPropertyChanged(nameof(_isVoidOrder));
            }
        }
    }

    public bool IsSuccessful
    {
        get { return _isSuccessful; }
        set
        {
            if (_isSuccessful != value)
            {
                _isSuccessful = value;
                OnPropertyChanged(nameof(IsSuccessful));
            }
        }
    }
}

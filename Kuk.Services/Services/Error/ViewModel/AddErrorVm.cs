namespace Kuk.Services.Services.Error.ViewModel
{
    public class AddErrorVm
    {
        
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ServiceName { get; set; }
        public string Table { get; set; }
        public int? TableId { get; set; }
        
        public object Variables { get; set; }
        
        public Exception Exception { get; set; }
        
    }
}

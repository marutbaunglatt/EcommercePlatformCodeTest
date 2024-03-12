namespace EcommercePlatformCodeTest.Dtos
{
    public class CBPayCheckResponse
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string transStatus { get; set; } //Length = 1
        public string bankTransId { get; set; } //Length = 32
        public string transAmount { get; set; } //Length = 13
        public string transCurrency { get; set; } //Length = 3
    }
}

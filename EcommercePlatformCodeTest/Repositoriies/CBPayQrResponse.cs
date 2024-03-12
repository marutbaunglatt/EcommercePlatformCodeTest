namespace EcommercePlatformCodeTest.Repositoriies
{
    public class CBPayQrResponse
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string merDqrCode { get; set; }//Length = 512
        public string transExpiredTime { get; set; }//Length = 19
        public string refNo { get; set; }//Length = 16
        public string transRef { get; set; }//Length = 16
    }
}
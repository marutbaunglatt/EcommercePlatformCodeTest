namespace EcommercePlatformCodeTest.Dtos
{
    public class CBPayQrRequest
    {
        public string reqId { get; set; } //Length = 32
        public string merId { get; set; }//Length = 16
        public string subMerId { get; set; }//Length = 16
        public string terminalId { get; set; }//Length = 8
        public string transAmount { get; set; }//Length = 13
        public string transCurrency { get; set; }//Length = 3
        public string ref1 { get; set; }//Length = 25
        public string ref2 { get; set; }//Length = 25
    }

    public class CBPaymentReqVM
    {
        //public string version { get; set; }
        public string merchantID { get; set; }
        public string invoiceNo { get; set; }
        public string productDesc { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string hashValue { get; set; }
        public string qrURL { get; set; }
        public string checkURL { get; set; }
        public string subMerchantId { get; set; }//Length = 16
        public string terminalId { get; set; }//Length = 8
        public string ref1 { get; set; }//Length = 25
        public string ref2 { get; set; }//Length = 25
        public string QrImage { get; set; }
        public string qrCodeData { get; set; }
        public string TransactionRefNo { get; set; }
        public string aut_Token { get; set; }

    }
}

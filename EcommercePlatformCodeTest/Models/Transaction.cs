namespace EcommercePlatformCodeTest.Models
{
    public class UserTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        public string TransStatus { get; set; } //Length = 1
        public string BankTransId { get; set; } //Length = 32
        public string TransAmount { get; set; } //Length = 13
        public string TransCurrency { get; set; } //Length = 3
    }
}

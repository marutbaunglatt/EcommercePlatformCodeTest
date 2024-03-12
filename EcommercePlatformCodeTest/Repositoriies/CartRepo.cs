using EcommercePlatformCodeTest.Data;
using EcommercePlatformCodeTest.Interfaces;
using EcommercePlatformCodeTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using IronBarCode;
using System.Drawing;
using System.Drawing.Imaging;
using EcommercePlatformCodeTest.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcommercePlatformCodeTest.Repositoriies
{
    public class CartRepo : ICart
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CartRepo(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task AddToCart(Product product, int userId, int quantity)
        {
            if (product == null) throw new ApplicationException("Product not found.");

            var newCart = await _context.CartItems.AsNoTracking()
                .Where(x => x.ProductId == product.Id && x.IsPaymentDone == false)
                .FirstOrDefaultAsync();

            if (newCart == null) newCart = new CartItem();

            if (newCart.Id > 0)
            {
                newCart.Quantity += quantity;
                newCart.UpdatedAt = DateTime.Now;
                _context.CartItems.Update(newCart);
            }
            else
            {
                newCart.ProductId = product.Id;
                newCart.UserId = userId;
                newCart.Quantity += quantity;
                newCart.CreatedAt = DateTime.Now;
                newCart.UpdatedAt = DateTime.Now;
                await _context.CartItems.AddAsync(newCart);
            }
            product.Stock -= quantity;
            product.UpdatedAt = DateTime.Now;
            _context.Products.Update(product);

            await _context.SaveChangesAsync();
        }

        public async Task<(int, List<CartItem>)> CartItems(int startIndex, int showCount, string searchString)
        {
            var res = await _context.CartItems.Include(x => x.Product).AsNoTracking()
                 .Where(x => x.IsPaymentDone == false && x.Quantity > 0 &&
                 ((!string.IsNullOrWhiteSpace(searchString) && x.Product != null) ? (x.Product.Title.ToLower().Contains(searchString) || x.Product.Description.ToLower().Contains(searchString)) : true))
                 .ToListAsync();
            int totalCount = res.Count();

            res = res.Skip((startIndex - 1) * showCount)
                     .Take(showCount)
                     .ToList();
            return (totalCount, res);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Id == productId);
        }

        public async Task RemoveCart(int id)
        {
            var cartItem = await _context.CartItems.SingleOrDefaultAsync(x => x.Id == id);
            if (cartItem != null)
            {
                var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == cartItem.ProductId);
                if (product != null)
                {
                    product.Stock += cartItem.Quantity;
                    _context.Products.Update(product);
                }

                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCartItemQuantity(int id, int quantity)
        {
            var cart = await _context.CartItems.Include(x => x.Product).SingleOrDefaultAsync(x => x.Id == id);
            if (cart != null && (quantity >= cart.Quantity || quantity <= cart.Quantity))
            {

                if (cart.Product != null)
                    cart.Product.Stock += quantity;

                cart.Quantity -= quantity;
                cart.UpdatedAt = DateTime.Now;

                _context.CartItems.Update(cart);

                await _context.SaveChangesAsync();
            }
        }

        public async Task PayNow(int id, int quantity)
        {

        }

        public async Task<CBPaymentReqVM> GetQrString_FromCBBank(int userId, decimal amount)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authen-Token", _configuration.GetSection("CBPayment:CB_AuthenToken").Value);

            string url = _configuration.GetSection("CBPayment:CB_QrGenerate_URL").Value;

            CBPaymentReqVM cbVM = new CBPaymentReqVM();


            CBPayQrRequest reqBody = new CBPayQrRequest()
            {
                reqId = "C_" + userId + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                merId = _configuration.GetSection("CBPayment:CB_MerchantID").Value,
                subMerId = _configuration.GetSection("CBPayment:CB_Sub_MarchantId").Value,
                terminalId = _configuration.GetSection("CBPayment:CB_TerminalId").Value,
                transAmount = amount.ToString(), //getAmount(Amount),
                transCurrency = _configuration.GetSection("CBPayment:Currency").Value,
                ref1 = "EcomercePlatFormPayment"
            };

            var json = JsonSerializer.Serialize(reqBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, data);
                var resBody = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<CBPayQrResponse>(resBody);
                if (res != null && res.code == "0000" && res.merDqrCode != null)
                {
                    cbVM.invoiceNo = reqBody.reqId;
                    cbVM.TransactionRefNo = res.transRef;
                    cbVM.amount = amount.ToString();
                    cbVM.QrImage = await GenereateQR(res.merDqrCode);
                    //cbVM.qrCodeData = res.merDqrCode;
                    //qrGenerator(res.merDqrCode);
                    //cbVM.qrPath = _configuration.GetSection("Qr_Path").Value;


                    Debug.WriteLine(resBody);
                }
                else
                {
                    Debug.WriteLine(resBody);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            }
            return cbVM;
        }

        public async Task<CBPayCheckResponse> CheckTransaction_FromCBBank(string transactionRefNo, int userId)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authen-Token", _configuration.GetSection("CBPayment:CB_AuthenToken").Value);

            string url = _configuration.GetSection("CBPayment:CB_CheckTransaction_URL").Value;

            CBPayCheckResponse cbResponse = new CBPayCheckResponse();
            CBPayCheckTransactionRequest reqBody = new CBPayCheckTransactionRequest()
            {
                merId = _configuration.GetSection("CBPayment:CB_MerchantID").Value,
                transRef = transactionRefNo
            };
            var json = JsonSerializer.Serialize(reqBody);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(url, data);
                var resBody = await response.Content.ReadAsStringAsync();

                var res = JsonSerializer.Deserialize<CBPayCheckResponse>(resBody);
                if (res.code == "0000" && res.transAmount != null && (res.transStatus == "P" || res.transStatus == "S"))
                {
                    cbResponse.transStatus = GetLongTransactionStatus(res.transStatus);
                    cbResponse.code = res.code;
                    cbResponse.msg = res.msg;
                    cbResponse.bankTransId = res.bankTransId;
                    cbResponse.transAmount = res.transAmount;
                    cbResponse.transCurrency = res.transCurrency;

                    Debug.WriteLine(resBody);

                    var carts = await _context.CartItems.Where(x => x.UserId == userId).ToArrayAsync();
                    foreach(var item in carts)
                    {
                        item.IsPaymentDone = true;
                        item.UpdatedAt = DateTime.Now;
                    }
                    _context.CartItems.UpdateRange(carts);

                    var tran = new UserTransaction
                    {
                        Code = cbResponse.code,
                        UserId = userId,
                        Msg = cbResponse.msg,
                        TransStatus = cbResponse.transStatus,
                        BankTransId = cbResponse.bankTransId,
                        TransAmount = cbResponse.transAmount,
                        TransCurrency = cbResponse.transCurrency,
                    };
                    await _context.UserTransactions.AddAsync(tran);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    cbResponse.transStatus = GetLongTransactionStatus(res.transStatus);
                    Debug.WriteLine(resBody);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                //return false;
            }
            return cbResponse;
        }


        public async Task<string> GenereateQR(string qrData)
        {
            try
            {
                string base64String = "";
                GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(qrData, BarcodeEncoding.QRCode);

                Bitmap barcodeBitmap = barcode.ToBitmap();
                using (MemoryStream ms = new MemoryStream())
                {
                    barcodeBitmap.Save(ms, ImageFormat.Png);
                    byte[] bitmapBytes = ms.ToArray();
                    base64String = Convert.ToBase64String(bitmapBytes);
                }

                return base64String;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string GetLongTransactionStatus(string shortCode)
        {
            string str = "";
            switch (shortCode)
            {
                case "P": str = "Pending"; break;
                case "S": str = "Success"; break;
                case "E": str = "Expired"; break;
                case "C": str = "Cancelled"; break;
                case "L": str = "Over limit"; break;
                case "AP": str = "Approved"; break;
                case "RS": str = "Ready to Settle"; break;
                case "SE": str = "Settled"; break;
                case "VO": str = "Voided"; break;
                case "DE": str = "Declined"; break;
                case "FA": str = "Failed"; break;
                case "RE": str = "Refund Pending"; break;
                case "RR": str = "Refund Ready"; break;
                case "RF": str = "Refunded"; break;
                case "PR": str = "Payment Gateway receive"; break;
                default: str = shortCode; break;
            }
            return str;
        }

        public async Task<List<UserTransaction>> GetUserTransaction(int userId)
        {
            return await _context.UserTransactions.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }
    }
}

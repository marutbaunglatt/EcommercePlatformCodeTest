using EcommercePlatformCodeTest.Interfaces;
using EcommercePlatformCodeTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommercePlatformCodeTest.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICart _cartRepo;
        public CartController(ICart cart)
        {
            _cartRepo = cart;
        }
        public async Task<IActionResult> Index(int startIndex = 1, int showCount = 10, string searchString = "")
        {
            HttpContext.Session.SetString("id", "clist");
            if (startIndex <= 0) startIndex = 1;
            var res = await _cartRepo.CartItems(startIndex, showCount, searchString);
            ViewBag.TotalCount = res.Item1;
            ViewBag.StartIndex = startIndex;
            ViewBag.SearchString = searchString;

            #region btn show count
            int btnShowCount = res.Item1 / showCount;
            if (res.Item1 % showCount > 0)
                btnShowCount += 1;
            ViewBag.ShowButtonCount = btnShowCount;
            #endregion

            return View(res.Item2);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    return Json(new { success = false, message = "Quantity must be greater than zero." });


                var currentUser = HttpContext.User;
                if (currentUser != null && User.Identity.IsAuthenticated)
                {
                    var product = await _cartRepo.GetProductByIdAsync(productId);
                    if (product == null)
                    {
                        TempData["CartError"] = "Product not found.";
                        return Json(new { success = false, message = "Product not found." });
                    }

                    if (quantity > product.Stock)
                    {
                        return Json(new { success = false, message = "Quantity exceeds cart item quantity." });
                    }

                    if (product.Stock <= 0)
                    {
                        TempData["CartWarning"] = "Product is out of stock.";
                        return Json(new { success = false, message = "Product is out of stock." });
                    }

                    if (int.TryParse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
                    {
                        await _cartRepo.AddToCart(product, userId, quantity);
                        TempData["CartSuccess"] = "Product added to cart successfully.";
                        return Json(new { success = true, message = "Product added to cart successfully." });
                    }
                    return Json(new { success = false, message = "Cart item not found." });
                }
                else return RedirectToAction("login", "User");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Cart item add error occurred " + ex.Message });
            }
        }

        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            await _cartRepo.UpdateCartItemQuantity(id, quantity);
            return RedirectToAction("Index", "Cart");
        }

        public async Task<ActionResult> RemoveItem(int id)
        {
            await _cartRepo.RemoveCart(id);
            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> GenerateQR(decimal amount)
        {
            var currentUser = HttpContext.User;
            if (currentUser != null && User.Identity.IsAuthenticated && int.TryParse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var res = await _cartRepo.GetQrString_FromCBBank(userId, amount);
                if (res != null)
                {
                    ViewBag.QRCodeImage = "data:image/png;base64," + res.QrImage;
                    ViewBag.TransactionRefNo = res.TransactionRefNo;
                    ViewBag.Amount = amount;
                    return View();
                }
            }
            return View("login");
        }

        public async Task<IActionResult> CheckTransaction_FromCBBank(string transactionRefNo, decimal amount)
        {
            var currentUser = HttpContext.User;
            if (currentUser != null && User.Identity.IsAuthenticated && int.TryParse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var res = await _cartRepo.CheckTransaction_FromCBBank(transactionRefNo, userId);
                if (res.transCurrency != null)
                {
                    return View("CheckTransaction_FromCBBank", transactionRefNo);
                }
                return RedirectToAction("GenerateQR", new { amount = amount });
            }
            return View("login");
        }

        public async Task<IActionResult> UserTransactions()
        {
             var currentUser = HttpContext.User;
            if (currentUser != null && User.Identity.IsAuthenticated && int.TryParse(currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var userTran = await _cartRepo.GetUserTransaction(userId);
                return View("UserTransactions", userTran);
            }
            return View("login");
        }
    }
}


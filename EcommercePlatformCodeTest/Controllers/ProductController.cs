using EcommercePlatformCodeTest.Data;
using EcommercePlatformCodeTest.Infterfaces;
using EcommercePlatformCodeTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatformCodeTest.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _product;
        public ProductController(IProduct product)
        {
            _product = product;
        }
        public async Task<IActionResult> Index(int startIndex = 1, int showCount = 9, string searchString = "", string categor = "", bool partial=false)
        {
            HttpContext.Session.SetString("id", "plist");
            if (startIndex <= 0) startIndex = 1;
            var res = await _product.GetAllProductsAsync(startIndex, showCount, searchString, categor);
            ViewBag.TotalCount = res.Item1;
            ViewBag.ShowCount = showCount;
            ViewBag.StartIndex = startIndex;
            ViewBag.SearchString = searchString;
            ViewBag.Categor = categor;

            #region btn show count
            int btnShowCount = res.Item1 / showCount;
            if (res.Item1 % showCount > 0)
                btnShowCount += 1;
            ViewBag.ShowButtonCount = btnShowCount;
            #endregion

            if (partial)
                return PartialView("_FilteredProducts", res.Item2);
            return View(res.Item2);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || id == 0)
            {
                //ViewBag.CorE = "အသစ်ထည့်သွင်းခြင်း";
                Product product = new Product();
                return View(product);
            }
            else
            {
                //ViewBag.CorE = "ပြင်ဆင်ခြင်း";
                var agency = await _product.GetProductByIdAsync(id ?? 0);
                //agency.BODList = managingDirectors;
                if (agency == null)
                    return NotFound();
                return View(agency);
            }
        }
    }
}

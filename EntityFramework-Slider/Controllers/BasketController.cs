using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System.Collections.Generic;

namespace EntityFramework_Slider.Controllers
{
    public class BasketController : Controller
    {

        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketProducts;
            List<BasketDetailVM> basketDetails = new();

            if (Request.Cookies["basket"] != null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basketProducts = new List<BasketVM>();
            }


            foreach (var product in basketProducts)
            {
                var dbProduct = await _context.Products.Include(m => m.Images).Include(m=>m.Category).FirstOrDefaultAsync(m => m.Id == product.Id);
                basketDetails.Add(new BasketDetailVM
                {
                    Id = dbProduct.Id,
                    Name = dbProduct.Name,
                    CategoryName = dbProduct.Category.Name,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price,
                    Image = dbProduct.Images.Where(m=>m.IsMain).FirstOrDefault().Image,
                    Count = product.Count,
                    Total = dbProduct.Price * product.Count
                });
            }

            return View(basketDetails);






            //List<BasketVM> basket;

            //if (Request.Cookies["basket"] != null)
            //{
            //    basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            //}
            //else
            //{
            //    basket = new List<BasketVM>();
            //}

            //foreach (var baskets in basket)
            //{
            //    Product dbproduct = _context.Products.Include(m => m.Images).FirstOrDefault(m => m.Id == baskets.Id);
            //    baskets.Product = dbproduct;
            //}


            //return View(basket);


            
        }


        [ActionName("Delete")]
        public IActionResult DeleteProductFromBasket(int? id)
        {
            if (id is null) return BadRequest();

            List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM deleteProduct = basketProducts.FirstOrDefault(m => m.Id == id);

            basketProducts.Remove(deleteProduct);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

            return RedirectToAction("Index");
        }
    }
}

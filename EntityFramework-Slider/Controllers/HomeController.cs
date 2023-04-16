using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using System.Diagnostics;

namespace EntityFramework_Slider.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //HttpContext.Session.SetString("name", "Pervin");

            //Response.Cookies.Append("surname", "Rehimli", new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) });

            //Book book = new Book
            //{
            //    Id = 1,
            //    Name = "Xosrov ve Shirin"
            //};

            //Response.Cookies.Append("book", JsonConvert.SerializeObject(book));
            
           

            List<Slider> sliders = await _context.Sliders.ToListAsync();

            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

            IEnumerable<Blog> blogs = await _context.Blogs.Where(m=>!m.SoftDelete).ToListAsync();

            IEnumerable<Category> categories = await _context.Categories.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToListAsync();

            About abouts = await _context.Abouts.Include(m=>m.Advantages).FirstOrDefaultAsync();

            IEnumerable<Expert> experts = await _context.Experts.Where(m => !m.SoftDelete).ToListAsync();

            Subscribe subscribes = await _context.Subscribes.FirstOrDefaultAsync();

            IEnumerable<Say> says = await _context.Says.Where(m => m.SoftDelete == false).ToListAsync();

            IEnumerable<Instagram> instagrams = await _context.Instagrams.Where(m => m.SoftDelete == false).ToListAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Blogs = blogs,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                Subscribes = subscribes, 
                Says = says,
                Instagrams = instagrams
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return BadRequest();

            Product? dbProduct = await GetProductById((int)id);

            if (dbProduct == null) return NotFound();

            List<BasketVM> basket = GetBasketDatas();

            BasketVM? existProduct = basket?.FirstOrDefault(m => m.Id == dbProduct.Id);

            AddProductToBasket(existProduct, dbProduct, basket);

            return RedirectToAction(nameof(Index));
        }

        private Task<Product?> GetProductById(int? id)
        {
            throw new NotImplementedException();
        }


        private async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        private List<BasketVM> GetBasketDatas()
        {
            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            return basket;
        }


        private void AddProductToBasket(BasketVM existProduct,Product product,List<BasketVM> basket)
        {
            if (existProduct == null)
            {
                basket?.Add(new BasketVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
        }






        //public IActionResult Test()
        //{
        //    var sessionData = HttpContext.Session.GetString("name");
        //    var cookieData = Request.Cookies["surname"];
        //    var objectData = JsonConvert.DeserializeObject<Book>(Request.Cookies["book"]);


        //    return Json(objectData);
        //}

        //class Book
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //}
    }
}
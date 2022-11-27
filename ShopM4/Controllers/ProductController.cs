using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Models.ViewModels;
using ShopM4.Controllers;
using ShopM4.wwwroot.Product;

namespace ShopM4.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;
        private IWebHostEnvironment webhostEnvironment;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product;
            /*
            // получаем ссылки на сущности категорий
            foreach (var item in objList)
            {
                // сопоставление таблицы категорий и таблицы product
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            */
            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            /*
            // получаем лист категорий для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            // отправляем лист категорий во View
            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList;
            */

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = db.Product.Find(id);

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }
        //POST - CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(IFormFile file, ProductViewModel productViewModel)
        {
            var files = HttpContext.Request.Form.Files;
            string wwroot = webhostEnvironment.WebRootPath;
            if (productViewModel.Product.Id == 0)
            {
                string upload = wwroot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                string path = upload + imageName + extension;
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                productViewModel.Product.Image = imageName + extension;
                db.Product.Add(productViewModel.Product);

            }
            else
            {
                //upadte
            }
            return View();
        }
    }
}


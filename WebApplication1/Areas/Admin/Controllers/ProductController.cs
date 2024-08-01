using AOUBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using AOUBook.DataAccess.Data;
using AOUBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AOUBook.Models.ViewModels;
using AOUBook.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AOUBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7200/api/Product";
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
        }
        public async Task <IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductVM>>(content);
                return View(products);
            }
            else
            {
                // Handle error response
                //List<CategoryViewModel> objCategoryList = _unitOfWork.Category.GetAll().Select(x => new CategoryViewModel
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    DisplayOrder = x.DisplayOrder
                //}).ToList();
                return View(new List<ProductVM>());
            }

        }
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public async Task <IActionResult> Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                var jsonContent = JsonConvert.SerializeObject(productVM.Product); 
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"); 


                if (productVM.Product.Id == 0)
                {

                    var response = await _httpClient.PostAsync(_apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["success"] = "Product created successfully";

                    }
                else
                {
                        ModelState.AddModelError("", "Error creating product: " + response.ReasonPhrase);
                        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        });
                        return View(productVM);
                    }
                }
                else
                {
                    var response = await _httpClient.PutAsync($"{_apiUrl}/{productVM.Product.Id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["success"] = "Product updated successfully";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error updating product: " + response.ReasonPhrase);
                        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        });
                        return View(productVM);
                    }
                }

                return RedirectToAction("Index");

            }

            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion

    }
}

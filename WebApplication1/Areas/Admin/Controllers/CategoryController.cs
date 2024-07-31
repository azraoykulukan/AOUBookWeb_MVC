using AOUBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using AOUBook.DataAccess.Data;
using AOUBook.Models;
using Microsoft.AspNetCore.Authorization;
using AOUBook.Utility;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace AOUBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7200/api/Category";
        public CategoryController(IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;

        }

        public async Task <IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryViewModel>>(content);
                return View(categories);
            }
            else
            {
                //Handle error response
                //List<CategoryViewModel> objCategoryList = _unitOfWork.Category.GetAll().Select(x => new CategoryViewModel
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    DisplayOrder = x.DisplayOrder
                //}).ToList();
                return View(new List<CategoryViewModel>());
            }
            //List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            //return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display Order can not be same");
            }

            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                }

                //_unitOfWork.Category.Add(obj);
                //_unitOfWork.Save();
                //TempData["success"] = "Category created successfully";
                //return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<Category>(content);
                return View(category);
            }
            return NotFound();

            //Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();



            //if (categoryFromDb == null)
            //{
            //    return NotFound();
            //}
            //return View(categoryFromDb);
        }
        [HttpPost]
        public async Task <IActionResult> Edit(Category obj)
        {


            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Put, $"{_apiUrl}/{obj.Id}")
                {
                    Content = content
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
                }
                

                //_unitOfWork.Category.Update(obj);
                //_unitOfWork.Save();
                //TempData["success"] = "Category Updated Successfully";
                //return RedirectToAction("Index");

            }
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);


            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<Category>(content);
                return View(category);
            }
            return NotFound();

            //if (categoryFromDb == null)
            //{
            //    return NotFound();
            //}
            //return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public async Task <IActionResult> DeletePOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the category.");
                return View();
            }
            //Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            //if (obj == null)
            //{
            //    return NotFound();
            //}
            //_unitOfWork.Category.Remove(obj);
            //_unitOfWork.Save();
            //TempData["success"] = "Category deleted Successfully";
            //return RedirectToAction("Index");
        }
    }
}

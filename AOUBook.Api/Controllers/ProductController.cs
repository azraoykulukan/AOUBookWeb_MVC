using AOUBook.DataAccess.Repository.IRepository;
using AOUBook.Models;
using AOUBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


namespace AOUBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Ok(productList);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] ProductVM productVm)
        {

            if (ModelState.IsValid)
            {
                if (productVm.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(productVm.Product);
                }
                else
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                _unitOfWork.Save();
                return Ok(productVm);

            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return Ok(productVm);
            }

        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProductVM productVm)
        {
            if (id != productVm.Product.Id)
            {
                return BadRequest();
            }

            productVm.Product.Id = id;
            _unitOfWork.Product.Update(productVm.Product);
            _unitOfWork.Save();

            return Ok(productVm);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();

            return NoContent();
        }



    }
}

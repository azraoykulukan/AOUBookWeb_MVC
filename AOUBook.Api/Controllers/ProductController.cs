using AOUBook.Api.Models;
using AOUBook.Api.Validatior;
using AOUBook.DataAccess.Repository.IRepository;
using AOUBook.Models;
using AOUBook.Models.ViewModels;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using static AOUBook.Api.Models.ProductResponse;


namespace AOUBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductVM> _productValidator;


        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger, IMapper mapper, IValidator<ProductVM> productValidator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _productValidator = productValidator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AOUBook.Models.Product>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            IEnumerable<AOUBook.Models.Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            var mappedProduct = _mapper.Map<List<ProductResponse>>(productList);
            return Ok(mappedProduct);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AOUBook.Models.Product), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
            var mappedProduct = _mapper.Map<ProductResponse>(product);

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<AOUBook.Models.Product>), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] ProductVM productVm)
        {
            var validationResult = _productValidator.Validate(productVm);

            if (validationResult.IsValid)
            {
                var mappedProduct = _mapper.Map<Product>(productVm.Product); //Ahmet Bey'e soralım, productvm dto
                _unitOfWork.Product.Add(mappedProduct);
                _unitOfWork.Save();
                return Ok(mappedProduct);
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return BadRequest(validationResult.Errors);
            }
        }
    
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IEnumerable<AOUBook.Models.Product>), StatusCodes.Status200OK)]
        public IActionResult Put(int id, [FromBody] ProductVM productVm)
        {
            var validationResult = _productValidator.Validate(productVm);
            if (id != productVm.Product.Id)
            {
                return BadRequest();
            }
            if (validationResult.IsValid)
            {
                var mappedProduct = _mapper.Map<Product>(productVm.Product);
                _unitOfWork.Product.Update(productVm.Product);
                _unitOfWork.Save();
                return Ok(mappedProduct);
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return BadRequest(validationResult.Errors);
            }
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
            var mappedProduct = _mapper.Map<ProductResponse>(product);

            //return NoContent();
            return Ok(mappedProduct);
        }



    }
}

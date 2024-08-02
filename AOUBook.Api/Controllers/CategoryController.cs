using AOUBook.Api.Models;
using AOUBook.Api.Validatior;
using AOUBook.DataAccess.Repository.IRepository;
using AOUBook.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;


namespace AOUBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<Category> _categoryValidator;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<Category> categoryValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _categoryValidator = categoryValidator;
        }




        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var categoryList = _unitOfWork.Category.GetAll();
            
            if (categoryList == null)
            {
                return NotFound();
            }
            var mappedCategory = _mapper.Map<List<CategoryResponse>>(categoryList);
            return Ok(mappedCategory);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            var category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var mappedCategory = _mapper.Map<CategoryResponse>(category);
            return Ok(mappedCategory);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] Category category)
        {
            var validationResult = _categoryValidator.Validate(category);
            if (validationResult.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();

                var mappedCategory = _mapper.Map<CategoryResponse>(category);


                return Ok(mappedCategory);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            var validationResult = _categoryValidator.Validate(category);
            if (id != category.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();

                var mappedCategory = _mapper.Map<CategoryResponse>(category);

                return Ok(mappedCategory);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        public IActionResult Delete(int id) {
            var category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();

            var mappedCategory = _mapper.Map<CategoryResponse>(category);

            return Ok(mappedCategory);
        }

    }
}

using AOUBook.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AOUBook.Api.Models
{
    public class ProductResponse
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ISBN { get; set; }
            public double ListPrice { get; set; }
            public double Price { get; set; }

            public double Price50 { get; set; }

            public double Price100 { get; set; }

            public int CategoryId { get; set; }
            public Category Category { get; set; }
            public string ImageUrl { get; set; }
       
    }
}
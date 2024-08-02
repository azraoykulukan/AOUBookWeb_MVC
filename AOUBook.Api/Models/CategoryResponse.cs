using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AOUBook.Api.Models
{
    public class CategoryResponse
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AOUBook.Model
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}

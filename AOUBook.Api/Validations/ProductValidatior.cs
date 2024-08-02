using AOUBook.Models.ViewModels;
using FluentValidation;

namespace AOUBook.Api.Validatior
{



    public class ProductValidatior : AbstractValidator<ProductVM>
    {
        public ProductValidatior()
        {

            RuleFor(x => x.Product.Title).NotEmpty().WithMessage("Product title is required");
            RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required aasadsf");
            RuleFor(x => x.Product.ISBN).NotEmpty().WithMessage("ISBN is required");
            RuleFor(x => x.Product.Author).NotEmpty().WithMessage("Author is required");
            RuleFor(x => x.Product.ListPrice).NotEmpty().WithMessage("List Price is required");
            RuleFor(x => x.Product.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Product.Price).InclusiveBetween(1, 1000).WithMessage("Price must be between 1-1000");
            RuleFor(x => x.Product.Price50).NotEmpty().WithMessage("Price50 is required");
            RuleFor(x => x.Product.Price50).InclusiveBetween(1,1000).WithMessage("Price50 is AAA");
            RuleFor(x => x.Product.Price100).NotEmpty().WithMessage("Price100 is required");
            RuleFor(x => x.Product.Price100).InclusiveBetween(1, 1000).WithMessage("Price100 is BBB");
            RuleFor(x => x.Product.ImageUrl).NotEmpty().WithMessage("Image is required");
            
            
        }

    }
}

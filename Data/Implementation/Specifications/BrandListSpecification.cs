using WebApplication1.Models;

namespace WebApplication1.Data.Implementation.Specifications
{
    public class BrandListSpecification: BaseSpecification<Product, string>
    {
        public BrandListSpecification()
        {
           AddSelect(x => x.Brand);
            ApplyDistinct();
        }
    }
}

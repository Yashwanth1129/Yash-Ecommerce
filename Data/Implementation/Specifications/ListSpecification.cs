using WebApplication1.Models;

namespace WebApplication1.Data.Implementation.Specifications
{
    public class ListSpecification: BaseSpecification<Product, string>
    {
        public ListSpecification()
        {
            AddSelect(x=> x.Type);
            ApplyDistinct();
        }
    }
}

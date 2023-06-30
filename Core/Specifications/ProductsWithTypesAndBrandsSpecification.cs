using Core.Entities;
using Core.Helper;
using Core.Params;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecificationParams productParams)
        : base(SpecificationHelper.BuildProductExpression(productParams))
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderBy(x => x.Name);
        ApplyPaging(
            productParams.PageSize * (productParams.PageIndex - 1), 
            productParams.PageSize
        );

        if(!string.IsNullOrEmpty(productParams.Sort))
        {
            switch(productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(n => n.Name);
                    break;
            }
        }
    }
}

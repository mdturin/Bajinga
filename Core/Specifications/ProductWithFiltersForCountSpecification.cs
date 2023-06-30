using Core.Entities;
using Core.Helper;
using Core.Params;

namespace Core.Specifications;

public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecificationParams productParams)
        : base(SpecificationHelper.BuildProductExpression(productParams)) { }
}

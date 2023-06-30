using Core.Entities;
using Core.Params;
using System.Linq.Expressions;

namespace Core.Helper;

public class SpecificationHelper
{
    public static Expression<Func<Product, bool>> 
        BuildProductExpression(ProductSpecificationParams productParams)
    {
        var expressions = new List<Expression<Func<Product, bool>>>();

        var searchExpression = string.IsNullOrEmpty(productParams.Search)
            ? (Expression<Func<Product, bool>>)null
            : x => x.Name.ToLower().Contains(productParams.Search);
        expressions.Add(searchExpression);

        var brandIdExpression = !productParams.BrandId.HasValue
            ? (Expression<Func<Product, bool>>)null
            : x => x.ProductBrandId == productParams.BrandId;
        expressions.Add(brandIdExpression);

        var typeIdExpression = !productParams.TypeId.HasValue
            ? (Expression<Func<Product, bool>>)null
            : x => x.ProductTypeId == productParams.TypeId;
        expressions.Add(typeIdExpression);

        Expression<Func<Product, bool>> finalExpression = (x => true);
        foreach(var expression in expressions)
        {
            if(expression == null) continue;
            finalExpression = Expression.Lambda<Func<Product, bool>>(
                Expression.AndAlso(finalExpression.Body, expression.Body),
                finalExpression.Parameters[0]
            );
        }

        return finalExpression;
    }
}

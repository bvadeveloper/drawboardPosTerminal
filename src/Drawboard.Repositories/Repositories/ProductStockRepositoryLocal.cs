using Drawboard.Contracts.Exceptions;
using Drawboard.Entities.Entities;
using Drawboard.Repositories.Abstractions;
using Microsoft.Extensions.Options;

namespace Drawboard.Repositories.Repositories;

public class ProductStockRepositoryLocal : IProductStockRepository
{
    /// <summary>
    /// Products data storage
    /// </summary>
    private readonly IEnumerable<ProductEntity> _products;

    public ProductStockRepositoryLocal(IOptions<List<ProductEntity>> options) => _products = options.Value;

    /// <summary>
    /// Find a product by code (case sensitive)
    /// </summary>
    /// <param name="productCode">product code.</param>
    /// <returns>product entity.</returns>
    public ProductEntity FindProductByCode(string productCode) =>
        _products.FirstOrDefault(model => model.ProductCode == productCode) // warning case sensitive
        ?? throw new ProductNotFoundException($"Product with code '{productCode}' not found");
}
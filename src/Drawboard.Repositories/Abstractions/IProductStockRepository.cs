using Drawboard.Entities.Entities;

namespace Drawboard.Repositories.Abstractions;

public interface IProductStockRepository
{
    /// <summary>
    /// Find a product by code
    /// </summary>
    /// <param name="productCode">product code.</param>
    /// <returns>product entity.</returns>
    public ProductEntity FindProductByCode(string productCode);
}
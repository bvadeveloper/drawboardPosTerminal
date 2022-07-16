using Drawboard.Contracts.Enums;
using Drawboard.Contracts.Exceptions;
using Drawboard.Entities.Entities;
using Drawboard.PosTerminal.Abstractions;
using Drawboard.PosTerminal.Extensions;
using Drawboard.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace Drawboard.PosTerminal;

public class PosTerminalLocal : IPosTerminal
{
    private readonly IProductStockRepository _productStockRepository;
    private readonly IDiscountCalculatorFactory _discountCalculatorFactory;
    private readonly IUserInterface _userInterface;
    private readonly ILogger _logger;

    private readonly ReceiptEntity _receipt;

    public PosTerminalLocal(
        IProductStockRepository productStockRepository,
        IDiscountCalculatorFactory discountCalculatorFactory,
        IUserInterface userInterface,
        ILogger<PosTerminalLocal> logger)
    {
        _productStockRepository = productStockRepository;
        _discountCalculatorFactory = discountCalculatorFactory;
        _userInterface = userInterface;
        _logger = logger;
        
        _receipt = ReceiptEntity.New;
    }

    /// <summary>
    /// Scan product code
    /// </summary>
    /// <param name="code">scan code.</param>
    public void Scan(string code)
    {
        if (!IsValidInput(code))
        {
            _userInterface.ShowWarning("Sorry, the input is not recognized.");
            return;
        }

        try
        {
            var productEntity = _productStockRepository.FindProductByCode(code);

            var (productCode, cost, count) = _discountCalculatorFactory
                .Make(DiscountType.CountOfProducts)
                .Calculate(_receipt.ReceiptItems, productEntity);
            
            _receipt.ReceiptItems.AddOrUpdateItem(productCode, cost, count);
        }
        catch (Exception e)
        {
            HandleException(e, code);
        }
    }

    private void HandleException(Exception exception, string code)
    {
        switch (exception)
        {
            case ProductNotFoundException notFoundException:
                _userInterface.ShowWarning($"Sorry, product with code '{code}' not found!");
                _logger.LogWarning(notFoundException, $"A product not found exception occurred with error id '{notFoundException.ErrorContextId}'. Receipt Id '{_receipt.ReceiptId}'.");
                return;

            case BusinessException businessException:
                _logger.LogError(businessException, $"A business exception occurred with error Id '{businessException.ErrorContextId}'. Exception message '{businessException.Message}'. Receipt Id '{_receipt.ReceiptId}'");
                return;

            case Exception ex:
                _logger.LogError(ex, $"An unhandled exception occurred. Exception message '{ex.Message}'. Receipt Id '{_receipt.ReceiptId}'");
                return;

            default: // we don't hide the original stack, just keep it in inner exception
                throw new Exception("Something went wrong.", exception);
        }
    }

    /// <summary>
    /// Print all info about purchase
    /// </summary>
    public void PrintReceipt()
    {
        var totalCost = _receipt.ReceiptItems.Sum(pair => pair.Value.Cost);
        var totalCount = _receipt.ReceiptItems.Sum(pair => pair.Value.Count);
        var message = $"Receipt Id '{_receipt.ReceiptId}' with Total Cost '${totalCost}' and Total Count '{totalCount}'";

        _userInterface.ShowInfo(message);

        // calling hardware management subsystem ...
    }

    /// <summary>
    /// Some input verification
    /// We usually use FluentValidator for such cases.
    /// </summary>
    /// <param name="input">input.</param>
    /// <returns></returns>
    private static bool IsValidInput(string input) => !string.IsNullOrWhiteSpace(input);

    public void Dispose()
    {
        // disposing here
    }
}
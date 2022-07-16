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
    private readonly IUserInterface _userInterface;
    private readonly ILogger _logger;

    private readonly ReceiptEntity _receipt;

    public PosTerminalLocal(
        IProductStockRepository productStockRepository,
        IUserInterface userInterface,
        ILogger<PosTerminalLocal> logger)
    {
        _productStockRepository = productStockRepository;
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
            var (productCode, cost, count) = _receipt.CalculateDiscount(productEntity);

            _receipt.ReceiptItems.AddOrUpdateItem(productCode, cost, count);
        }
        catch (ProductNotFoundException productNotFoundException)
        {
            // show message for user
            _userInterface.ShowWarning($"Sorry, product with code '{code}' not found!");
            _logger.LogWarning(productNotFoundException,
                $"A product not found exception occurred with error id '{productNotFoundException.ErrorContextId}'. Receipt Id '{_receipt.ReceiptId}'.");
        }

        // do all stuff for alerting here for unhandled exceptions (pagerduty, send message to hot chanel, etc)

        catch (BusinessException businessException)
        {
            _logger.LogError(businessException,
                $"A business exception occurred with error Id '{businessException.ErrorContextId}'. Exception message '{businessException.Message}'. Receipt Id '{_receipt.ReceiptId}'");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                $"An unhandled exception occurred. Exception message '{exception.Message}'. Receipt Id '{_receipt.ReceiptId}'");
        }
    }

    /// <summary>
    /// Print all info in receipt
    /// </summary>
    public void PrintReceipt()
    {
        var totalCost = _receipt.ReceiptItems.Sum(pair => pair.Value.Cost);
        var totalCount = _receipt.ReceiptItems.Sum(pair => pair.Value.Count);
        var message = $"Receipt Id '{_receipt.ReceiptId}' with Total Price '{totalCost}' and Total Count '{totalCount}'";

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
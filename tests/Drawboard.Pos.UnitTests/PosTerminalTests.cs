using Drawboard.Contracts.Exceptions;
using Drawboard.Entities.Entities;
using Drawboard.PosTerminal;
using Drawboard.PosTerminal.Abstractions;
using Drawboard.PosTerminal.Discounts;
using Drawboard.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Drawboard.Pos.UnitTests;

[TestFixture]
public class PosTerminalTests
{
    private IPosTerminal _posTerminal;
    private UserInterfaceTest _userInterfaceTest;
    private Mock<IProductStockRepository> _productStockRepository;

    private readonly ProductEntity _productA = new("A", 1.25m, 3.00m, 3);
    private readonly ProductEntity _productB = new("B", 4.25m, 0m, 0);
    private readonly ProductEntity _productC = new("C", 1m, 5m, 6);
    private readonly ProductEntity _productD = new("D", 0.75m, 0m, 0);

    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<CountOfProductsDiscountCalculator>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var discountCalculatorFactory = new DiscountCalculatorFactory(serviceProvider);
        var logger = new Mock<ILogger<PosTerminalLocal>>();

        _userInterfaceTest = new UserInterfaceTest();
        _productStockRepository = new Mock<IProductStockRepository>();
        _posTerminal = new PosTerminalLocal(_productStockRepository.Object, discountCalculatorFactory, _userInterfaceTest, logger.Object);
    }
    
    [Test]
    public void Scan_product_A_as_result_valid_receipt_data()
    {
        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productA.ProductCode))
            .Returns(() => _productA);

        _posTerminal.Scan(_productA.ProductCode);

        _posTerminal.PrintReceipt();

        Assert.Multiple(() =>
        {
            Assert.That(_userInterfaceTest.InfoMessage,
                Contains.Substring("with Total Cost '$1.25' and Total Count '1'"));
            Assert.That(_userInterfaceTest.WarningMessage, Is.Null);
        });

        _productStockRepository.VerifyAll();
    }
    
    [Test]
    public void Scan_products_in_order_ABCD_as_result_valid_receipt_data()
    {
        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productA.ProductCode))
            .Returns(() => _productA);

        _posTerminal.Scan(_productA.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productB.ProductCode))
            .Returns(() => _productB);

        _posTerminal.Scan(_productB.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productC.ProductCode))
            .Returns(() => _productC);

        _posTerminal.Scan(_productC.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productD.ProductCode))
            .Returns(() => _productD);

        _posTerminal.Scan(_productD.ProductCode);


        _posTerminal.PrintReceipt();

        Assert.Multiple(() =>
        {
            Assert.That(_userInterfaceTest.InfoMessage,
                Contains.Substring("with Total Cost '$7.25' and Total Count '4'"));
            Assert.That(_userInterfaceTest.WarningMessage, Is.Null);
        });

        _productStockRepository.VerifyAll();
    }
    
    [Test]
    public void Scan_products_in_order_CCCCCCC_as_result_valid_receipt_data()
    {
        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productC.ProductCode))
            .Returns(() => _productC);

        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);
        _posTerminal.Scan(_productC.ProductCode);

        _posTerminal.PrintReceipt();

        Assert.Multiple(() =>
        {
            Assert.That(_userInterfaceTest.InfoMessage, Contains.Substring("with Total Cost '$6' and Total Count '7'"));
            Assert.That(_userInterfaceTest.WarningMessage, Is.Null);
        });

        _productStockRepository.VerifyAll();
    }
    
    [Test]
    public void Scan_products_in_order_ABCDABA_as_result_valid_receipt_data()
    {
        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productA.ProductCode))
            .Returns(() => _productA);

        _posTerminal.Scan(_productA.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productB.ProductCode))
            .Returns(() => _productB);

        _posTerminal.Scan(_productB.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productC.ProductCode))
            .Returns(() => _productC);

        _posTerminal.Scan(_productC.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productD.ProductCode))
            .Returns(() => _productD);

        _posTerminal.Scan(_productD.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productA.ProductCode))
            .Returns(() => _productA);

        _posTerminal.Scan(_productA.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productB.ProductCode))
            .Returns(() => _productB);

        _posTerminal.Scan(_productB.ProductCode);


        _productStockRepository
            .Setup(repository => repository.FindProductByCode(_productA.ProductCode))
            .Returns(() => _productA);

        _posTerminal.Scan(_productA.ProductCode);

        _posTerminal.PrintReceipt();

        Assert.Multiple(() =>
        {
            Assert.That(_userInterfaceTest.InfoMessage,
                Contains.Substring("with Total Cost '$13.25' and Total Count '7'"));
            Assert.That(_userInterfaceTest.WarningMessage, Is.Null);
        });

        _productStockRepository.VerifyAll();
    }
    
    [Test]
    public void Scan_not_exist_product_with_result_product_not_found()
    {
        const string productCodeX = "X";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeX))
            .Returns(() => throw new ProductNotFoundException($"Product with code '{productCodeX}' not found"));

        _posTerminal.Scan(productCodeX);

        _posTerminal.PrintReceipt();

        Assert.Multiple(() =>
        {
            Assert.That(_userInterfaceTest.WarningMessage, Is.EqualTo("Sorry, product with code 'X' not found!"));
            Assert.That(_userInterfaceTest.InfoMessage, Contains.Substring("with Total Cost '$0' and Total Count '0'"));
        });

        _productStockRepository.VerifyAll();
    }
}
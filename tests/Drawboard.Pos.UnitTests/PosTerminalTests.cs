using Drawboard.Contracts.Exceptions;
using Drawboard.Entities.Entities;
using Drawboard.PosTerminal;
using Drawboard.PosTerminal.Abstractions;
using Drawboard.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Drawboard.Pos.UnitTests;

[TestFixture]
public class PosTerminalTests
{
    private IPosTerminal _posTerminal;
    private TestMessenger _messenger;
    private Mock<IProductStockRepository> _productStockRepository;

    [SetUp]
    public void Setup()
    {
        _messenger = new TestMessenger();
        _productStockRepository = new Mock<IProductStockRepository>();
        _posTerminal = new PosTerminalLocal(_productStockRepository.Object, _messenger,
            new Mock<ILogger<PosTerminalLocal>>().Object);
    }

    /// <summary>
    /// Test product A
    /// </summary>
    /// <param name="productCode"></param>
    /// <param name="unitPrice"></param>
    /// <param name="packPrice"></param>
    /// <param name="discountNumberItems"></param>
    [Test]
    [TestCase("A", 1.25, 3.00, 3)]
    public void TestProduct_A(string productCode, decimal unitPrice, decimal packPrice, int discountNumberItems)
    {
        _productStockRepository.Setup(repository => repository.FindProductByCode(productCode))
            .Returns(() => new ProductEntity(productCode, unitPrice, packPrice, discountNumberItems));

        _posTerminal.Scan(productCode);
        _posTerminal.PrintReceipt();
        
        Assert.Multiple(() =>
        {
            Assert.That(_messenger.InfoMessage, Contains.Substring("with Total Price '1.25' and Total Count '1'"));
            Assert.That(_messenger.WarningMessage, Is.Null);
        });
        
        _productStockRepository.VerifyAll();
    }

    /// <summary>
    /// Scan products in order ABCD
    /// </summary>
    [Test]
    public void TestProducts_ABCD()
    {
        const string productCodeA = "A";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeA))
            .Returns(() => new ProductEntity(productCodeA, 1.25m, 3.00m, 3));

        _posTerminal.Scan(productCodeA);

        const string productCodeB = "B";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeB))
            .Returns(() => new ProductEntity(productCodeB, 4.25m, 0m, 0));

        _posTerminal.Scan(productCodeB);

        const string productCodeC = "C";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeC))
            .Returns(() => new ProductEntity(productCodeC, 1m, 5m, 6));

        _posTerminal.Scan(productCodeC);

        const string productCodeD = "D";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeD))
            .Returns(() => new ProductEntity(productCodeD, 0.75m, 0m, 0));

        _posTerminal.Scan(productCodeD);

        _posTerminal.PrintReceipt();
        
        Assert.Multiple(() =>
        {
            Assert.That(_messenger.InfoMessage, Contains.Substring("with Total Price '7.25' and Total Count '4'"));
            Assert.That(_messenger.WarningMessage, Is.Null);
        });
        
        _productStockRepository.VerifyAll();
    }

    /// <summary>
    /// Scan products in order CCCCCCC
    /// </summary>
    [Test]
    public void TestProducts_CCCCCCC()
    {
        const string productCodeC = "C";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeC))
            .Returns(() => new ProductEntity(productCodeC, 1m, 5m, 6));

        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        _posTerminal.Scan(productCodeC);
        
        _posTerminal.PrintReceipt();
        
        Assert.Multiple(() =>
        {
            Assert.That(_messenger.InfoMessage, Contains.Substring("with Total Price '6' and Total Count '7'"));
            Assert.That(_messenger.WarningMessage, Is.Null);
        });
        
        _productStockRepository.VerifyAll();
    }

    /// <summary>
    /// Scan products in order ABCDABA
    /// </summary>
    [Test]
    public void TestProducts_ABCDABA()
    {
        const string productCodeA = "A";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeA))
            .Returns(() => new ProductEntity(productCodeA, 1.25m, 3.00m, 3));

        _posTerminal.Scan(productCodeA);

        const string productCodeB = "B";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeB))
            .Returns(() => new ProductEntity(productCodeB, 4.25m, 0m, 0));

        _posTerminal.Scan(productCodeB);

        const string productCodeC = "C";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeC))
            .Returns(() => new ProductEntity(productCodeC, 1m, 5m, 6));

        _posTerminal.Scan(productCodeC);

        const string productCodeD = "D";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeD))
            .Returns(() => new ProductEntity(productCodeD, 0.75m, 0m, 0));

        _posTerminal.Scan(productCodeD);


        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeA))
            .Returns(() => new ProductEntity(productCodeA, 1.25m, 3.00m, 3));

        _posTerminal.Scan(productCodeA);


        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeB))
            .Returns(() => new ProductEntity(productCodeB, 4.25m, 0m, 0));

        _posTerminal.Scan(productCodeB);


        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeA))
            .Returns(() => new ProductEntity(productCodeA, 1.25m, 3.00m, 3));

        _posTerminal.Scan(productCodeA);

        _posTerminal.PrintReceipt();
        
        Assert.Multiple(() =>
        {
            Assert.That(_messenger.InfoMessage, Contains.Substring("with Total Price '13.25' and Total Count '7'"));
            Assert.That(_messenger.WarningMessage, Is.Null);
        });
        
        _productStockRepository.VerifyAll();
    }

    /// <summary>
    /// Product not exist
    /// </summary>
    [Test]
    public void TestProduct_X()
    {
        const string productCodeX = "X";

        _productStockRepository.Setup(repository => repository.FindProductByCode(productCodeX))
            .Returns(() => throw new ProductNotFoundException($"Product with code '{productCodeX}' not found"));

        _posTerminal.Scan(productCodeX);

        _posTerminal.PrintReceipt();
        
        Assert.Multiple(() =>
        {
            Assert.That(_messenger.WarningMessage, Is.EqualTo("Sorry, product with code 'X' not found!"));
            Assert.That(_messenger.InfoMessage, Contains.Substring("with Total Price '0' and Total Count '0'"));
        });
        
        _productStockRepository.VerifyAll();
    }
}
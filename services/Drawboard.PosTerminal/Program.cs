using System.Reflection;
using Drawboard.Entities.Entities;
using Drawboard.PosTerminal.Abstractions;
using Drawboard.PosTerminal.Discounts;
using Drawboard.Repositories.Abstractions;
using Drawboard.Repositories.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Drawboard.PosTerminal
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            await Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config
                        .SetBasePath(Directory.GetParent(Assembly.GetExecutingAssembly().Location)!.FullName)
                        .AddJsonFile("products.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHostedService<Seller>()
                        .AddScoped<IPosTerminal, PosTerminalLocal>()
                        .AddScoped<IUserInterface, UserInterfaceConsole>()
                        .AddScoped<IProductStockRepository, ProductStockRepositoryLocal>()

                        // discounts calculator registration
                        .AddScoped<IDiscountCalculatorFactory, DiscountCalculatorFactory>()
                        .AddScoped<CountOfProductsDiscountCalculator>();
                    
                    services.Configure<List<ProductEntity>>(options =>
                        context.Configuration.GetSection("Products").Bind(options));
                })
                .ConfigureLogging(builder => builder.AddConsole())
                .Build()
                .RunAsync();
    }
}
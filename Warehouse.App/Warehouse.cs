using Microsoft.Extensions.DependencyInjection;
using Warehouse.Services;

namespace Warehouse.App
{
    static class Warehouse
    {
        static IWarehouseService warehouseService = Program.ServiceProvider.GetService<IWarehouseService>();

        public static async Task Products()
        {
            var products = await warehouseService.GetProducts();

            foreach (var p in products)
            {
                Console.WriteLine($"{p.ProductId},{p.ProductName}");
            }
        }

        public static async Task RetailerProducts()
        {
            var retailerProducts = await warehouseService.GetRetailerProducts();

            foreach (var p in retailerProducts)
            {
                Console.WriteLine($"{p.ProductId},{p.RetailerName},{p.RetailerProductCode},{p.RetailerProductCodeType},{p.DateReceived}");
            }
        }

        public static async Task OutputProducts()
        {
            var outputProducts = await warehouseService.GetOutputProducts();

            foreach (var p in outputProducts)
            {
                Console.WriteLine($"{p.ProductId},{p.ProductName},{p.CodeType},{p.Code}");
            }
        }
    }
}

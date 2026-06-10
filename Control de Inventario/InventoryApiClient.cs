using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Control_de_Inventario
{
    internal sealed class InventoryApiClient
    {
        private readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5071/")
        };

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            using var response = await httpClient.GetAsync("api/products");
            var content = await ReadSuccessfulContentAsync(response);
            var products = JsonConvert.DeserializeObject<List<ProductResponse>>(content) ?? new();

            return products
                .Select(product => new Producto
                {
                    Id = product.Id,
                    Nombre = product.Name,
                    Stock = product.Stock,
                    MinimumStock = product.MinimumStock
                })
                .ToList();
        }

        public async Task<List<Movimiento>> ObtenerMovimientosAsync()
        {
            using var response = await httpClient.GetAsync("api/stock-movements");
            var content = await ReadSuccessfulContentAsync(response);
            var movements = JsonConvert.DeserializeObject<List<StockMovementResponse>>(content) ?? new();

            return movements
                .Select(movement => new Movimiento
                {
                    Producto = movement.ProductName,
                    Cantidad = movement.Quantity,
                    Tipo = movement.Type == "Entry" ? "Ingreso" : "Egreso",
                    Fecha = movement.CreatedAtUtc.ToLocalTime()
                })
                .ToList();
        }

        public async Task CrearProductoAsync(string nombre, int stock)
        {
            var request = new
            {
                name = nombre,
                stock,
                minimumStock = 5
            };

            using var response = await httpClient.PostAsync("api/products", CreateJsonContent(request));
            await ReadSuccessfulContentAsync(response);
        }

        public async Task IngresarStockAsync(int productId, int quantity)
        {
            await SendStockOperationAsync(productId, quantity, "increase");
        }

        public async Task DescontarStockAsync(int productId, int quantity)
        {
            await SendStockOperationAsync(productId, quantity, "decrease");
        }

        public async Task EliminarProductoAsync(int productId)
        {
            using var response = await httpClient.DeleteAsync($"api/products/{productId}");
            await ReadSuccessfulContentAsync(response);
        }

        private async Task SendStockOperationAsync(int productId, int quantity, string operation)
        {
            var request = new
            {
                quantity
            };

            using var response = await httpClient.PostAsync(
                $"api/products/{productId}/stock/{operation}",
                CreateJsonContent(request));

            await ReadSuccessfulContentAsync(response);
        }

        private static StringContent CreateJsonContent(object value)
        {
            return new StringContent(
                JsonConvert.SerializeObject(value),
                Encoding.UTF8,
                "application/json");
        }

        private static async Task<string> ReadSuccessfulContentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return content;
            }

            var error = JsonConvert.DeserializeObject<ApiError>(content);
            var message = string.IsNullOrWhiteSpace(error?.Message)
                ? $"La API respondio con error {(int)response.StatusCode}."
                : error.Message;

            throw new InvalidOperationException(message);
        }

        private sealed class ProductResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Stock { get; set; }
            public int MinimumStock { get; set; }
        }

        private sealed class StockMovementResponse
        {
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public string Type { get; set; } = string.Empty;
            public DateTime CreatedAtUtc { get; set; }
        }

        private sealed class ApiError
        {
            public string? Message { get; set; }
        }
    }
}

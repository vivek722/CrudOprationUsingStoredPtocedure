using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spprectise
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task DeleteProduct(int id);
        Task UpdateProduct(Product product);
        Task<Product> GetByIdProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }

    public class Repository : IProductRepository
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddProduct(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("InsertProduct", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@name", product.name);
            command.Parameters.AddWithValue("@price", product.Price);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteProduct(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("DeleteProduct", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = new List<Product>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetAllProducts", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }
            return products;
        }

        public async Task<Product> GetByIdProduct(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetByIdProduct", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                };
            }
            return null;
        }

        public async Task UpdateProduct(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("UpdateProduct", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@ID", product.Id);
            command.Parameters.AddWithValue("@name", product.name);
            command.Parameters.AddWithValue("@price", product.Price);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}

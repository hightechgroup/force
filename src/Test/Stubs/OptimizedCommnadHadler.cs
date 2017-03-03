using System;

using System.Data.SqlClient;
using System.Threading.Tasks;
using CostEffectiveCode.Cqrs;
using Dapper;

namespace CostEffectiveCode.Tests
{
    public class OptimizedCommnadHadler
        : IAsyncHandler<CreateProductDto, int>
    {

        public int Price { get; set; } = 5;


        public async Task<int> Handle(CreateProductDto input)
        {
            using (var sqlConnection
                = new SqlConnection(""))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.ExecuteAsync("INSERT INTO Products(Category_Id, Name, Price) " +
                                           "VALUES (@CategoryId, @Name, @Price)", new
                                           {
                                               CategoryId = input.CategoryId,
                                               Name = input.Name,
                                               Price = input.Price
                                           });
            }
        }
    }
}

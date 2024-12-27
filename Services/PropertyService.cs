using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using api.Models;

namespace api.Services
{
    public interface IPropertyService
    {
        Task<Property?> GetPropertyByIdAsync(string propertyId);
    }

    public class PropertyService : IPropertyService
    {
        private readonly IConfiguration _configuration;

        public PropertyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Property?> GetPropertyByIdAsync(string propertyId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    ""propertyID"", ""flatNumber"", ""streetNumber"", ""streetName"", 
                    ""streetType"", ""streetTypeLong"", ""suburb"", ""state"", 
                    ""postcode"", ""propertyType"", ""bedrooms"", ""bathrooms"", 
                    ""carSpaces"", ""landSize"", ""buildingSize"", ""lastListedDate"", 
                    ""lastSoldDate"", ""lastRentedDate"", ""lastListedPrice"", 
                    ""lastSoldPrice"", ""lastRentedPrice"", ""ownerRenter"", ""marketStatus""
                FROM properties 
                WHERE ""propertyID"" = @PropertyId";

            return await connection.QueryFirstOrDefaultAsync<Property>(sql, new { PropertyId = propertyId });
        }
    }
}
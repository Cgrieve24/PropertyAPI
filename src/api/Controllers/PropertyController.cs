using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(IPropertyService propertyService, ILogger<PropertyController> logger)
        {
            _propertyService = propertyService;
            _logger = logger;
        }

        [HttpGet("{propertyId}")]
        [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<PropertyResponse>>> GetProperty(string propertyId)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(propertyId);
                
                if (property == null)
                {
                    return NotFound(new ApiResponse<PropertyResponse>("error", $"Property with ID {propertyId} not found.", null));
                }

                // Map the property to the new response structure
                var response = new PropertyResponse
                {
                    Attributes = new PropertyAttributes
                    {
                        PropertyID = property.PropertyId ?? "Unknown",
                        FlatNumber = property.FlatNumber ?? "Unknown",
                        StreetNumber = property.StreetNumber ?? "Unknown",
                        StreetName = property.StreetName ?? "Unknown",
                        Suburb = property.Suburb ?? "Unknown",
                        State = property.State ?? "Unknown",
                        Postcode = property.Postcode ?? "Unknown",
                        PropertyType = property.PropertyType ?? "Unknown",
                        Bedrooms = property.Bedrooms,
                        Bathrooms = property.Bathrooms,
                        CarSpaces = property.CarSpaces,
                        LandSize = property.LandSize,
                        BuildingSize = property.BuildingSize
                    },
                    SalesHistory = new SalesHistory
                    {
                        LastListedDate = property.LastListedDate,
                        LastSoldDate = property.LastSoldDate,
                        LastRentedDate = property.LastRentedDate,
                        LastListedPrice = property.LastListedPrice,
                        LastSoldPrice = property.LastSoldPrice,
                        LastRentedPrice = property.LastRentedPrice
                    },
                    AdditionalInfo = new AdditionalInfo
                    {
                        OwnerRenter = property.OwnerRenter,
                        MarketStatus = property.MarketStatus
                    }
                };

                return Ok(new ApiResponse<PropertyResponse>("success", "Property retrieved successfully.", response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property {PropertyId}", propertyId);
                return StatusCode(500, new ApiResponse<PropertyResponse>("error", "An error occurred while retrieving the property.", null));
            }
        }
    }
} 
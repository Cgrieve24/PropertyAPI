using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;

namespace api.Controllers
{
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
        [ProducesResponseType(typeof(Property), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Property>> GetProperty(string propertyId)
        {
            try
            {
                var property = await _propertyService.GetPropertyByIdAsync(propertyId);
                
                if (property == null)
                    return NotFound($"Property with ID {propertyId} not found.");

                return Ok(property);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property {PropertyId}", propertyId);
                return StatusCode(500, "An error occurred while retrieving the property.");
            }
        }
    }
} 
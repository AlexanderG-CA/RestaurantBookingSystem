namespace RestaurantBookingSystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RestaurantBookingSystem.DTOs.Requests;
    using RestaurantBookingSystem.DTOs.Response;
    using RestaurantBookingSystem.Interfaces;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authResponse = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (authResponse == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(authResponse);
        }
    }
}
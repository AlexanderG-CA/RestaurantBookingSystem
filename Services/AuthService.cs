
    using Microsoft.IdentityModel.Tokens;
    using RestaurantBookingSystem.DTOs.Response;
    using RestaurantBookingSystem.Interfaces;
    using RestaurantBookingSystem.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
public class AuthService : IAuthService
{
    private readonly IRepository<Administrator> _adminRepository;
    private readonly IConfiguration _config;

    public AuthService(IRepository<Administrator> adminRepository, IConfiguration config)
    {
        _adminRepository = adminRepository;
        _config = config;
    }

    public async Task<AuthResponse> AuthenticateAsync(string username, string password)
    {
        var admin = (await _adminRepository.GetAllAsync())
            .FirstOrDefault(a => a.Username == username);

        if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
            return null;

        var token = GenerateJwtToken(admin);
        return new AuthResponse { Token = token, Expires = DateTime.Now.AddHours(2) };
    }

    public Task AuthenticateAsync(object username, string password)
    {
        throw new NotImplementedException();
    }

    private string GenerateJwtToken(Administrator admin)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim(ClaimTypes.Name, admin.Username) },
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
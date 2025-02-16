using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Infrastructure.Email.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlightReservation.Infrastructure.Email.Services;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtsetting;

    public JwtService(IOptions<JwtSetting> jwtsetting)
    {
        _jwtsetting = jwtsetting.Value;
    }

    public string GenerateToken(int userId)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", userId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsetting.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtsetting.Issuer,
            _jwtsetting.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtsetting.ExpiryInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token).ToString();
    }

    public int GetUserId(string token)
    {
        if (string.IsNullOrEmpty(token))
           throw new ArgumentNullException(nameof(token)); 

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token);
        var tokenS = handler.ReadToken(token) as JwtSecurityToken;

        return Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "Id").Value);
       
    }
}

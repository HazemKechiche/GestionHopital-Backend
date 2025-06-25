using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Services/TokenService.cs
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TE.Core.Services;
using TE.Core.Domain;

public class TokenService
{
    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role,
                user is Admin ? "Admin" :
                user is Medecin ? "Medecin" :
                                     "Patient")

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyUltraS3cur3AndRand0mKey!@#2025"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "SGPH",
            audience: "SGPH",
            claims: claims,
            expires: DateTime.Now.AddHours(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

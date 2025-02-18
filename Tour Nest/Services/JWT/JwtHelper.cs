﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TourNest.Models.JWT_Refresh_Token;

namespace TourNest.Services.JWT;

public class JwtHelper(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public TokenModel GenerateTokens(string userId, string username)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        // Generate Access Token
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["AccessTokenExpirationMinutes"]));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, userId),  // Should be a valid GUID
    new Claim(ClaimTypes.NameIdentifier, userId),    // Should be a valid GUID
    new Claim(ClaimTypes.Name, username),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};


        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: accessTokenExpiration,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        // Generate Refresh Token
        var refreshToken = Guid.NewGuid().ToString();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["RefreshTokenExpirationDays"]));

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshTokenExpiration
        };
    }



}


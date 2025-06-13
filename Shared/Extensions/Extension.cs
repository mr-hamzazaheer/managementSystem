using Microsoft.IdentityModel.Tokens;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Extensions
{
    public static class Extension
    {
        public static string ToDateFormatString(this DateTime dateTime, bool useCustomFormat = false, string customFormat = "dd/MM/yyyy")
        {
            string format = useCustomFormat ? customFormat : "yyyy-MM-dd";
            return dateTime.ToString(format);
        }
        public static string GenerateToken(this Jwt jwt, IEnumerable<Claim> claims, bool isRemember)
        {
            if (jwt == null) throw new ArgumentNullException(nameof(jwt));
            if (string.IsNullOrWhiteSpace(jwt.SecretKey)) throw new ArgumentException("JWT key is not configured.");
            if (jwt.ExpiryMinutes <= 0) throw new ArgumentException("Invalid token expiry duration.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(isRemember ? 525600 : jwt.ExpiryMinutes);
            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

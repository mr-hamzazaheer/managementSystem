using Microsoft.IdentityModel.Tokens;
using Shared.Common;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text; 
using System.IO.Compression;

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
        public static string GetEnumDisplayName(this Enum enumValue)
        {
            var attribute = enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>();
            if (ReferenceEquals(attribute, null))
                return enumValue.ToString();
            else
                return attribute.GetName();
        }
        public static string Encode(this object input)
        {
            if (input == null) return string.Empty;

            var text = input.ToString();
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var raw = Encoding.UTF8.GetBytes(text);
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
                gzip.Write(raw, 0, raw.Length);

            var compressed = output.ToArray();
            var lengthPrefix = BitConverter.GetBytes(raw.Length);

            var finalData = new byte[compressed.Length + 4];
            Buffer.BlockCopy(lengthPrefix, 0, finalData, 0, 4);
            Buffer.BlockCopy(compressed, 0, finalData, 4, compressed.Length);

            return Convert.ToBase64String(finalData)
                .Replace("+", "_@_")
                .Replace("=", "_@@_")
                .Replace("&", "_endo_")
                .Replace("/", "_~_");
        }

        public static string Decode(this string encoded)
        {
            if (string.IsNullOrWhiteSpace(encoded)) return string.Empty;

            var base64 = encoded
                .Replace("_@_", "+")
                .Replace("_@@_", "=")
                .Replace("_endo_", "&")
                .Replace("_~_", "/");

            var allData = Convert.FromBase64String(base64);
            var length = BitConverter.ToInt32(allData, 0);

            using var input = new MemoryStream(allData, 4, allData.Length - 4);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            var buffer = new byte[length];
            gzip.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }

    }
}

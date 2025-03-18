using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;

namespace Api.Rest;

public class CustomJwtSecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly string _secret;
        
        public CustomJwtSecurityTokenValidator(string secret)
        {
            _secret = secret;
        }
        
        public bool CanValidateToken => true;
        
        public int MaximumTokenSizeInBytes { get; set; } = 8192;
        
        public bool CanReadToken(string securityToken) => true;
        
        public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            try
            {
                // Use the JWT library to decode and verify the token
                var decodedToken = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA512Algorithm())
                    .WithSecret(_secret)
                    .WithUrlEncoder(new JwtBase64UrlEncoder())
                    .WithJsonSerializer(new JsonNetSerializer())
                    .MustVerifySignature()
                    .Decode<Dictionary<string, string>>(token);
                
                // Convert the decoded token claims to ClaimsPrincipal
                var claims = new List<Claim>();
                foreach (var kvp in decodedToken)
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value));
                }
                
                // Add role claim for authorization to work correctly
                if (decodedToken.TryGetValue("Role", out var role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                
                // Check token expiration
                if (decodedToken.TryGetValue("Exp", out var expString) && 
                    long.TryParse(expString, out var exp))
                {
                    var expiration = DateTimeOffset.FromUnixTimeSeconds(exp);
                    if (expiration < DateTimeOffset.UtcNow)
                    {
                        throw new SecurityTokenExpiredException("Token has expired");
                    }
                }
                
                var identity = new ClaimsIdentity(claims, "Bearer");
                validatedToken = new JwtSecurityToken(token);
                
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenValidationException($"Token validation failed: {ex.Message}", ex);
            }
        }
    }

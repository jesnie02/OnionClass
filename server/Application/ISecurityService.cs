using Application.Models;
using Application.Models.Dto.Request;
using Application.Models.Dto.Responses;

namespace Application;

public interface ISecurityService
{
    public string HashPassword(string password);
    public void VerifyPasswordOrThrow(string password, string hashedPassword);
    public string GenerateSalt();
    public string GenerateJwt(JwtClaims claims);
    public AuthResponseDto Login(AuthRequestDto dto);
    public AuthResponseDto Register(AuthRequestDto dto);
    public JwtClaims VerifyJwtOrThrow(string jwt);
}
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dto.Responses;

public class AuthResponseDto
{
    [Required] public string Jwt { get; set; } = null!;
}
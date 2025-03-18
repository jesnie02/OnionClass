using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dto;

public abstract class ApplicationBaseDto
{
    [Required] public string EventType { get; set; } = null!;
}
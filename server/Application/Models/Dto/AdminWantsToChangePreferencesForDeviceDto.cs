using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dto;

public class AdminWantsToChangePreferencesForDeviceDto
{
    [Required] public string DeviceId { get; set; } = null!;

    [Required] public int IntervalMilliseconds { get; set; }

    [Required] public string Unit { get; set; } = null!;
}
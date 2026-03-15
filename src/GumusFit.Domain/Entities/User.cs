using GumusFit.Domain.Enums;

namespace GumusFit.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public ICollection<CalorieEntry> CalorieEntries { get; set; } = new List<CalorieEntry>();
}

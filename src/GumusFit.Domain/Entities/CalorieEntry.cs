namespace GumusFit.Domain.Entities;

public class CalorieEntry
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int Calories { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

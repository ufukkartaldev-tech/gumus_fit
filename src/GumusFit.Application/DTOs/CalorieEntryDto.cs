namespace GumusFit.Application.DTOs;

public class CalorieEntryDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int Calories { get; set; }
    public string? Description { get; set; }
}

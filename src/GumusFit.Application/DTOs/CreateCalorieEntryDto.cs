namespace GumusFit.Application.DTOs;

public class CreateCalorieEntryDto
{
    public DateTime Date { get; set; }
    public int Calories { get; set; }
    public string? Description { get; set; }
}

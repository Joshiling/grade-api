using System.ComponentModel.DataAnnotations;

public class StudentDTO
{
    [Required]
    public string Name {get; set;} = "";

    [Range(0,100)]
    public int Score {get; set;}
}
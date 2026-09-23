using System.Dynamic;
using System.Runtime.InteropServices;

public class Student : IGradeable
{
    public int Id {get; set;}
    public string Name {get; set;}
    public int Score {get; set;}
    public bool Favourite {get; set;}

    public Student(string name, int score)
    {
        Name = name;
        Score = score;
        Favourite=false;
    }

    public string GetGrade()
    {
        if (Score >= 70) return "A";
        if (Score >= 50) return "B";
        return "C";
    }
}
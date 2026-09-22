using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class StudentController : ControllerBase
{
    private static List<Student> students = new List<Student>
    {
    new Student("Laurence", 50,1),
     new Student("Lewis", 10,2),
     new Student("Melih", 70,3) 
    };

    private readonly ILogger<StudentController> _logger;

    public StudentController(ILogger<StudentController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<List<Student>> GetAll()
    {
        return Ok(students);
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetByID(int id)
    {
        Student? student = students.Find(student => student.Id == id);
        if (student != null)
        {
            return Ok(student);
        }
        return NotFound();
    }

    [HttpPost]
    public ActionResult<Student> Create(Student newStudent)
    {
        students.Add(newStudent);
        return CreatedAtAction(nameof(GetByID), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, Student newStudent)
    {
        int studentIndex = students.FindIndex(student => student.Id == id);
        if (studentIndex == -1)
        {
            return NotFound();
        }
        students[studentIndex] = newStudent;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        int studentIndex = students.FindIndex(student => student.Id == id);
        if (studentIndex == -1)
        {
            return NotFound();
        }
        students.RemoveAt(studentIndex);
        return NoContent();
    }

}

  
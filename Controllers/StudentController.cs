using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
    public ActionResult<List<Student>> GetAll(int? largerThan, int? smallerThan,string? sortBy)
    {
        if (largerThan == null)
        {
            largerThan = students.Min(s=>s.Score);
        }
        if (smallerThan == null)
        {
            smallerThan = students.Max(s=>s.Score);
        }
        
        IEnumerable<Student> filteredStudents = students;
        
        if (sortBy != null)
        {
            switch (sortBy.ToLower())
            {
                case "asc":
                    filteredStudents = filteredStudents.OrderBy(s=>s.Score);
                    break;
                case "desc":
                    filteredStudents = filteredStudents.OrderByDescending(s=>s.Score);
                    break;
                default:
                    return BadRequest("sortBy must be asc or desc");
            }        
        }
        
        return Ok(filteredStudents.Where(s=>s.Score>=largerThan).Where(s=>s.Score<=smallerThan).ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetByID(int id)
    {
        Student? student = students.Find(student => student.Id == id);
        if (student != null)
        {
            return Ok(student);
        }
        return NotFound($"No student with ID {id}");
    }

    [HttpPost]
    public ActionResult<Student> Create(StudentDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newStudent = new Student(dto.Name, dto.Score, students.Count > 0 ? students.Max(s => s.Id) + 1 : 1);
        students.Add(newStudent);
        return CreatedAtAction(nameof(GetByID), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, StudentDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int studentIndex = students.FindIndex(student => student.Id == id);
        if (studentIndex == -1)
        {
            return NotFound($"No student with ID {id}");
        }

        var newStudent = new Student(dto.Name, dto.Score, id);

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

  
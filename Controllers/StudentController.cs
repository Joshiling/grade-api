using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

[ApiController]
[Route("[controller]")]

public class StudentController : ControllerBase
{
    // private static List<Student> students = new List<Student>
    // {
    // new Student("Laurence", 50,1),
    //  new Student("Lewis", 10,2),
    //  new Student("Melih", 70,3) 
    // };

    private readonly ILogger<StudentController> _logger;
    private readonly AppDbContext _context;

    public StudentController(ILogger<StudentController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Student>>> GetAll(int? largerThan, int? smallerThan,string? sortBy)
    {
        var db_students = await _context.Students.ToListAsync();
        if (db_students.Count() == 0)
        {
            return Ok(db_students);
        }
        if (largerThan == null)
        {
            largerThan = db_students.Min(s=>s.Score);
        }
        if (smallerThan == null)
        {
            smallerThan = db_students.Max(s=>s.Score);
        }
        
        IEnumerable<Student> filteredStudents = db_students;
        
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
        
        return Ok(filteredStudents.Where(s=>s.Score>=largerThan && s.Score<=smallerThan).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetByID(int id)
    {
        Student? student = await _context.Students.FindAsync(id);//students.Find(student => student.Id == id);
        if (student != null)
        {
            return Ok(student);
        }
        return NotFound($"No student with ID {id}");
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Create(StudentDTO dto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newStudent = new Student(dto.Name,dto.Score);//, students.Count > 0 ? students.Max(s => s.Id) + 1 : 1};

        _context.Students.Add(newStudent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByID), new { id = newStudent.Id }, newStudent);
    }

    [HttpPut("{id}/favourite")]
    public async Task<ActionResult> Favourite(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound($"No student with ID {id}");
        }
        student.Favourite = !student.Favourite;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, StudentDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound($"No student with ID {id}");
        }

        student.Name = dto.Name;
        student.Score = dto.Score;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}

  
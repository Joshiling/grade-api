using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    options =>
    options.AddPolicy("AngularClient",policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    })
);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=students.db"));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AngularClient");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Students.Any())
    {
        context.Students.AddRange(
            new Student("Ava Thompson", 88),
            new Student("Liam Chen", 74),
            new Student("Sofia Martinez", 91),
            new Student("Noah Patel", 65 ),
            new Student("Isla Robertson", 79)
        );

        context.SaveChanges();
    }
}

app.Run();

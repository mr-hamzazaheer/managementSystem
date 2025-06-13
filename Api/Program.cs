using Api.Middleware;
using Api.ServiceExtensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration); 
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); // Corrected from UseSwaggerGen to UseSwagger
app.UseSwaggerUI(); // Corrected from UseSwaggerUi to UseSwaggerUI 
// Configure 
app.UseHttpsRedirection();
app.UseMiddleware<Middleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

using Api.ServiceExtensions; 

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); // Corrected from UseSwaggerGen to UseSwagger
app.UseSwaggerUI(); // Corrected from UseSwaggerUi to UseSwaggerUI 
// Configure 
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

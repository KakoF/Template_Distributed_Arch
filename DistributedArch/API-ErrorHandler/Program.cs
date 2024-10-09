using API_ErrorHandler.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.AddServices();
builder.AddHealthChecks();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddCustomMiddlewares();
app.UsePrometheus();

app.UseAuthorization();

app.MapControllers();

app.Run();

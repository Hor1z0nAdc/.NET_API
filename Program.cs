using FirstProject.Data;
using FirstProject.MappingProfiles;
using FirstProject.StorageRepositories;
using FirstProject.StorageRepositories.Implementation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProjectDbContext>();
builder.Services.AddScoped<IProjectTaskStorageRepository, SqlProjectTaskStorageRepository>();
builder.Services.AddScoped<IProjectStorageRepository, SqlProjectStorageRepository>();
builder.Services.AddAutoMapper(typeof(ProjectTaskProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

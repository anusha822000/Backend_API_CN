// using Microsoft.EntityFrameworkCore;
// using RoleManagementApi.Data;
// using System.Text.Json.Serialization;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         // Handle reference loops for EF Core navigation properties
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//     });

// // Configure Swagger
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // Configure CORS to allow Angular frontend
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAngular",
//         policy => policy
//             .AllowAnyOrigin()    // ⚠ In production, replace with Angular app URL
//             .AllowAnyMethod()
//             .AllowAnyHeader());
// });

// // Configure EF Core DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// var app = builder.Build();

// // Enable middleware
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(c =>
//     {
//         c.SwaggerEndpoint("/swagger/v1/swagger.json", "Role Management API V1");
//         c.RoutePrefix = string.Empty;
//     });
// }

// app.UseHttpsRedirection();

// // Use CORS
// app.UseCors("AllowAngular");

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

using Microsoft.EntityFrameworkCore;
using RoleManagementApi.Data;
using RoleManagementApi.Services;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add DbContext
// ----------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------
// Add HttpClient support (for future use in services)
// ----------------------
builder.Services.AddHttpClient();

// ----------------------
// Register application services
// ----------------------
builder.Services.AddScoped<ILinkCheckerService, LinkCheckerService>();
builder.Services.AddScoped<IChatbotService, ChatbotService>();

// ----------------------
// Add controllers with JSON options
// ----------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Handle reference loops for EF Core navigation properties
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// ----------------------
// Enable CORS
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// ----------------------
// Add Swagger / OpenAPI
// ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------
// Middleware pipeline
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RoleManagementApi v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Apply CORS
app.UseCors("AllowAngularApp");

// ----------------------
// Ensure Uploads folder exists
// ----------------------
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// Serve static files for uploaded documents
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
    ),
    RequestPath = "/Uploads"
});


app.UseAuthorization();

app.MapControllers();

app.Run();

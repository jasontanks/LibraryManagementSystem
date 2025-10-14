using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.Behaviours;
using LibraryManagement.Application.Commands;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.EF;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure EF Core with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LibraryDb"));

// Register Repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();

// Register MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AddBookCommand).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

// Register FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(AddBookCommand).Assembly);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom middleware for handling validation exceptions
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Ensure the database is created and seeded on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // For in-memory database, this ensures it's created and seeded.
    dbContext.Database.EnsureCreated();
}


app.Run();

public partial class Program { }

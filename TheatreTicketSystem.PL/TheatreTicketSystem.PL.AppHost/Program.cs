using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Repositories;
using TheatreTicketSystem.DAL.UoW;
using AutoMapper;
using TheatreTicketSystem.PL.Mapping;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Додавання сервісів до контейнера
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Конфігурація бази даних
builder.Services.AddDbContext<TheatreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Реєстрація репозиторіїв
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Реєстрація Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Реєстрація сервісів бізнес-логіки
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<HallService>();
builder.Services.AddScoped<PerformanceService>();
builder.Services.AddScoped<SeatService>();
builder.Services.AddScoped<TicketService>();

// Конфігурація AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Додавання CORS для тестування
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Конфігурація HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
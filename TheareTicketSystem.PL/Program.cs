using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.UoW;
using TheatreTicketSystem.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context - використовуємо SQLite для простоти
builder.Services.AddDbContext<TheatreDbContext>(options =>
    options.UseSqlite("Data Source=theatre.db"));

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<PerformanceService>();
builder.Services.AddScoped<HallService>();
builder.Services.AddScoped<SeatService>();
builder.Services.AddScoped<TicketService>();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TheatreDbContext>();
    context.Database.EnsureCreated();
}

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
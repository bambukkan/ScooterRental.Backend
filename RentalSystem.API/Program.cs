
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyLibrary.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
// Регистрируем в DI, чтобы ValidationFilter мог взять каждый валидатор с помощью GetService 
builder.Services.AddValidatorsFromAssemblyContaining<CreatingUserRequestValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserRequestValidator>();

// builder.Services.AddValidatorsFromAssemblyContaining<CreatingScooterRequestValidator>();
// builder.Services.AddValidatorsFromAssemblyContaining<UpdateScooterRequestValidator>();

// builder.Services.AddValidatorsFromAssemblyContaining<CreatingBookingRequestValidator>();

/*
Лайфхак на будущее: Метод AddValidatorsFromAssemblyContaining<T> сканирует всю сборку (проект), 
в которой находится указанный класс, и регистрирует вообще все валидаторы, которые там найдёт.
Тебе не нужно писать отдельную строчку для CreatingUserRequestValidator, 
отдельную для UpdateUserRequestValidator и так далее, если они лежат в одном проекте.
 Достаточно оставить один единственный вызов для любого из них, и FluentValidation подтянет всё остальное автоматически.
*/

builder.Services.AddControllers( options =>
    {
        options.Filters.Add<ValidationFilter>(); // теперь он будет перехватывать запросы!
    })
    .AddJsonOptions(options =>
    {
        // Эта магия останавливает бесконечную вложенность
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        
        // По желанию: сделает JSON более читаемым (с отступами)
        options.JsonSerializerOptions.WriteIndented = true;  
    });

builder.Services.AddHttpLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   

builder.Services.AddScoped<IBookingRepository,BookingRepository>();
builder.Services.AddScoped<IScooterRepository,ScooterRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddScoped<IScooterService,ScooterService>();
builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<IJwtProvider,JwtProvider>();
builder.Services.AddScoped<IPasswordHasher,PasswordHasher>();

builder.Services.AddDbContext<RentalSystemDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString("RentalSystemDbContext"));
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpLogging();

app.MapControllers();

app.Run();

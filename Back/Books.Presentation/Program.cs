using Books.Application.Services;
using Books.Core.Abstractions.Services;
using Books.Infrastructure.Context;
using Books.Presentation.Middlewares;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
//builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateRoleDtoValidator>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization()
    .AddFluentValidation();

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

builder.Services.AddDbContext<BooksContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Books")));

var app = builder.Build();

var supportedCultures = new[] { "en-US", "ru-RU", "az-AZ" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<LocalizationMiddleware>();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
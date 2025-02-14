using System.Globalization;
using System.Text;
using Books.Application.Mappings;
using Books.Application.Services.Auth;
using Books.Application.Services.Main;
using Books.Application.Validators.Create;
using Books.Application.Validators.Update;
using Books.Core.Abstractions.Repositories.Auth;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Abstractions.UOW;
using Books.Infrastructure.Context;
using Books.Infrastructure.Repositories.Auth;
using Books.Infrastructure.Repositories.Main;
using Books.Infrastructure.UOW;
using Books.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("accessToken"))
                context.Token = context.Request.Cookies["accessToken"];
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value))
    };
});


builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ru"),
        new CultureInfo("az")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlackListedRepository, BlackListedRepository>();
builder.Services.AddScoped<IUserActiveSessionsRepository, UserActiveSessionsRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserActiveSessionsService, UserActiveSessionsService>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlackListedService, BlackListedService>();

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<CreateBookValidator>();
builder.Services.AddScoped<CreateOrderItemValidator>();
builder.Services.AddScoped<CreateOrderValidator>();
builder.Services.AddScoped<CreateReviewValidator>();
builder.Services.AddScoped<CreateRoleValidator>();
builder.Services.AddScoped<CreateUserValidator>();

builder.Services.AddScoped<UpdateBookValidator>();
builder.Services.AddScoped<UpdateOrderItemValidator>();
builder.Services.AddScoped<UpdateOrderValidator>();
builder.Services.AddScoped<UpdateReviewValidator>();
builder.Services.AddScoped<UpdateRoleValidator>();
builder.Services.AddScoped<UpdateUserValidator>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<BooksContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Books")));

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;


app.UseMiddleware<CustomExceptionMiddleware>();
app.UseMiddleware<CustomSuccessResponseMiddleware>();
app.UseMiddleware<BlackListMiddleware>();
//app.UseMiddleware<ActiveSessionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<LocalizationMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRequestLocalization(localizationOptions);
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.UseCors();


app.Run();
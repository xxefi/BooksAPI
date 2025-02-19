using Auth.Application.Mappings;
using Auth.Application.Services;
using Auth.Application.Validators.Create;
using Auth.Application.Validators.Update;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Abstractions.Services;
using Auth.Core.Abstractions.UOW;
using Auth.Infrastructure.Repositories;
using Auth.Infrastructure.UOW;
using Auth.Presentation.Middlewares;
using Auth.Presentation.Services;
using Microsoft.EntityFrameworkCore;
using AuthContext = Auth.Infrastructure.Context.AuthContext;
using AuthService = Auth.Application.Services.AuthService;
using RoleService = Auth.Application.Services.RoleService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GrpcExceptionInterceptor>();
    options.MaxReceiveMessageSize = 50 * 1024 * 1024;
    options.MaxSendMessageSize = 50 * 1024 * 1024;
});

builder.Services.AddDbContext<AuthContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Auth")));


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddScoped<IBlackListedRepository, BlackListedRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserActiveSessionsRepository, UserActiveSessionsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlackListedService, BlackListedService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserActiveSessionsService, UserActiveSessionsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddSingleton<ILocalizationService, LocalizationService>();


builder.Services.AddScoped<CreateRoleValidator>();
builder.Services.AddScoped<CreateUserValidator>();

builder.Services.AddScoped<UpdateRoleValidator>();
builder.Services.AddScoped<UpdateUserValidator>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();


app.MapGrpcService<AccountServiceImpl>();
app.MapGrpcService<AuthServiceImpl>();
app.MapGrpcService<RoleServiceImpl>();
app.MapGet("/", () => "grpc test");

app.Run();

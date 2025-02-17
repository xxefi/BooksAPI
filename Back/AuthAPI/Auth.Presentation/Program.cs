using Auth.Application.Services;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Abstractions.Services;
using Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AuthContext = Auth.Infrastructure.Context.AuthContext;
using AuthService = Auth.Application.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<AuthContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Auth")));

var app = builder.Build();


builder.Services.AddScoped<IBlackListedRepository, BlackListedRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserActiveSessionsRepository, UserActiveSessionsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlackListedService, BlackListedService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserActiveSessionsService, UserActiveSessionsService>();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

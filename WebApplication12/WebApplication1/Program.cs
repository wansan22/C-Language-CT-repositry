
using Application.Behaviour.Course;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Persistence;
using System.Text;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateCourseCommand).Assembly));
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddServices(builder.Configuration);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                                    IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
                                };

                                options.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        string? accessToken = context.Request.Cookies["accessToken"];

                                        if (!string.IsNullOrEmpty(accessToken))
                                        {
                                            context.Token = accessToken;
                                        }

                                        return Task.CompletedTask;
                                    }
                                };
                            });

            builder.Services.AddAuthorization(); // Добавляем сервисы авторизации

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });

            }
            app.UseCors(config => {
                config.AllowAnyHeader();
                config.AllowAnyMethod();
                config.AllowCredentials();
                config.WithOrigins("");
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            

            await app.Services.RunMigration();

            app.Run();
        }
    }
}


public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Введите JWT Token (без префикса Bearer)"
        };

        // 2. Применение глобального требования безопасности (чтобы появилась кнопка Authorize)
        document.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                // Ключевой момент: используем OpenApiSecuritySchemeReference и передаём документ
                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
            }
        };

        return Task.CompletedTask;
    }
}

// Регистрация в builder

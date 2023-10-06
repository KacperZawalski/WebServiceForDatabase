using Microsoft.EntityFrameworkCore;
using WebServiceForDatabase.Models;
using WebServiceForDatabase.DatabaseContext;
using FluentValidation;
using WebServiceForDatabase.Validators;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebServiceForDatabase
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<UserDatabaseContext>(opt => opt.UseInMemoryDatabase("UserDatabase"));
            builder.Services.AddScoped<IValidator<UserModel>, UserValidator>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            var app = builder.Build();

            app.MapGet("/Home", async (UserDatabaseContext userDatabase) =>
                await userDatabase.users.ToListAsync());

            app.MapGet("/Home/{id}", async (int id, UserDatabaseContext userDatabase) =>
                await userDatabase.users.FindAsync(id)
                    is UserModel userModel
                        ? Results.Ok(userModel)
                        : Results.NotFound());
            
            app.MapPost("/Home/Add", async (IValidator<UserModel> validator, UserModel userModel, UserDatabaseContext userDatabaseContext) =>
            {
                var validationResult = await validator.ValidateAsync(userModel);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                userDatabaseContext.AddUserToDatabase(userModel);
                await userDatabaseContext.SaveChangesAsync();

                return Results.Created($"/Home/{userModel.iD}", userModel);
            });

            app.Run();
        }
    }
}
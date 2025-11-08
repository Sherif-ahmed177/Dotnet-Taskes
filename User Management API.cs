using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

public class Program
{
    private static List<User> users = new List<User>
    {
        new User { Id = 1, Name = "Sherif", Email = "sherif@example.com" }
    };

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.Use(async (context, next) =>
        {
            Console.WriteLine($"[{DateTime.Now}] Request {context.Request.Method} {context.Request.Path}");
            await next();
        });

        app.MapGet("/users", () =>
        {
            return Results.Ok(users);
        });

        app.MapPost("/users", (User newUser) =>
        {
            if (!ValidateUser(newUser, out var errors))
                return Results.BadRequest(errors);

            newUser.Id = users.Count + 1;
            users.Add(newUser);
            return Results.Created($"/users/{newUser.Id}", newUser);
        });

        app.MapPut("/users/{id}", (int id, User updatedUser) =>
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Results.NotFound(new { Message = "User not found" });

            if (!ValidateUser(updatedUser, out var errors))
                return Results.BadRequest(errors);

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return Results.Ok(user);
        });

        app.MapDelete("/users/{id}", (int id) =>
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Results.NotFound(new { Message = "User not found" });

            users.Remove(user);
            return Results.Ok(new { Message = "User deleted" });
        });

        app.Run();
    }

    private static bool ValidateUser(User user, out Dictionary<string, string[]> errors)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(user);
        bool isValid = Validator.TryValidateObject(user, context, validationResults, true);

        errors = validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "")
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage ?? "").ToArray()
            );

        return isValid;
    }
}

public class User
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}

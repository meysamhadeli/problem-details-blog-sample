using Microsoft.AspNetCore.Diagnostics;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var title = "Bad Input";
        var detail = "Invalid input";
        var type = "https://errors.example.com/badInput";

        if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
        {
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exceptionType = exceptionHandlerFeature?.Error;

            if (exceptionType is not null)
            {
                switch (exceptionType)
                {
                    case CustomException customException:
                        title = customException.GetType().Name;
                        detail = customException.Message;
                        type = customException.GetType().ToString();
                        context.Response.StatusCode = (int)customException.StatusCode;
                        break;
                }   
            }

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = title,
                    Detail = detail,
                    Type = type
                }
            });
        }
    });
});

app.MapControllers();

app.Run();
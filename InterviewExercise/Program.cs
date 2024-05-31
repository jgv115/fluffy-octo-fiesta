using InterviewExercise.Capabilities.InterviewCapability;
using InterviewExercise.Capabilities.InterviewCapability.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IInterviewCapabilityHandler, InterviewCapabilityHandler>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/interviewEndpoint", async (IInterviewCapabilityHandler interviewCapabilityHandler) =>
    {
        await interviewCapabilityHandler.HandleInterviewCapabilityCall(new InterviewRequest(Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid()));

        return Results.NoContent();
    })
    .WithName("PostInterviewEndpoint")
    .WithOpenApi();

app.Run();
using InterviewExercise.Capabilities.InterviewCapability.Dtos;

namespace InterviewExercise.Capabilities.InterviewCapability;

public interface IInterviewCapabilityHandler
{
    public Task HandleInterviewCapabilityCall(InterviewRequest request);
}
namespace InterviewExercise.Capabilities.InterviewCapability.Dtos;

public record InterviewRequest(Guid OrganisationId, Guid PaymentAccountId, Guid PaymentGatewayId);
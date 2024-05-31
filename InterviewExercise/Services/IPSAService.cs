using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterviewExercise.Services;

public interface IPaymentServiceAdministrator
{
    Task<PaymentGateway?> GetPaymentGateway(Guid paymentGatewayId, Guid organisationId);
    Task<PaymentGateway> SavePaymentGateway(PaymentGateway paymentGateway);
}

public readonly record struct PaymentGateway(
    [property: JsonPropertyName("sequence")]
    int Sequence,
    [property: JsonPropertyName("organisationID")]
    Guid OrganisationId,
    [property: JsonPropertyName("paymentGatewayID")]
    Guid PaymentGatewayId
);

public record PaymentGatewayPaymentMethodDto
{
    public PaymentGateway PaymentGateway { get; set; } = default!;
    public List<PaymentMethod> PaymentMethods { get; set; } = default!;
}

public record struct PaymentMethod(
    Guid OrganisationId,
    Guid PaymentGatewayId,
    string PaymentMethodName,
    string PaymentMethodStatus,
    string PaymentMethodCategory
);
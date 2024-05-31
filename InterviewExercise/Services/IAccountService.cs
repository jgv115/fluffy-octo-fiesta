using System.Text.Json.Serialization;

namespace InterviewExercise.Services;

public interface IAccountsService
{
    Task<Account?> GetAccount(Guid organisationId, Guid accountId);
    Task<List<Account>> GetAccounts(Guid organisationId);
    Task<Account> CreateAccount(Guid organisationId, Account account);
}


public readonly record struct Account(
    [property: JsonPropertyName("AccountID")]
    Guid? AccountId,
    [property: JsonPropertyName("Name")] string? Name,
    [property: JsonPropertyName("Code")] string? Code,
    [property: JsonPropertyName("Type")] string? Type,
    [property: JsonPropertyName("TaxType")]
    string? TaxType
);

public readonly record struct Organisation(
    [property: JsonPropertyName("OrganisationID")]
    Guid? OrganisationId,
    [property: JsonPropertyName("CountryCode")]
    string? CountryCode
);

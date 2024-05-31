using System.Text.Json;
using InterviewExercise.Capabilities.InterviewCapability.Dtos;
using InterviewExercise.Services;

namespace InterviewExercise.Capabilities.InterviewCapability;

public class InterviewCapabilityHandler(
    ILogger<InterviewCapabilityHandler> logger,
    IAccountsService accountsService,
    IPaymentServiceAdministrator psa) : IInterviewCapabilityHandler
{
    public async Task HandleInterviewCapabilityCall(InterviewRequest request)
    {
        var paymentAccount = await GetPaymentAccount(request.OrganisationId, request.PaymentAccountId);
        var paymentGateway = await GetPaymentGateway(request.OrganisationId, request.PaymentGatewayId);
        var feeAccount = await FindOrCreateFeeAccount(request);
        var updatedPaymentGateway =
            await UpdatePaymentGateway(paymentGateway, paymentAccount.AccountId, feeAccount.AccountId);
        logger.LogInformation("Updated Gateway is: {Gateway}", JsonSerializer.Serialize(updatedPaymentGateway));
        SaveInvoiceThemes(request.OrganisationId);
        await CompletePromotionGracefully(request.OrganisationId);
    }


    private void SaveInvoiceThemes(Guid organisationId)
    {
        Console.WriteLine($"saving invoice theme for orgId {organisationId}");
    }


    private async Task CompletePromotionGracefully(Guid organisationId)
    {
        try
        {
            Console.WriteLine("completing promos..");

            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            logger.LogError(e, "CompletePromotion failed for {OrganisationId}", organisationId);
        }
    }

    private async Task<PaymentGateway> UpdatePaymentGateway(PaymentGateway paymentGateway,
        Guid? paymentAccountId, Guid? feeAccountId)
    {
        var updatedPaymentGateway = await psa.SavePaymentGateway(paymentGateway);
        return updatedPaymentGateway;
    }

    private async Task<PaymentGateway> GetPaymentGateway(Guid organisationId,
        Guid paymentGatewayId)
    {
        var paymentGateway =
            await psa.GetPaymentGateway(paymentGatewayId, organisationId);
        return paymentGateway ?? throw new Exception("hello");
    }

    private async Task<Account> FindOrCreateFeeAccount(InterviewRequest request)
    {
        if (request.OrganisationId.Equals(Guid.Empty))
        {
            return await GetAccount(request.OrganisationId, Guid.NewGuid(), "");
        }

        var accounts = await accountsService.GetAccounts(request.OrganisationId);

        var (newAccountCode, newAccountName) = GetUniqueAccountNameAndCode(accounts);

        return await accountsService.CreateAccount(request.OrganisationId,
            new Account(
                null,
                newAccountName,
                newAccountCode,
                "type",
                "type"
            ));
    }

    private static (string Code, string Name) GetUniqueAccountNameAndCode(List<Account> accounts)
    {
        var maxExpenseAccountCode = accounts?.Where(x => x.Type == "type").Select(x => x.Code).OrderBy(x => x).Max();
        var newAccountCode = int.Parse(maxExpenseAccountCode!) + 1;

        var newAccountName = "test";
        var accountNameIncrement = 1;

        var retryCount = 0;

        while (retryCount <= 100)
        {
            var accountCodeExists = accounts!.Any(x => x.Code == newAccountCode.ToString());
            var accountNameExists = accounts!.Any(x => x.Name!.Equals(newAccountName));

            if (!accountCodeExists && !accountNameExists)
            {
                break;
            }

            if (accountCodeExists)
            {
                newAccountCode++;
            }

            if (accountNameExists)
            {
                newAccountName = $"{newAccountName} {accountNameIncrement}";
                accountNameIncrement++;
            }

            retryCount++;
        }

        return (newAccountCode.ToString(), newAccountName);
    }

    private async Task<Account> GetPaymentAccount(Guid organisationId, Guid paymentAccountId)
    {
        return await GetAccount(organisationId, paymentAccountId, "error");
    }

    private async Task<Account> GetAccount(Guid organisationId, Guid paymentAccountId, string errorCode)
    {
        var paymentAccount = await accountsService.GetAccount(organisationId, paymentAccountId);
        return paymentAccount ?? throw new Exception(errorCode);
    }
}
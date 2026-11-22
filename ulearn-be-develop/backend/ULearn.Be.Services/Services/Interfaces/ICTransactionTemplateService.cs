using Microsoft.AspNetCore.Http;

namespace ULearn.Be.Services.Services.Interfaces;

public interface ICTransactionTemplateService
{
    Task Upload(IFormFileCollection formFiles, string transactionTypeKey);
}
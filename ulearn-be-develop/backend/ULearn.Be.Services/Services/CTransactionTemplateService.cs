using Microsoft.AspNetCore.Http;
using ULearn.Be.Common.Enums;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class CTransactionTemplateService : ICTransactionTemplateService
{
    private readonly ICTransactionTemplateRepository _cTransactionTemplateRepository;
    private readonly ICTransactionTemplateTypeRepository _cTransactionTypeRepository;

    public CTransactionTemplateService(
        ICTransactionTemplateRepository cTransactionTemplateRepository,
        ICTransactionTemplateTypeRepository cTransactionTypeRepository
    )
    {
        _cTransactionTemplateRepository = cTransactionTemplateRepository;
        _cTransactionTypeRepository = cTransactionTypeRepository;
    }

    public async Task Upload(
        IFormFileCollection formFiles,
        string transactionTypeKey
    )
    {
        var transactionTypeEnum = await _cTransactionTypeRepository.GetTransactionTemplateTypeEnum(transactionTypeKey);
        var file = formFiles.FirstOrDefault();

        if (file == null)
        {
            throw new ConflictException(ErrorCodesEnum.MissingFile);
        }

        await _cTransactionTemplateRepository.Upsert(file.OpenReadStream(), file.ContentType, transactionTypeEnum);
    }
}
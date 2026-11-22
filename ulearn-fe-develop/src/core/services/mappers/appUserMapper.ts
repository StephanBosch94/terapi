import { paths } from "apiTypes";


export const toAppUserIm = (userVm: paths['/api/user']['get']['responses']['200']['content']['application/json'])
    : paths['/api/user']['put']['requestBody']['content']['application/json'] => {
    return {
        login: userVm.login,
        email: userVm.email,
        userRoleId: userVm.userRoleId,
        userStateId: userVm.stateId,
        name: userVm.name,
        firstName: userVm.firstName,
        lastName: userVm.lastName,
        phone: userVm.phone,
        countryId: userVm.country?.countryId,
        placeOfBusiness: userVm.placeOfBusiness,
        ico: userVm.ico,
        billingEmail: userVm.billingEmail,
        companyName: userVm.companyName,
        dic: userVm.dic,
        bankAccountNumber: userVm.billingInfo?.accountNumber,
        bankCode: userVm.billingInfo?.bankCode,
        attributes: userVm.attributes?.map(x => ({
            attributeTypeId: x.attributeType?.attributeTypeId,
            value: x.value
        })),
        languages: userVm.languages?.map(x => ({
            languageId: x.language?.languageId,
            languageLevelId: x.level?.languageLevelId
        })),
        specializationIds: userVm.specializations?.map(x => x.specializationId!),
        subjectId: userVm.subject?.subjectId,
        currencyId: userVm.currency?.currencyId,
        hourRate: userVm.hourRate,
        teachingExperienceId: userVm.teachingExperience?.teachingExperienceId,
        timezone: userVm.timezone,
        subjectLevelIds: userVm.subjectLevels?.map(x => x.subjectLevelId!),
        interestIds: userVm.interests?.map(x => x.interestId!),
        weeklyGoalId: userVm.weeklyGoal?.weeklyGoalId,
        isVatPayer: userVm.isVatPayer,
        translationLanguageId: userVm.translationLanguageId
    }
} 
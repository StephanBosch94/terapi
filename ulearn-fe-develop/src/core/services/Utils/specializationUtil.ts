import { SpecializationType } from "../Types/AppTypes";

export const filterAndGroupSpecializations = ({ specializations, subjectId }: { specializations: SpecializationType[], subjectId: number }) => {
    return specializations
        .filter(x => x.subjectId === subjectId)
        .reduce((acc, curr) =>
            acc.length && acc[acc.length - 1].specializationCategoryId === curr.specializationCategoryId
                ? [...acc, { ...curr, includeCategoryTitle: false }]
                : [...acc, { ...curr, includeCategoryTitle: true }]
            , [] as (SpecializationType & { includeCategoryTitle: boolean })[]);
}
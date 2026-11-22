import { UserSearchQueryParsed, UserSearchQueryType } from "core/services/Types/AppTypes";
import { ParsedUrlQuery } from "querystring";

export const parseSearchQuery = (inputQuery: ParsedUrlQuery): UserSearchQueryParsed => {
    const query = inputQuery as UserSearchQueryType | undefined;
    const subjectId = query?.subjectId ? parseInt(query.subjectId) : undefined;
    const filtersApplied = query?.filtersApplied === "true" ? true : undefined;
    return {
        subjectId: subjectId !== undefined && subjectId > 0 ? subjectId : undefined,
        toTrialPrice: query?.toTrialPrice ? parseInt(query?.toTrialPrice) : undefined,
        specializationIds: query?.specializationIds?.length ? query?.specializationIds?.split(",")?.filter(x => x)?.map((x, idx) => parseInt(x)) : undefined,
        filtersApplied: filtersApplied
    }
}


import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { PagedVM } from "../Types/AppTypes"

const getAppendedData = <T>(lhsData: T[], rhsData: T[], appendType: AppendTypeEnum) => {
    switch (appendType) {
        case AppendTypeEnum.Start:
            return [...rhsData, ...lhsData]
        case AppendTypeEnum.End:
            return [...lhsData, ...rhsData]
        case AppendTypeEnum.None:
            return rhsData;
        default:
            throw new Error("Invalid appendType: " + appendType);
    }
}

export const getJoinedPagedVM = <T>(previousData: PagedVM<T> | undefined, newData: PagedVM<T>, appendType: AppendTypeEnum): PagedVM<T> => {
    if (previousData === undefined) return newData;
    return {
        size: newData.size,
        status: newData.status,
        data: getAppendedData(previousData.data, newData.data, appendType)
    }
}
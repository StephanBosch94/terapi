import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import jwtDecode from "jwt-decode";

type ClaimsType = {
    USER_ID: string,
    USER_ROLE_ID: string,
    USER_STATE: string
} | undefined;


export type ParsedClaimsType = {
    userId: number,
    userRoleId: UserRoleEnum,
    userStateId: UserStateEnum
} | undefined;

export const getClaims = (token: string | undefined): ParsedClaimsType => {
    if (!token) return undefined;
    const decoded = jwtDecode(token) as ClaimsType;
    if (!decoded) return undefined;

    return {
        userId: parseInt(decoded.USER_ID),
        userRoleId: parseInt(decoded.USER_ROLE_ID) as UserRoleEnum,
        userStateId: parseInt(decoded.USER_STATE) as UserStateEnum
    }
}
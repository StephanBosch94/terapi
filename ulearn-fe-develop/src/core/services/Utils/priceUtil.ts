import { COMMON_CONSTANTS } from "core/constants/commonConstants"

export const getWithdrawTotalPrice = (amount: number) => parseFloat(((1 - COMMON_CONSTANTS.WITHDRAW_COMMON_RATE) * amount).toFixed(2));
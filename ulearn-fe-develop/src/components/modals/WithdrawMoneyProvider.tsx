import Modal from "@mui/material/Modal";
import { useTranslation } from "components/core/TranslationProvider";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectWithdrawStatus, setUserWalletStatistics, widthdrawMoney } from "core/redux/userWalletSlice";
import { createContext, useEffect, useMemo, useRef, useState } from "react";
import ModalsHeader from "./common/ModalsHeader";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import Box from "@mui/system/Box";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { styled } from "@mui/material/styles";
import H5Styled from "components/styles/typography/H5Styled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Tooltip from "components/styles/Tooltip";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import { selectUserRoleId } from "core/redux/authSlice";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { getWithdrawTotalPrice } from "core/services/Utils/priceUtil";
import InputCurrncyAdornment from "./common/InputCurrencyAdornment";
import { usePriceLocalization } from "core/hooks/priceHook";

const PrimaryButtonNonActiveStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.BLACK_10,
    color: COLORS.WHITE,
    ":hover": {
        backgroundColor: COLORS.BLACK_10,
        color: COLORS.WHITE,
        cursor: "initial"
    }
}) as typeof PrimaryButtonRectangle;

const DescriptionTypographyStyled = styled(Body1Styled)({
    color: COLORS.BLACK,
    "b": {
        fontWeight: "600"
    }
}) as typeof Body1Styled;

type StateType = "account_number" | "amount";
type ModalStateType = {
    type: StateType,
    titleKey: string,
    descriptionKey: string,
    buttonKey: string,
    tooltipKey: string
}
const modalStates: ModalStateType[] = [
    {
        type: "account_number",
        titleKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.TITLE,
        descriptionKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.ENTER_ACCOUNT_INFO_DESCRIPTION,
        buttonKey: TRANSLATION_KEYS.COMMON.NEXT_BUTTON,
        tooltipKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.ENTER_ACCOUNT_INFO_TOOLTIP
    },
    {
        type: "amount",
        titleKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.TITLE,
        descriptionKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.ENTER_AMOUNT_DESCRIPTION,
        buttonKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.WITHDRAW_BUTTON,
        tooltipKey: TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.ENTER_AMOUNT_GREATER_THAN_ZERO_TOOLTIP
    }
];

type WithdrawMoneyModalContextType = {
    setOpen: () => void
} | undefined;

type WithdrawMoneyInternalContextType = {
    accountNumber: string,
    bankCode: string,
    amount: number | undefined,
    state: ModalStateType,
    setAccountNumber: (newNumber: string) => void,
    setBankCode: (newBankCode: string) => void,
    setAmount: (newAmount: number | undefined) => void,
    isButtonActive: boolean,
    handleClick: () => void
} | undefined;

const WithdrawMoneyModalContext = createContext<WithdrawMoneyModalContextType>(undefined);
const WithdrawMoneyInternalContext = createContext<WithdrawMoneyInternalContextType>(undefined);
const useInternalContext = contextFactoryCreate('InternalContext', WithdrawMoneyInternalContext);

const BankAccountDetailsFields = () => {
    const { t } = useTranslation();
    const {
        accountNumber,
        setAccountNumber,
        bankCode,
        setBankCode
    } = useInternalContext();

    const handleChangeAccountNumber = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setAccountNumber(value as string);
    }

    const handleChangeBankCode = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setBankCode(value as string);
    }

    return (
        <Box sx={{ display: "flex", flexFlow: "row noWrap", alignItems: "center", gap: "8px" }}>
            <TextFieldStyled
                type="number"
                value={accountNumber}
                placeholder={t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.ACCOUNT_NUMBER_PLACEHOLDER)}
                onChange={handleChangeAccountNumber}
                sx={{ flex: "1 1 70%" }}
            />
            <H5Styled>/</H5Styled>
            <TextFieldStyled
                type="number"
                value={bankCode}
                placeholder={t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.BANK_CODE_PLACEHOLDER)}
                onChange={handleChangeBankCode}
                sx={{ flex: "1 1 30%" }}
            />
        </Box>
    )
}

const AmountFieild = ({ balance }: { balance: number | undefined }) => {
    const { t } = useTranslation();
    const { amount, setAmount } = useInternalContext();
    const { localize } = usePriceLocalization();
    const userRoleId = useAppSelector(selectUserRoleId);

    const handleChangeAmount = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value as string;
        const parsed = parseInt(value);
        const newAmount = !isNaN(parsed) ? parsed : undefined;
        setAmount(newAmount);
    }

    return (
        <>
            <TextFieldStyled
                type="number"
                value={amount}
                placeholder={t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.AVAILABLE_BALANCE_PLACEHOLDER, { amount: localize({ price: balance }) })}
                onChange={handleChangeAmount}
                sx={{ width: "100%" }}
                InputProps={{ endAdornment: <InputCurrncyAdornment /> }}
            />
            {amount && amount > 0 && userRoleId === UserRoleEnum.Student ? (
                <DescriptionTypographyStyled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.FINAL_AMOUNT_TEXT, { amount: localize({ price: getWithdrawTotalPrice(amount) }) }) }} />
            ) : <></>}
        </>
    )
}

const WithdrawMoneyModalProvider = ({
    children,
    bankCode: prevBankCode,
    bankAccountNumber: prevAccountNumber,
    balance
}: {
    children: any,
    bankCode?: string | null | undefined,
    bankAccountNumber?: string | null | undefined,
    balance?: number | undefined,
}) => {
    const { t } = useTranslation();
    const { convertToInternalCurrency } = usePriceLocalization();
    const [open, setOpen] = useState(false);
    const [accountNumber, setAccountNumber] = useState(prevAccountNumber ?? "");
    const [bankCode, setBankCode] = useState(prevBankCode ?? "");
    const [amount, setAmount] = useState<number | undefined>();
    const [state, setState] = useState<ModalStateType>(modalStates[0]);
    const dispatch = useAppDispatch();
    const withdrawStatus = useAppSelector(selectWithdrawStatus);
    const { enqueueSnackbar } = useSnackbar();
    const snackbarPending = useRef(false);
    const convertedAmount = convertToInternalCurrency({ price: amount });

    const isButtonActive = useMemo(() => {
        if (state.type === "account_number") {
            return accountNumber.length > 0 && bankCode.length > 0;
        }
        return amount !== undefined && amount > 0;
    }, [state, accountNumber, bankCode, amount]);

    useEffect(() => {
        setBankCode(prevBankCode ?? "");
    }, [prevBankCode]);

    useEffect(() => {
        setAccountNumber(prevAccountNumber ?? "");
    }, [prevAccountNumber]);

    useEffect(() => {
        if (!snackbarPending.current) return;
        if (withdrawStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.SUCCESSFULLY_CREATED_WITHDRAW_REQUEST), { variant: "success" });
            snackbarPending.current = false;
        }
        else if (withdrawStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.FAILED_TO_CREATE_WITHDRAW_REQUEST), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            snackbarPending.current = false;
        };
    }, [withdrawStatus]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setState(modalStates[0]);
        setOpen(true);
    }

    const handleClick = () => {
        if (!isButtonActive) return;
        if (state.type === "account_number") {
            setState(modalStates[1]);
            return;
        }

        if (!balance || !convertedAmount || Math.round(convertedAmount) > Math.round(balance)) {
            enqueueSnackbar(t(TRANSLATION_KEYS.DASHBOARD.WITHDRAW_MODAL.INSUFFICIENT_FUNDS_AVAILABLE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        snackbarPending.current = true;
        dispatch(widthdrawMoney({
            amount: convertedAmount,
            bankCode,
            bankAccountNumber: accountNumber
        })).then(() => {
            dispatch(setUserWalletStatistics());
            setOpen(false);
        })
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            handleClick();
        }
    }

    return (
        <WithdrawMoneyModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            <WithdrawMoneyInternalContext.Provider value={{
                accountNumber,
                bankCode,
                amount,
                state,
                isButtonActive,
                setAccountNumber: (newNumber: string) => setAccountNumber(newNumber),
                setBankCode: (newBankCode: string) => setBankCode(newBankCode),
                setAmount: (newAmount: number | undefined) => setAmount(newAmount),
                handleClick
            }}
            >
                {children}
                <Modal open={open} onClose={handleClose}>
                    <ModalBoxStyled>
                        <ModalsHeader handleClose={handleClose} text={t(state.titleKey)} />
                        <Body1Styled>{t(state.descriptionKey)}</Body1Styled>
                        {state.type === "account_number" ? <BankAccountDetailsFields /> : <></>}
                        {state.type === "amount" ? <AmountFieild balance={balance} /> : <></>}
                        {isButtonActive ? (
                            <PrimaryButtonRectangle onClick={handleClick}>
                                {withdrawStatus !== LoadingStatusEnum.Loading ? t(state.buttonKey) : <CircularProgressCustom color={COLORS.WHITE} />}
                            </PrimaryButtonRectangle>
                        ) : (
                            <Tooltip text={t(state.tooltipKey)}>
                                <PrimaryButtonNonActiveStyled>{t(state.buttonKey)}</PrimaryButtonNonActiveStyled>
                            </Tooltip>
                        )}
                    </ModalBoxStyled>
                </Modal>
            </WithdrawMoneyInternalContext.Provider>
        </WithdrawMoneyModalContext.Provider>
    )
};

export const useWithdrawMoneyModalCotext = contextFactoryCreate('WithdrawMoney', WithdrawMoneyModalContext);
export default WithdrawMoneyModalProvider;
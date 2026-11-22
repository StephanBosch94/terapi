import Box from "@mui/material/Box";
import InputAdornment from "@mui/material/InputAdornment";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { snackbarConstants } from "components/core/Snackbar";
import { useTranslation } from "components/core/TranslationProvider";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import { ModalStyled } from "components/styles/ModalStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import Tooltip from "components/styles/Tooltip";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { depositMoney, selectDepositMoneyResponse, selectDepositMoneyStatus } from "core/redux/userWalletSlice";
import { useRouter } from "next/router";
import { enqueueSnackbar } from "notistack";
import { createContext, useEffect, useState } from "react";
import InputCurrncyAdornment from "./common/InputCurrencyAdornment";
import { usePriceLocalization } from "core/hooks/priceHook";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

const PrimaryButtonNonActiveStyled = styled(PrimaryButtonRectangle)({
    backgroundColor: COLORS.BLACK_10,
    color: COLORS.WHITE,
    width: "100%",
    ":hover": {
        backgroundColor: COLORS.BLACK_10,
        color: COLORS.WHITE,
        cursor: "initial"
    }
}) as typeof PrimaryButtonRectangle;

type DepositMoneyModalContextType = {
    setOpen: () => void
} | undefined;

const DepositMoneyModalContext = createContext<DepositMoneyModalContextType>(undefined);

const DepositMoneyModalProvider = ({ children }: { children: any }) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const [amount, setAmount] = useState<number | undefined>();

    const { currencyCode } = useAppDataContext();
    const dispatch = useAppDispatch();
    const router = useRouter();
    const depositMoneyStatus = useAppSelector(selectDepositMoneyStatus);
    const depositMoneyResponse = useAppSelector(selectDepositMoneyResponse);
    const isButtonActive = amount !== undefined && amount > 0;

    useEffect(() => {
        if (depositMoneyStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.STUDENT_DASHBOARD.FAILED_TO_DEPOSIT_MONEY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }, [depositMoneyStatus]);

    useEffect(() => {
        if (depositMoneyStatus !== LoadingStatusEnum.Loaded) return;
        if (!depositMoneyResponse) return;
        if (!depositMoneyResponse.redirectUrl) return;
        router.push(depositMoneyResponse.redirectUrl);
    }, [depositMoneyStatus, depositMoneyResponse]);

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
        setOpen(true);
    }

    const handleChangeAmount = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value as string;
        const parsed = parseInt(value);
        const newAmount = !isNaN(parsed) ? parsed : undefined;
        setAmount(newAmount);
    }

    const handleClick = () => {
        if (!isButtonActive) return;
        dispatch(depositMoney({ amount, currencyCode: currencyCode ?? COMMON_CONSTANTS.INTERNAL_CURRENCY_CODE }));
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
        <DepositMoneyModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <Box sx={{ backgroundColor: COLORS.WHITE, display: "flex", flexDirection: "column" }}>
                        <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                            <H3Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_MONEY)}</H3Styled>
                            <CloseIconButton onClick={handleClose} />
                        </Box>
                        <Body1Styled sx={{ marginBottom: "16px" }}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_DESCRIPTION)}</Body1Styled>
                        <TextFieldStyled
                            type="number"
                            value={amount}
                            onChange={handleChangeAmount}
                            placeholder={t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_PLACEHOLDER_TEXT_FIELD)}
                            InputProps={{ endAdornment: <InputCurrncyAdornment /> }}
                        />
                        <Box sx={{ display: "flex", justifyContent: "space-between", marginTop: "24px", width: "100%" }}>
                            {isButtonActive ? (
                                <PrimaryButtonActiveStyled onClick={handleClick}>
                                    {depositMoneyStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT) : <CircularProgressCustom color={COLORS.WHITE} />}
                                </PrimaryButtonActiveStyled>
                            ) : (
                                <Tooltip text={t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_MONEY_TOOLTIP)}>
                                    <PrimaryButtonNonActiveStyled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT)}</PrimaryButtonNonActiveStyled>
                                </Tooltip>
                            )}
                        </Box>
                    </Box>
                </ModalBoxStyled>
            </ModalStyled>
        </DepositMoneyModalContext.Provider>
    )
}

export const useDepositMoneyModalContext = contextFactoryCreate('DepositMoneyModal', DepositMoneyModalContext);
export default DepositMoneyModalProvider;
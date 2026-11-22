import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import { ModalStyled } from "components/styles/ModalStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { createContext, useEffect, useRef, useState } from "react";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import H5Styled from "components/styles/typography/H5Styled";
import H3Styled from "components/styles/typography/H3Styled";
import Avatar from "@mui/material/Avatar";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { FilterEmployeesResponseBody, setEmployeesFilter } from "core/redux/userSlice";
import { selectTopUpPaymentStatus, selectUserWalletStatistics, setUserWalletStatistics, topUpPayment } from "core/redux/userWalletSlice";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

const AvatarSmallStyled = styled(Avatar)({
    width: "40px",
    height: "40px",
    border: `3px solid ${COLORS.WHITE}`,
    marginLeft: "-8px"
}) as typeof Avatar;

const AvatarMoreStyled = styled(Box)({
    width: "40px",
    height: "40px",
    border: `1px solid ${COLORS.BLACK_10}`,
    marginLeft: "-8px",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    color: COLORS.PRIMARY,
    borderRadius: "50%",
    backgroundColor: COLORS.WHITE,
}) as typeof Box;

const CaptionBlackTextStyled = styled(CaptionTextStyled)({
    color: COLORS.BLACK
}) as typeof CaptionTextStyled;

type SendMoneyMultiModalContextType = {
    setOpen: () => void
} | undefined;

const SendMoneyMultiModalContext = createContext<SendMoneyMultiModalContextType>(undefined);

const componentSettings = {
    numberOfUserCircles: 5
}
const getTotalAmount = (amount: number | undefined, count: number | undefined) => {
    const result = (amount ?? 0) * (count ?? 0);
    return !isNaN(result) ? result : 0;
}

const SendMoneyMultiModalProvider = ({ children, filter }: { children: any, filter: FilterEmployeesResponseBody }) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const [amount, setAmount] = useState<number | undefined>();
    const topUpPaymentStatus = useAppSelector(selectTopUpPaymentStatus);
    const submitted = useRef(false);
    const { enqueueSnackbar } = useSnackbar();
    const numberOfAdditionalUsers = (filter.size ?? 0) - componentSettings.numberOfUserCircles;
    const totalAmount = getTotalAmount(amount, filter.size);
    const walletStatistics = useAppSelector(selectUserWalletStatistics);
    const dispatch = useAppDispatch();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    useEffect(() => {
        if (!submitted.current) return;
        if (topUpPaymentStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.SUCCESSFULLY_SEND_MONEY), { variant: "success" });
            submitted.current = false;
        }
        else if (topUpPaymentStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.FAILED_TO_SEND_MONEY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            submitted.current = false;
        };
    }, [topUpPaymentStatus]);

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    const handleChangeAmount = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setAmount(parseInt(value));
    }

    const handleClick = async () => {
        submitted.current = true;
        if (!walletStatistics) return;
        if (!amount) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.AMOUNT_MUST_BE_GREATER_THEN_ZERO), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }
        if (!walletStatistics.amount || walletStatistics.amount < totalAmount) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.NOT_ENOUGH_MONEY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }
        await dispatch(topUpPayment({ amount }))
        await dispatch(setUserWalletStatistics())
        await dispatch(setEmployeesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                skip: 0,
                take: 200
            }
        }))
        setOpen(false);
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
        <SendMoneyMultiModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <Box sx={{ backgroundColor: COLORS.WHITE, display: "flex", flexDirection: "column", gap: "24px" }}>
                        <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                            <H3Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.SEND_MONEY_TITLE)}</H3Styled>
                            <CloseIconButton onClick={handleClose} />
                        </Box>
                        <Box sx={{ display: "flex", justifyContent: "start", flexFlow: "row", alignItems: "center" }}>
                            {filter.data
                                .slice(0, componentSettings.numberOfUserCircles)
                                .map(x => (
                                    <AvatarSmallStyled key={x.userId!} src={getProfilePhotoThumbnailUrl(x)} />
                                ))}
                            {numberOfAdditionalUsers > 0 ? (
                                <AvatarMoreStyled>
                                    <H5Styled>+{numberOfAdditionalUsers}</H5Styled>
                                </AvatarMoreStyled>
                            ) : <></>}
                        </Box>
                        <Box sx={{ display: "flex", justifyContent: "space-between", width: "100%", flexFlow: "column noWrap", gap: "4px" }} >
                            <CaptionBlackTextStyled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.AMOUNT_FOR_EACH)}</CaptionBlackTextStyled>
                            <TextFieldStyled type="number" value={amount} onChange={handleChangeAmount} placeholder={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.AMOUNT_SEND_PLACEHOLDER)} />
                        </Box>
                        <Box>
                            <CaptionBlackTextStyled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.TOTAL_AMOUNT_TO_SEND, { totalAmount: `${totalAmount} Kč` })}</CaptionBlackTextStyled>
                        </Box>
                        <PrimaryButtonActiveStyled onClick={handleClick}>
                            {topUpPaymentStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.SEND_MONEY_BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                        </PrimaryButtonActiveStyled>
                    </Box>
                </ModalBoxStyled>
            </ModalStyled>
        </SendMoneyMultiModalContext.Provider >
    )
}

export const useSendMoneyMultiModalContext = contextFactoryCreate('SendMoneyMultiModal', SendMoneyMultiModalContext);
export default SendMoneyMultiModalProvider;
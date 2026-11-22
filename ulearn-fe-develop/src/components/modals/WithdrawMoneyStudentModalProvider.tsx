import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { snackbarConstants } from "components/core/Snackbar";
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
import { useSnackbar } from "notistack";
import { createContext, useEffect, useRef, useState } from "react";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { FilterEmployeesResponseBody, selectShortUserInfo, setEmployeesFilter } from "core/redux/userSlice";
import H5Styled from "components/styles/typography/H5Styled";
import H3Styled from "components/styles/typography/H3Styled";
import Avatar from "@mui/material/Avatar";
import Body1Styled from "components/styles/typography/Body1Styled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { extractMoney, selectExtractMoneyStatus, setUserWalletStatistics } from "core/redux/userWalletSlice";
import { getUserName } from "core/services/Utils/userUtils";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%"
})) as typeof PrimaryButtonRectangle;

const AvatarStyledSmall = styled(Avatar)({
    width: "40px",
    height: "40px"
}) as typeof Avatar;

type WithdrawMoneyStudentModalContextType = {
    setOpen: () => void
} | undefined;

const WithdrawMoneyStudentModalContext = createContext<WithdrawMoneyStudentModalContextType>(undefined);

const WithdrawMoneyStudentModalProvider = ({ children, user }: { children: any, user: FilterEmployeesResponseBody['data'][0] }) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const [amount, setAmount] = useState<number | undefined>();
    const submitted = useRef(false);
    const { enqueueSnackbar } = useSnackbar();
    const extractMoneyStatus = useAppSelector(selectExtractMoneyStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    useEffect(() => {
        if (!submitted.current) return;
        if (extractMoneyStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.WITHDRAW_STUDENT_MONEY_MODAL.SUCCESS), { variant: "success" });
            submitted.current = false;
        }
        else if (extractMoneyStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.WITHDRAW_STUDENT_MONEY_MODAL.FAILED), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            submitted.current = false;
        };
    }, [extractMoneyStatus]);

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
        if (!amount) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.AMOUNT_MUST_BE_GREATER_THEN_ZERO), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }
        if (!user.wallet?.amount || user.wallet.amount < amount) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.SEND_MONEY_MODAL.NOT_ENOUGH_MONEY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }
        
        await dispatch(extractMoney({ amount, userId: user.userId! }))
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
        <WithdrawMoneyStudentModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <Box sx={{ backgroundColor: COLORS.WHITE, display: "flex", flexDirection: "column", gap: "24px" }}>
                        <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                            <H3Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.WITHDRAW_STUDENT_MONEY_MODAL.TITLE)}</H3Styled>
                            <CloseIconButton onClick={handleClose} />
                        </Box>
                        <Box sx={{ display: "flex", justifyContent: "start", flexFlow: "row", gap: "12px", alignItems: "center" }}>
                            <AvatarStyledSmall src={getProfilePhotoThumbnailUrl(user)} />
                            <Box sx={{ display: "flex", justifyContent: "center", flexFlow: "column", gap: "4px" }}>
                                <H5Styled>{getUserName(user)}</H5Styled>
                                <Body1Styled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.COMPANY_DASHBOARD.AVAILABLE_BALANCE, { amount: `${user.wallet?.amount ?? 0} Kč` }) }} />
                            </Box>
                        </Box>
                        <Box sx={{ display: "flex", justifyContent: "space-between", width: "100%", flexFlow: "column noWrap", gap: "4px" }} >
                            <CaptionTextStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.AMOUNT)}</CaptionTextStyled>
                            <TextFieldStyled type="number" value={amount} onChange={handleChangeAmount} placeholder={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.AMOUNT_WITHDRAW_PLACEHOLDER)} />
                        </Box>
                        <PrimaryButtonActiveStyled onClick={handleClick}>
                            {extractMoneyStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.WITHDRAW_STUDENT_MONEY_MODAL.BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                        </PrimaryButtonActiveStyled>
                    </Box>
                </ModalBoxStyled>
            </ModalStyled>
        </WithdrawMoneyStudentModalContext.Provider >
    )
}

export const useWithdrawMoneyStudentModalContext = contextFactoryCreate('WithdrawMoneyStudentModal', WithdrawMoneyStudentModalContext);
export default WithdrawMoneyStudentModalProvider;
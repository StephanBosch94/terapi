import React, { useContext, useEffect, useState } from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import H2Styled from "./styles/typography/H2Styled";
import PStyled from "./styles/typography/PStyled";
import GmailIcon from "assets/gmail.svg";
import OutlookIcon from "assets/outlook.svg";
import { WhiteIconButton } from "./styles/WhiteButtonIcon";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import { COLORS } from "./styles/constants/ColorsConstatnts";
import {
    selectActivateEmailStatus,
    selectResendEmailStatus,
    selectResendEmailStatusCode,
    selectSendedCodeLatestInfo,
    setActivateEmail,
    setResendCode,
    setSendedCodeLatestInfo
} from "core/redux/registrationSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { useSnackbar } from "notistack";
import moment from "moment";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import CircleProgressStyled from "components/styles/CircularProgressStyled";
import { snackbarConstants } from "./core/Snackbar";
import Box from "@mui/material/Box";
import Link from "next/link";
import Body1Styled from "./styles/typography/Body1Styled";
import InputFieldLabel from "./form/InputFieldLabel";
import { TextFieldStyled } from "./styles/TextFieldStyled";
import { ErrorResponse, nameof } from "core/services/Types/AppTypes";
import { isDispatchFulfilled, isDispatchRejected } from "core/services/Utils/apiUtil";
import { PrimaryButtonRectangle } from "./styles/PrimaryButtonRectangle";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { UIContext } from "./core/UIContextProvider";

const ButtonsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
    justifyContent: "center",
    alignItems: "center",
    width: "100%",
    flexDirection: { xs: "column", md: "row" },
    a: {
        width: { xs: "100%", md: "auto" },
        textDecoration: "none"
    }
})) as typeof Box;

const ConfirmEmailBoxStyled = styled(Box)(() => ({
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    gap: "24px",
    maxWidth: "550px"
})) as typeof Box;

const ConfirmEmailHeadingBoxStyled = styled(Box)(() => ({
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    gap: "16px"
})) as typeof Box;

const ResendEmailBoxStyled = styled(Box)(() => ({
    width: "100%",
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "center",
    "p:hover": {
        textDecoration: "underline",
        cursor: "pointer"
    }
})) as typeof Box;

const ResendEmailTypographyStyled = styled(Body1Styled)(() => ({
    color: COLORS.BLUE,
    fontWeight: "500",
})) as typeof Body1Styled;

const AttributeBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "4px",
    width: "100%"
})) as typeof Box;

const DeviderBoxStyled = styled(Box)(() => sx({
    width: "100%",
    height: "1px",
    backgroundColor: COLORS.BLACK_10
})) as typeof Box;

type ContinueButtonPropsType = {
    isButtonActive: boolean
}
const ContinueButtonStyled = styled(PrimaryButtonRectangle, {
    shouldForwardProp: (prop) => prop !== nameof<ContinueButtonPropsType>('isButtonActive')
})<ContinueButtonPropsType>(({ isButtonActive }) => ({
    background: isButtonActive ? COLORS.PRIMARY : COLORS.PRIMARY_40,
    ":hover": {
        cursor: isButtonActive ? "pointer" : "default",
        background: isButtonActive ? COLORS.PRIMARY_HOVER : COLORS.PRIMARY_40
    }
}));

const ResendEmailButton = () => {
    const resendEmailStatus = useAppSelector(selectResendEmailStatus);
    const resendEmailStatusCode = useAppSelector(selectResendEmailStatusCode);
    const sendedCodeLatestInfo = useAppSelector(selectSendedCodeLatestInfo);
    const [countdown, setCountdown] = useState(0);

    const dispatch = useAppDispatch();
    const { enqueueSnackbar } = useSnackbar();
    const { t } = useTranslation();

    // Show forbidden in case of sending without waiting for timeout
    useEffect(() => {
        if (resendEmailStatus === LoadingStatusEnum.Error && resendEmailStatusCode === 403) {
            enqueueSnackbar(t(TRANSLATION_KEYS.CONFIRM_EMAIL.ERRORS.WAIT_FOR_TIMEOUT), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (resendEmailStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.CONFIRM_EMAIL.EMAIL_WAS_SENT), { variant: "success" });
        }
    }, [resendEmailStatus]);


    // Synchonize timer
    useEffect(() => {
        if (!sendedCodeLatestInfo) return;
        const interval = setInterval(() => {
            const now = moment();
            const prev = moment(sendedCodeLatestInfo.created);
            const diff = now.diff(prev, 'seconds');
            setCountdown(COMMON_CONSTANTS.EMAIL.RESEND_EMAIL_AWAITING_TIME - diff);
        }, 1000);

        return () => clearInterval(interval);
    }, [sendedCodeLatestInfo]);

    useEffect(() => {
        dispatch(setSendedCodeLatestInfo());
    }, []);

    const resendEmailClick = () => {
        dispatch(setResendCode());
    }

    return (
        <ResendEmailBoxStyled>
            <Box onClick={resendEmailClick}>
                <ResendEmailTypographyStyled>
                    {resendEmailStatus === LoadingStatusEnum.Loading
                        ? <CircleProgressStyled />
                        : (
                            <>
                                {countdown > 0
                                    ? t(TRANSLATION_KEYS.CONFIRM_EMAIL.RESEND_BUTTON_WITH_TIMER, { time: countdown })
                                    : t(TRANSLATION_KEYS.CONFIRM_EMAIL.RESEND_BUTTON_WITHOUT_TIMER)
                                }
                            </>
                        )}
                </ResendEmailTypographyStyled>
            </Box>
        </ResendEmailBoxStyled>
    )
}

const ConfirmEmailBox = ({ onSuccess }: { onSuccess: () => void }) => {
    const { t } = useTranslation();
    const userInfo = useAppSelector(selectShortUserInfo);
    const [code, setCode] = useState("");
    const { screenType } = useContext(UIContext);
    const isButtonActive = code.length >= 6;
    const dispatch = useAppDispatch();
    const { enqueueSnackbar } = useSnackbar();
    const activationStatus = useAppSelector(selectActivateEmailStatus);

    const handleChangeCode = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setCode(e.target.value);
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (e.key === 'Enter') {
            handleContinue();
        }
    }

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    }, [code]);

    const handleContinue = async () => {
        const result = await dispatch(setActivateEmail({ registrationCode: code }));
        if (isDispatchFulfilled(result)) {
            onSuccess();
        }

        if (isDispatchRejected(result)) {
            const errorResponse = result.payload as ErrorResponse;
            if (errorResponse.status === 403) {
                enqueueSnackbar(t(TRANSLATION_KEYS.CONFIRM_EMAIL.ERRORS.INVALID_CODE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            }
            else {
                enqueueSnackbar(t(TRANSLATION_KEYS.CONFIRM_EMAIL.ERRORS.FAILED_TO_VALIDATE_CODE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            }
        }
    }

    return (
        <ConfirmEmailBoxStyled>
            <ConfirmEmailHeadingBoxStyled>
                <H2Styled>{t(TRANSLATION_KEYS.CONFIRM_EMAIL.TITLE)}</H2Styled>
                <PStyled dangerouslySetInnerHTML={{ __html: (t(TRANSLATION_KEYS.CONFIRM_EMAIL.DESCRIPTION, { email: userInfo?.email ?? "" })) }} sx={{ color: COLORS.BLACK }} />
            </ConfirmEmailHeadingBoxStyled>
            {screenType === "desktop" ? (
                <ButtonsBoxStyled>
                    <Link target="_blank" rel="noopener noreferrer" href="https://mail.google.com/mail/u/0/#search/yaspeak.com">
                        <WhiteIconButton icon={<GmailIcon />} text={t(TRANSLATION_KEYS.CONFIRM_EMAIL.OPEN_GMAIL)} />
                    </Link>
                    <Link target="_blank" rel="noopener noreferrer" href="https://outlook.office.com/mail/">
                        <WhiteIconButton icon={<OutlookIcon />} text={t(TRANSLATION_KEYS.CONFIRM_EMAIL.OPEN_OUTLOOK)} />
                    </Link>
                </ButtonsBoxStyled>
            ) : <></>}
            <DeviderBoxStyled />
            <AttributeBoxStyled>
                <InputFieldLabel titleKey={TRANSLATION_KEYS.CONFIRM_EMAIL.CODE} />
                <TextFieldStyled
                    fullWidth
                    placeholder={t(TRANSLATION_KEYS.CONFIRM_EMAIL.CODE_PLACEHOLDER)}
                    type="text"
                    value={code}
                    onChange={handleChangeCode}
                />
            </AttributeBoxStyled>
            <ContinueButtonStyled
                fullWidth
                isButtonActive={isButtonActive}
                onClick={handleContinue}
            >
                {activationStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMMON.CONTINUE) : <CircularProgressCustom color={COLORS.WHITE} />}
            </ContinueButtonStyled>
            <ResendEmailButton />
        </ConfirmEmailBoxStyled>
    );
};

export default ConfirmEmailBox;
import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H2Styled from "./styles/typography/H2Styled";
import PStyled from "./styles/typography/PStyled";
import { PrimaryButtonRectangle } from "./styles/PrimaryButtonRectangle";
import { selectForgottenPasswordStatus, setForgottenPasword } from "core/redux/registrationSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CheckIcon from "assets/check-circle-fill.svg";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "./core/Snackbar";
import CircularProgressCustom from "./styles/CircularProgressStyled";
import { TextFieldStyled } from "./styles/TextFieldStyled";
import Box from "@mui/material/Box";
import LargeModalBoxStyled from "./styles/boxes/LargeModalBoxStyled";

const PrimaryButtonStyled = styled(PrimaryButtonRectangle)(() => ({
  width: "100%",
  height: "44px"
})) as typeof PrimaryButtonRectangle;

const IconBoxStyled = styled(Box)(() => ({
  display: "block",
  color: COLORS.PRIMARY,
  height: "80px",
  width: "80px",
  svg: {
    marginTop: "30px", // workaround for shifted svg
    transform: "scale(5)",
  }
})) as typeof Box;

const InnerBoxStyled = styled(Box)(() => ({
  display: "flex",
  flexFlow: "column noWrap",
  gap: "16px",
  justifyContent: "center",
  alignItems: "center",
})) as typeof Box;

const PStyledBox = styled(PStyled)(() => ({
  color: COLORS.BLACK_60
})) as typeof PStyled;

const ForgottenPasswordHeader = () => {
  const { t } = useTranslation();

  return (
    <InnerBoxStyled>
      <H2Styled>{t(TRANSLATION_KEYS.FORGOT_PASSWORD.TITLE)}</H2Styled>
      <PStyledBox>{t(TRANSLATION_KEYS.FORGOT_PASSWORD.DESCRIPTION)}</PStyledBox>
    </InnerBoxStyled>
  )
}

const ForgottenPasswordLoaded = ({ email }: { email: string }) => {
  const { t } = useTranslation();

  return (
    <>
      <ForgottenPasswordHeader />
      <InnerBoxStyled>
        <IconBoxStyled><CheckIcon /></IconBoxStyled>
        <PStyledBox sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.AUTHORIZATION.FORGOTTEN_PASSWORD.EMAIL_SENT, { email })}</PStyledBox>
      </InnerBoxStyled>
    </>
  )
}

const ForgottenPasswordInitial = ({ email, onChange }: { email: string, onChange: (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => void }) => {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();

  const handleSendForgottenPassword = () => {
    dispatch(setForgottenPasword({ email }))
  }

  return (
    <>
      <ForgottenPasswordHeader />

      <TextFieldStyled type="email" fullWidth placeholder={t(TRANSLATION_KEYS.AUTHORIZATION.LOGIN_PLACEHOLDER)} onChange={onChange} />
      <PrimaryButtonStyled onClick={handleSendForgottenPassword}>{t(TRANSLATION_KEYS.FORGOT_PASSWORD.BUTTON_TEXT)}</PrimaryButtonStyled>
    </>
  )
}

const ForgottenPasswordLoading = () => {

  return (
    <CircularProgressCustom />
  )
}

const ForgotPasswordBox = () => {
  const { t } = useTranslation();
  const [email, setEmail] = React.useState<string>('');
  const forgottenPasswordStatus = useAppSelector(selectForgottenPasswordStatus);
  const { enqueueSnackbar } = useSnackbar();

  const onChange = (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    setEmail(e.target.value);
  }

  useEffect(() => {
    if (forgottenPasswordStatus === LoadingStatusEnum.Error) {
      enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.FORGOTTEN_PASSWORD.FAILD_TO_SEND_EMAIL), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
    }
  }, [forgottenPasswordStatus]);

  return (
    <LargeModalBoxStyled sx={{ maxWidth: { xs: "auto", md: "520px" }, display: "flex", flexFlow: "column noWrap", gap: "32px" }}>
      {forgottenPasswordStatus === undefined || forgottenPasswordStatus === LoadingStatusEnum.Error ? <ForgottenPasswordInitial email={email} onChange={onChange} /> : <></>}
      {forgottenPasswordStatus === LoadingStatusEnum.Loading ? <ForgottenPasswordLoading /> : <></>}
      {forgottenPasswordStatus === LoadingStatusEnum.Loaded ? <ForgottenPasswordLoaded email={email} /> : <></>}
    </LargeModalBoxStyled >
  );
};

export default ForgotPasswordBox;
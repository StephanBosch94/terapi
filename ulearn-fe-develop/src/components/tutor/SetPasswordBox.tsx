import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import React, { useEffect, useState } from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import H2Styled from "components/styles/typography/H2Styled";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { PrimaryButton } from "components/styles/PrimaryButton";
import CheckCircleFillIcon from "assets/check-circle-fill.svg";
import EyeIcon from "assets/show-password.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { useSnackbar } from "notistack";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";
import { useAppSelector } from "core/redux/reduxHooks";
import { RootState } from "core/redux/store";
import { selectChangePasswordStatus, selectResetPasswordStatus } from "core/redux/userSlice";

const SetPasswordBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    width: { xs: "auto", md: "520px" },
    alignItems: "center",
    gap: "32px"
})) as typeof Box;

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: { md: "center" },
    width: "100%",
    gap: "16px"
})) as typeof Box;

const ValidationItemBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    gap: "8px"
})) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%",
    height: "44px",
})) as typeof PrimaryButton;

const EyeIconBox = styled(Box)(() => sx({
    position: "absolute",
    top: "0",
    right: "0",
    height: "100%",
    display: "flex",
    alignItems: "center",
    padding: "0 14px",
    ":hover": {
        cursor: "pointer",
        color: COLORS.PRIMARY
    },
    "svg": {
        transform: "scale(1.25)"
    }
})) as typeof Box;

type PasswordInputType = "password" | "text";
type ViewType = "set_new_password" | "recover_password";


const config: {
    type: ViewType,
    titleKey: string,
    descriptionKey: string,
    buttonKey: string,
    selectStatus: (state: RootState) => LoadingStatusEnum | undefined
}[] = [
        {
            type: "set_new_password",
            titleKey: TRANSLATION_KEYS.SET_PASSWORD.TITLE,
            descriptionKey: TRANSLATION_KEYS.SET_PASSWORD.DESCRIPTION,
            buttonKey: TRANSLATION_KEYS.SET_PASSWORD.BUTTON_TEXT,
            selectStatus: selectChangePasswordStatus
        },
        {
            type: "recover_password",
            titleKey: TRANSLATION_KEYS.RESET_PASSWORD.TITLE,
            descriptionKey: TRANSLATION_KEYS.RESET_PASSWORD.DESCRIPTION,
            buttonKey: TRANSLATION_KEYS.RESET_PASSWORD.BUTTON_TEXT,
            selectStatus: selectResetPasswordStatus
        }
    ]

const ValidationItem = ({ titleKey, satisfied }: { titleKey: string, satisfied: boolean }) => {
    const { t } = useTranslation();

    return (
        <ValidationItemBoxStyled>
            <Box sx={{ color: satisfied ? COLORS.PRIMARY : COLORS.BLACK_40 }}>
                <CheckCircleFillIcon />
            </Box>
            <Body1Styled>{t(titleKey)}</Body1Styled>
        </ValidationItemBoxStyled>
    )
}

const SetPasswordBox = ({ type, onClick }: { type: ViewType, onClick: (newPassword: string) => void }) => {
    const specialCharacters = ["!", "\"", "#", "$", "&", "’", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~"];
    const { t } = useTranslation();
    const [password, setPassword] = React.useState<string>('');
    const [passwordFieldType, setPasswordFieldType] = useState<PasswordInputType>("password");
    const minPaswordLengthSatisfied = password.length > 6;
    const specialCharacterAdded = [...password].some(x => specialCharacters.some(y => x === y));
    const passwordOk = minPaswordLengthSatisfied && specialCharacterAdded;
    const { enqueueSnackbar } = useSnackbar();
    const { titleKey, descriptionKey, buttonKey, selectStatus } = config.find(x => x.type === type)!;
    const requestStatus = useAppSelector(selectStatus);

    useEffect(() => {
        if (requestStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.VALIDATIONS.FAILED_TO_SET_PASSWORD), { variant: "error" });
        }
    }, [requestStatus]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    }, [password]);

    const handleSetNewPassword = () => {
        if (!minPaswordLengthSatisfied) {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.VALIDATIONS.INVALID_MIN_PASSWORD_LENGTH), { variant: "error" });
            return;
        }
        if (!specialCharacterAdded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.VALIDATIONS.MISSING_SPECIAL_CHARACTERS), { variant: "error" });
            return;
        }
        onClick(password)
    }

    const handleClickShowPassword = () => {
        setPasswordFieldType(prev => prev === "password" ? "text" : "password");
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (e.key === 'Enter') {
            handleSetNewPassword();
        }
    }

    return (
        <SetPasswordBoxStyled>
            <HeadingBoxStyled>
                <H2Styled>{t(titleKey)}</H2Styled>
                <PStyled>{t(descriptionKey)}</PStyled>
            </HeadingBoxStyled>
            <Box sx={{ display: "flex", gap: "24px", alignItems: "center", flexFlow: "column", width: "100%" }}>
                <Box sx={{ width: "100%", position: "relative" }}>
                    <TextFieldStyled sx={{ width: "100%" }}
                        type={passwordFieldType}
                        fullWidth
                        placeholder={t(TRANSLATION_KEYS.AUTHORIZATION.PASSWORD_PLACEHOLDER)}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <EyeIconBox
                        onClick={handleClickShowPassword}
                        color={passwordFieldType === "password" ? COLORS.BLACK_40 : COLORS.PRIMARY}
                    >
                        <EyeIcon />
                    </EyeIconBox>
                </Box>
                <Box sx={{ display: "flex", flexFlow: "column noWrap", width: "100%", gap: "12px" }}>
                    <ValidationItem titleKey={TRANSLATION_KEYS.AUTHORIZATION.VALIDATIONS.MIN_PASSWORD_LENGTH} satisfied={minPaswordLengthSatisfied} />
                    <ValidationItem titleKey={TRANSLATION_KEYS.AUTHORIZATION.VALIDATIONS.SPECIAL_CHARACTER} satisfied={specialCharacterAdded} />
                </Box>
            </Box>
            <PrimaryButtonStyled
                sx={{
                    background: passwordOk ? COLORS.PRIMARY : COLORS.PRIMARY_40,
                    ":hover": {
                        cursor: passwordOk ? "pointer" : "default",
                        background: passwordOk ? COLORS.PRIMARY_HOVER : COLORS.PRIMARY_40
                    }
                }}
                onClick={handleSetNewPassword}
            >
                {t(buttonKey)}
            </PrimaryButtonStyled>
        </SetPasswordBoxStyled>
    );
};

export default SetPasswordBox;
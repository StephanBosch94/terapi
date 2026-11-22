import { useAppDispatch } from "core/redux/reduxHooks";
import { createContext, useContext, useMemo, useState } from "react";
import React, { useEffect } from "react";
import { useAppSelector } from "core/redux/reduxHooks";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import LargeModalBoxStyled from "components/styles/boxes/LargeModalBoxStyled";
import routes, { staticRoutes } from "routes/routes";
import Link from "next/link";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import PStyled from "components/styles/typography/PStyled";
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import { SocialButtonStyled } from "components/styles/SocialButtonStyled";
import H2Styled from "components/styles/typography/H2Styled";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { selectErrorCode, selectInternalAuthStatus, setAuthUsingInternalLogin, updateIsAuthorized } from "core/redux/authSlice";
import { getGoogleApiLink as getGoogleAuthUrl, getFacebookUrl as getFacebookAuthUrl, getFacebookConfigKeys, getGoogleConfigKeys } from "core/services/Utils/authUtil";
import { useTranslation, translationConfig } from "components/core/TranslationProvider";
import GoogleIcon from "assets/google.svg";
import FacebookIcon from "assets/facebook-social.svg";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { selectRegisterErrorType, selectRegisterStatus, setRegister } from "core/redux/registrationSlice";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import FormControlLabel from "@mui/material/FormControlLabel";
import Image from 'next/image';
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import { UIContext } from "components/core/UIContextProvider";
import Body1Styled from "components/styles/typography/Body1Styled";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import IconButton from "@mui/material/IconButton";
import ShowPasswordIcon from "assets/show-password.svg";
import HidePasswordIcon from "assets/hide-password.svg";
import InputAdornment from "@mui/material/InputAdornment";
import OutlinedInput from "@mui/material/OutlinedInput";
import { isDispatchFulfilled } from "core/services/Utils/apiUtil";
import OauthPopup from "react-oauth-popup";
import { nameof } from "core/services/Types/AppTypes";
import { paths } from "apiTypes";
import { getTimeZoneName } from "core/services/Utils/timeUtil";
import AuthService from "core/services/AuthService";

const TypographySeparatorStyled = styled(Typography)(() => sx({
    my: 2
})) as typeof Typography;

const IconButtonStyled = styled(IconButton)(() => ({
    ":hover": {
        backgroundColor: COLORS.PRIMARY_10
    }
})) as typeof IconButton;

const IconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.25)",
        color: COLORS.BLACK
    }
})) as typeof Box;

const OutlinedInputStyled = styled(OutlinedInput)(() => ({
    background: COLORS.WHITE,
    border: `1px solid ${(COLORS.BLACK_10)}`,
    borderRadius: "4px",
    boxSizing: "border-box",
    ":hover": {
        borderColor: COLORS.BLACK_40
    },
    "fieldset": {
        border: "none"
    },
    "input": {
        paddingTop: "10px",
        paddingBottom: "10px",
        fontWeight: 400,
        color: COLORS.BLACK
    },
    "& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button": {
        display: "none",
    },
    "& input[type=number]": {
        MozAppearance: "textfield",
    },
}));

const ConnectionLineBoxStyled = styled(Box)(() => sx({
    width: "100%",
    height: "1px",
    backgroundColor: COLORS.BLACK_10,
})) as typeof Box;

const LinkStyled = styled(Box)(() => sx({
    color: COLORS.BLACK,
    textDecoration: "underline",
    fontSize: "1rem",
    ":hover": {
        cursor: "pointer",
        color: COLORS.PRIMARY,
    }
})) as typeof Box;

const DividerStyledBox = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    gap: "16px",
    width: "100%"
})) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%",
    height: "44px",
    marginTop: "8px"
})) as typeof PrimaryButton;

const CenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px",
    width: "100%",
})) as typeof Box;

const PStyledText = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
})) as typeof PStyled;

const PAlternativeTypographyStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
})) as typeof PStyled;

const PAlternativeLinkTypographyStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    textDecoration: "underline",
    ":hover": {
        cursor: "pointer",
        color: COLORS.PRIMARY,
    }
})) as typeof PStyled;

const LinkBoxStyledBox = styled(Box)(() => sx({
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

const RememberMeFormControlStyled = styled(FormControlLabel)(() => sx({
    display: "flex",
    gap: "8px",
    minWidth: "max-content",
})) as typeof FormControlLabel;

const ContentBoxStyledBox = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    height: "fit-content",
    alignItems: "space-between",
    width: "100%"
})) as typeof Box;

const MiddleStyledBox = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    flex: "1 1 auto",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const BottomBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "0 1 auto"
})) as typeof Box;

const StreechBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    justifyContent: "space-between",
    alignSelf: "stretch",
    width: { xs: "100%", md: "412px" },
    gap: { md: "32px" }
})) as typeof Box;

const AutImageBoxStyled = styled(Box)(() => sx({
    "img": {
        borderRadius: "8px"
    }
})) as typeof Box;

const MiddlePanelBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    width: "100%",
    justifyContent: "space-between"
})) as typeof Box;

const FooterTextStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK, textAlign: "left",
    "a": {
        color: `${COLORS.BLACK}`,
        ":hover": {
            color: `${COLORS.PRIMARY}`
        }
    }
})) as typeof PStyled;

const SocialIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(1.125)",
    }
})) as typeof Box;

export type PageType = "login" | "registration";
type AuthentifcationPropsType = {
    userRole: UserRoleEnum,
    pageType: PageType,
    hideTitle?: boolean,
    hideImage?: boolean,
    onSuccessExternal: () => void,
    onSucessInternal: () => void,
    onClose?: () => void,
    onAlternativeClick?: () => void,
};

type SocialButtonsType = {
    google: string,
    facebook: string
};

type PageOptionsType = {
    type: PageType,
    userRoles: UserRoleEnum[],
    title: string,
    buttonText: string,
    alternativeActionText: string,
    alternativeLinkText: string,
    footerText: string,
    socialButtons: SocialButtonsType
}

type SocialButtonPropsType = {
    icon: JSX.Element,
    oauthUri: string,
    beApiUrl: string,
    redirectUri: string,
    translationKey: string,
    userRole: UserRoleEnum,
    onSuccess?: () => void
}

const signUpSocialButtons: SocialButtonsType = {
    google: TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.GOOGLE_BUTTON_TEXT,
    facebook: TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.FACEBOOK_BUTTON_TEXT
}

const loginSocialButtons: SocialButtonsType = {
    google: TRANSLATION_KEYS.AUTHORIZATION.LOGIN.GOOGLE_BUTTON_TEXT,
    facebook: TRANSLATION_KEYS.AUTHORIZATION.LOGIN.FACEBOOK_BUTTON_TEXT
}

const ModalTypes: PageOptionsType[] = [
    {
        type: "login",
        userRoles: [UserRoleEnum.Student, UserRoleEnum.Admin, UserRoleEnum.Tutor, UserRoleEnum.Company],
        title: TRANSLATION_KEYS.LOG_IN_BOX.TITLE,
        buttonText: TRANSLATION_KEYS.LOG_IN_BOX.BUTTON_TEXT,
        alternativeActionText: TRANSLATION_KEYS.LOG_IN_BOX.ALTERNATIVE_ACTION_TEXT,
        alternativeLinkText: TRANSLATION_KEYS.AUTHORIZATION.SIGN_UP.TITLE,
        footerText: TRANSLATION_KEYS.LOG_IN_BOX.FOOTER_TEXT,
        socialButtons: loginSocialButtons
    },
    {
        type: "registration",
        userRoles: [UserRoleEnum.Student],
        title: TRANSLATION_KEYS.SIGN_UP_BOX.STUDENT_TITLE,
        buttonText: TRANSLATION_KEYS.SIGN_UP_BOX.BUTTON_TEXT,
        alternativeActionText: TRANSLATION_KEYS.SIGN_UP_BOX.ALTERNATIVE_ACTION_TEXT,
        alternativeLinkText: TRANSLATION_KEYS.AUTHORIZATION.LOGIN.TITLE,
        footerText: TRANSLATION_KEYS.SIGN_UP_BOX.FOOTER_TEXT,
        socialButtons: signUpSocialButtons
    },
    {
        type: "registration",
        userRoles: [UserRoleEnum.Tutor],
        title: TRANSLATION_KEYS.SIGN_UP_BOX.TUTOR_TITLE,
        buttonText: TRANSLATION_KEYS.SIGN_UP_BOX.BUTTON_TEXT,
        alternativeActionText: TRANSLATION_KEYS.SIGN_UP_BOX.ALTERNATIVE_ACTION_TEXT,
        alternativeLinkText: TRANSLATION_KEYS.AUTHORIZATION.LOGIN.TITLE,
        footerText: TRANSLATION_KEYS.SIGN_UP_BOX.FOOTER_TEXT,
        socialButtons: signUpSocialButtons
    },
    {
        type: "registration",
        userRoles: [UserRoleEnum.Company],
        title: TRANSLATION_KEYS.SIGN_UP_BOX.COMPANY_TITLE,
        buttonText: TRANSLATION_KEYS.SIGN_UP_BOX.BUTTON_TEXT,
        alternativeActionText: TRANSLATION_KEYS.SIGN_UP_BOX.ALTERNATIVE_ACTION_TEXT,
        alternativeLinkText: TRANSLATION_KEYS.AUTHORIZATION.LOGIN.TITLE,
        footerText: TRANSLATION_KEYS.SIGN_UP_BOX.FOOTER_TEXT,
        socialButtons: signUpSocialButtons
    }
]

const getModalOptions = (role: UserRoleEnum, type: PageType) => {
    return ModalTypes.filter(x => x.userRoles.some(y => y === role))
        .find(x => x.type === type)!;
}

const SocialButton = ({ icon, oauthUri, translationKey, beApiUrl, userRole, redirectUri, onSuccess }: SocialButtonPropsType) => {
    const { t } = useTranslation();
    const { enqueueSnackbar } = useSnackbar();
    const { locale } = useAppDataContext();
    const dispatch = useAppDispatch();
    const [loadingStatus, setLoadingStatus] = useState<LoadingStatusEnum | undefined>();

    const handleCodeReceived = async (code: string, params: URLSearchParams) => {
        if (!code || code === "") {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.ERRORS.INVALID_VERIFICATION_CODE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }

        setLoadingStatus(LoadingStatusEnum.Loading);
        try {
            await AuthService.externalProviderAuth({
                userRoleId: userRole,
                timeZone: getTimeZoneName(),
                authenticationCode: code,
                redirectUrl: redirectUri,
                languageCode: locale ?? translationConfig.fallbackCode
            }, beApiUrl);
        }
        catch (err) {
            setLoadingStatus(LoadingStatusEnum.Error);
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.ERRORS.INVALID_VERIFICATION_CODE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        dispatch(updateIsAuthorized(true));
        setLoadingStatus(LoadingStatusEnum.Loaded);
        onSuccess?.();
    }

    const handleClose = () => { }

    return (
        <OauthPopup
            url={oauthUri}
            onCode={handleCodeReceived}
            onClose={handleClose}
            width={500}
            height={600}
            title=""
        >
            <Box sx={{ width: "100%", display: "inline-flex" }}>
                <SocialButtonStyled>
                    {loadingStatus !== LoadingStatusEnum.Loading ? (
                        <>
                            <SocialIconBoxStyled>{icon}</SocialIconBoxStyled>
                            <PStyledText>{t(translationKey)}</PStyledText>
                        </>
                    ) : <CircularProgressCustom />}
                </SocialButtonStyled>
            </Box>
        </OauthPopup>
    )
}

const SocialButtons = ({ userRole, pageType, onSuccess }: { userRole: UserRoleEnum, pageType: PageType, onSuccess?: () => void }) => {
    const { envConfigurations } = useAppDataContext();
    const options = useMemo(() => getModalOptions(userRole, pageType), [userRole, pageType]);

    return (
        <Box sx={{ display: "flex", flexFlow: "column", gap: "12px", width: "100%" }}>
            <SocialButton
                icon={<GoogleIcon />}
                translationKey={options.socialButtons.google}
                userRole={userRole}
                onSuccess={onSuccess}
                oauthUri={getGoogleAuthUrl(envConfigurations, userRole)}
                beApiUrl={nameof<paths>('/api/auth/login/google')}
                redirectUri={envConfigurations[getGoogleConfigKeys(userRole).redirectUrl] as string}
            />
            <SocialButton
                icon={<FacebookIcon />}
                translationKey={options.socialButtons.facebook}
                userRole={userRole}
                onSuccess={onSuccess}
                oauthUri={getFacebookAuthUrl(envConfigurations, userRole)}
                beApiUrl={nameof<paths>('/api/auth/login/facebook')}
                redirectUri={envConfigurations[getFacebookConfigKeys(userRole).redirectUrl] as string}
            />
        </Box>
    )
}

const SeparatorBox = () => {
    const { t } = useTranslation();

    return (
        <DividerStyledBox>
            <ConnectionLineBoxStyled />
            <TypographySeparatorStyled>{t(TRANSLATION_KEYS.AUTHORIZATION.OR)}</TypographySeparatorStyled>
            <ConnectionLineBoxStyled />
        </DividerStyledBox>
    )
}

const AlternativeOptionsBox = ({ textKey, linkKey, onClick }: { textKey: string, linkKey: string, onClick: () => void }) => {
    const { t } = useTranslation();
    return (
        <Box sx={{ width: "100%", display: "inline-flex", justifyContent: { xs: "center", md: "start" }, marginTop: { xs: "24px", md: "0px" }, gap: "4px" }}>
            <PAlternativeTypographyStyled>{t(textKey)}</PAlternativeTypographyStyled>
            <LinkBoxStyledBox onClick={onClick}>
                <PAlternativeLinkTypographyStyled>{t(linkKey)}</PAlternativeLinkTypographyStyled>
            </LinkBoxStyledBox>
        </Box>
    )
}

const ModalTitle = ({ titleKey, onClose }: { titleKey: string, onClose?: () => void }) => {
    const { t } = useTranslation();
    return (
        <Box sx={{ display: "flex", width: "100%", justifyContent: "space-between", marginBottom: { xs: "40px", md: "0px" }, flex: "0 1 auto" }}>
            <Box>
                <H2Styled sx={{ color: COLORS.BLACK, fontSize: "1.75rem !important" }}>
                    {t(titleKey)}
                </H2Styled>
            </Box>
            {onClose ? <CloseIconButton onClick={onClose} /> : <></>}
        </Box>
    )
}

const IMAGE_DIMENSIONS = {
    WIDTH: 400,
    HEIGHT: 540
}
const SignUpImage = ({ userRole }: { userRole: UserRoleEnum }) => (
    <AutImageBoxStyled>
        {userRole === UserRoleEnum.Tutor ? (
            <Image src="/images/registration_tutor.png" placeholder="blur" blurDataURL={LOADING_IMAGES.TUTOR_HERO.MAIN} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
        {userRole === UserRoleEnum.Student ? (
            <Image src="/images/registration_student.png" placeholder="blur" blurDataURL={LOADING_IMAGES.REGISTRATION.MAIN} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
        {userRole === UserRoleEnum.Company ? (
            <Image src="/images/registration_company.png" placeholder="blur" blurDataURL={LOADING_IMAGES.REGISTRATION.COMPANY} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
    </AutImageBoxStyled>
)

const LogInImage = ({ userRole }: { userRole: UserRoleEnum }) => (
    <AutImageBoxStyled>
        {userRole === UserRoleEnum.Tutor ? (
            <Image src="/images/registration_tutor.png" placeholder="blur" blurDataURL={LOADING_IMAGES.TUTOR_HERO.MAIN} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
        {userRole === UserRoleEnum.Student ? (
            <Image src="/images/registration_student.png" placeholder="blur" blurDataURL={LOADING_IMAGES.REGISTRATION.MAIN} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
        {userRole === UserRoleEnum.Company ? (
            <Image src="/images/registration_company.png" placeholder="blur" blurDataURL={LOADING_IMAGES.REGISTRATION.COMPANY} quality={75} alt="" width={IMAGE_DIMENSIONS.WIDTH} height={IMAGE_DIMENSIONS.HEIGHT} />
        ) : <></>}
    </AutImageBoxStyled>
)

const SignUpBox = ({ userRole, pageType, hideTitle, hideImage, onClose, onAlternativeClick, onSuccessExternal, onSucessInternal }: AuthentifcationPropsType) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const { locale } = useAppDataContext();
    const [email, setEmail] = React.useState<string>('');
    const options = useMemo(() => getModalOptions(userRole, pageType), [userRole, pageType]);
    const registerError = useAppSelector(selectRegisterErrorType);
    const registerStatus = useAppSelector(selectRegisterStatus);
    const { enqueueSnackbar } = useSnackbar();
    const { screenType } = useContext(UIContext);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    }, [email]);

    const handleSignUp = async () => {
        const result = await dispatch(setRegister({
            email,
            userRoleId: userRole,
            languageCode: locale ?? translationConfig.fallbackCode,
            timezone: getTimeZoneName()
        }))
        if (isDispatchFulfilled(result)) {
            dispatch(updateIsAuthorized(true));
            onSucessInternal();
        } else {
            enqueueSnackbar(t(TRANSLATION_KEYS.AUTHORIZATION.ERRORS.FAILED_TO_REGISTER), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (e.key === 'Enter') {
            handleSignUp();
        }
        if (e.key === 'Escape') {
            onClose?.();
        }
    }

    return (
        <>
            {!hideImage ? <SignUpImage userRole={userRole} /> : <></>}
            <StreechBoxStyled>
                {!hideTitle ? <ModalTitle titleKey={options.title} onClose={onClose} /> : <></>}
                <MiddleStyledBox>
                    <SocialButtons userRole={userRole} pageType={pageType} onSuccess={onSuccessExternal} />
                    <SeparatorBox />
                    <CenterBoxStyled>
                        <Box sx={{ display: "flex", alignItems: "start", gap: "8px", flexFlow: "column", width: "100%" }}>
                            <TextFieldStyled type="email" fullWidth placeholder={t(TRANSLATION_KEYS.AUTHORIZATION.LOGIN_PLACEHOLDER)} onChange={(e) => setEmail(e.target.value)} />
                            {registerError === "already_exists" ?
                                (<Body1Styled sx={{ color: COLORS.RED, textAlign: "left" }}>{t(TRANSLATION_KEYS.AUTHORIZATION.ERRORS.EMAIL_EXISTS)}</Body1Styled>) : <></>}
                        </Box>
                        <PrimaryButtonStyled onClick={handleSignUp}>{registerStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.SIGN_UP_BOX.BUTTON_TEXT) : <CircularProgressCustom color={COLORS.WHITE} />}</PrimaryButtonStyled>
                        <FooterTextStyled dangerouslySetInnerHTML={{ __html: (t(options.footerText, { urlPrivacyPolicy: staticRoutes.gdpr, urlTermsOfUse: routes.tutorLandingPage ? staticRoutes.tutorTermsOfUse : staticRoutes.termsOfUse })) }} />
                    </CenterBoxStyled>
                </MiddleStyledBox>
                {onAlternativeClick ? (
                    <BottomBoxStyled>
                        <AlternativeOptionsBox textKey={options.alternativeActionText} linkKey={options.alternativeLinkText} onClick={onAlternativeClick} />
                    </BottomBoxStyled>
                ) : <></>}
            </StreechBoxStyled>
        </>
    )
}

const LogInBox = ({ userRole, pageType, hideTitle, hideImage, onClose, onAlternativeClick, onSuccessExternal }: AuthentifcationPropsType) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const [email, setEmail] = React.useState<string>('');
    const [password, setPassword] = React.useState<string>('');
    const [showPassword, setShowPassword] = React.useState(false);
    const internalLoginErrorCode = useAppSelector(selectErrorCode);
    const [checkbox, setCheckbox] = React.useState<boolean>(false);
    const options = useMemo(() => getModalOptions(userRole, pageType), [userRole, pageType]);
    const authStatus = useAppSelector(selectInternalAuthStatus);
    const handleClickShowPassword = () => setShowPassword((show) => !show);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    }, [email, password]);

    const handleLogin = async () => {
        const result = await dispatch(setAuthUsingInternalLogin({
            login: email,
            password: password,
        }));
        if (isDispatchFulfilled(result)) {
            onSuccessExternal();
        }
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            handleLogin();
        }
        if (e.key === 'Escape') {
            onClose?.();
        }
    }

    return (
        <>
            {!hideImage ? <LogInImage userRole={userRole} /> : <></>}
            <StreechBoxStyled>
                {!hideTitle ? <ModalTitle titleKey={options.title} onClose={onClose} /> : <></>}
                <MiddleStyledBox>
                    <SocialButtons userRole={userRole} pageType={pageType} onSuccess={onSuccessExternal} />
                    <SeparatorBox />
                    <CenterBoxStyled>
                        <TextFieldStyled type="email" fullWidth placeholder={t(TRANSLATION_KEYS.AUTHORIZATION.LOGIN_PLACEHOLDER)} onChange={(e) => setEmail(e.target.value)} />
                        <Box sx={{ display: "flex", alignItems: "start", gap: "8px", flexFlow: "column", width: "100%" }}>
                            <OutlinedInputStyled type={showPassword ? 'text' : 'password'} fullWidth placeholder={t(TRANSLATION_KEYS.AUTHORIZATION.PASSWORD_PLACEHOLDER)} onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
                                endAdornment={
                                    <InputAdornment position="end">
                                        <IconButtonStyled onClick={handleClickShowPassword}>
                                            {showPassword ? <IconBoxStyled><HidePasswordIcon /></IconBoxStyled> : <IconBoxStyled><ShowPasswordIcon /></IconBoxStyled>}
                                        </IconButtonStyled>
                                    </InputAdornment>
                                } />
                            {internalLoginErrorCode ? (<Body1Styled sx={{ color: COLORS.RED, textAlign: "left" }}>{t(TRANSLATION_KEYS.AUTHORIZATION.INTERNAL_LOGIN_UNAUTHORIZED)}</Body1Styled>) : <></>}
                        </Box>
                        <MiddlePanelBoxStyled>
                            <RememberMeFormControlStyled
                                control={<MuiCheckboxStyled checked={checkbox} />}
                                label={t(TRANSLATION_KEYS.LOG_IN_BOX.KEEP_ME_SIGNED_IN)}
                                onChange={() => setCheckbox(prev => !prev)}
                            />
                            <Link href={routes.forgotPassword.path}><LinkStyled><PStyled sx={{ color: COLORS.BLACK, ":hover": { cursor: "pointer", color: COLORS.PRIMARY, }, minWidth: "max-content" }}>{t(TRANSLATION_KEYS.LOG_IN_BOX.FORGOT_PASWWORD)}</PStyled></LinkStyled></Link>
                        </MiddlePanelBoxStyled>
                        <PrimaryButtonStyled onClick={handleLogin}>{authStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.LOG_IN_BOX.BUTTON_TEXT) : <CircularProgressCustom color={COLORS.WHITE} />}</PrimaryButtonStyled>
                    </CenterBoxStyled>
                </MiddleStyledBox>
                {onAlternativeClick ? (
                    <BottomBoxStyled>
                        <AlternativeOptionsBox textKey={options.alternativeActionText} linkKey={options.alternativeLinkText} onClick={onAlternativeClick} />
                    </BottomBoxStyled>
                ) : <></>}
            </StreechBoxStyled>
        </>
    )
}

const Authentication = (props: AuthentifcationPropsType) => (
    <>
        {props.pageType === "login" ? <LogInBox {...props} /> : <></>}
        {props.pageType === "registration" ? <SignUpBox {...props} /> : <></>}
    </>
)

export default Authentication;
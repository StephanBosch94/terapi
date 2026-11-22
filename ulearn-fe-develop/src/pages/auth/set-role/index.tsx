import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import NavbarWithoutLinks from "components/navbar/NavbarWithoutLinks";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { UserInfoResponse, selectShortUserInfo, selectUpdateUserInfoStatus, selectUserInfo, selectUserInfoStatus, setUserInfo, updateProfileSettingsInfo } from "core/redux/userSlice";
import { useEffect, useState } from "react";
import StudentIcon from "assets/emoji/student.svg";
import TutorIcon from "assets/emoji/tutor.svg";
import CompanyIcon from "assets/emoji/company.svg";
import { toAppUserIm } from "core/services/mappers/appUserMapper";
import routes from "routes/routes";
import PStyled from "components/styles/typography/PStyled";
import RadioButton from "components/styles/RadioButton";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import H2Styled from "components/styles/typography/H2Styled";
import { isDispatchFulfilled } from "core/services/Utils/apiUtil";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import H3Styled from "components/styles/typography/H3Styled";

type RoleType = { type: UserRoleEnum, descriptionKey: string, icon: () => JSX.Element }
const ROLES: RoleType[] = [
    { type: UserRoleEnum.Student, descriptionKey: TRANSLATION_KEYS.SET_ROLE.STUDENT_DESCRIPTION, icon: () => <StudentIcon /> },
    { type: UserRoleEnum.Company, descriptionKey: TRANSLATION_KEYS.SET_ROLE.COMPANY_DESCRIPTION, icon: () => <CompanyIcon /> },
    { type: UserRoleEnum.Tutor, descriptionKey: TRANSLATION_KEYS.SET_ROLE.TUTOR_DESCRIPTION, icon: () => <TutorIcon /> },
];

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    width: "100%",
    height: "100%",
    minHeight: 0,
    flexFlow: "row nowrap",
    flexGrow: 1,
    gap: "12px",
    alignItems: "center",
    justifyContent: "center",
})) as typeof ContainerStyled;

const SetRoleOuterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    maxWidth: "794px",
    border: { md: `1px solid ${COLORS.BLACK_10}` },
    flexFlow: "column noWrap",
    borderRadius: "4px",
    padding: { xs: 0, md: "40px" },
    gap: "40px"
})) as typeof Box;

const SetRoleOuterBoxPlaceholderStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    width: "694px",
    height: "268px",
})) as typeof Box;

const RoleItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    border: `1px solid ${COLORS.BLACK_10}`,
    flexFlow: "column noWrap",
    borderRadius: "4px",
    padding: "20px",
    gap: "12px",
    width: { xs: "100%", md: "auto" },
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY}`,
    }
})) as typeof Box;

const RoleItemBoxHeaderStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    width: "100%"
})) as typeof Box;

const RolesBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: { xs: "column noWrap", md: "row noWrap" },
    width: { xs: "100%", md: "auto" },
    gap: "20px"
})) as typeof Box;

const CenteringBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const DesciptionTypographyStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: "600"
})) as typeof PStyled;

const UserRolesLoaded = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const [userRoleId, setUserRoleId] = useState<number | undefined>();
    const status = useAppSelector(selectUpdateUserInfoStatus);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        setUserRoleId(userInfo.userRoleId);
    }, [userInfo]);

    const handleContinue = async () => {
        const result = await dispatch(updateProfileSettingsInfo({
            ...toAppUserIm(userInfo),
            userStateId: UserStateEnum.Registration,
            userRoleId: userRoleId
        }))
        if (isDispatchFulfilled(result)) {
            location.replace(routes.home.path);
        }
        else {
            enqueueSnackbar(t(TRANSLATION_KEYS.SET_ROLE.FAILED_TO_UPDATE_ROLE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
    }

    return (
        <>
            <CenteringBoxStyled>
                <H3Styled>{t(TRANSLATION_KEYS.SET_ROLE.TITLE)}</H3Styled>
            </CenteringBoxStyled>
            <RolesBoxStyled>
                {ROLES.map(x => (
                    <RoleItemBoxStyled key={x.type} onClick={() => setUserRoleId(x.type)}>
                        <RoleItemBoxHeaderStyled>
                            {x.icon()}
                            <RadioButton checked={x.type === userRoleId} />
                        </RoleItemBoxHeaderStyled>
                        <DesciptionTypographyStyled>{t(x.descriptionKey)}</DesciptionTypographyStyled>
                    </RoleItemBoxStyled>
                ))}
            </RolesBoxStyled>
            <CenteringBoxStyled>
                <PrimaryButton onClick={handleContinue}>
                    {status !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.SET_ROLE.CONTINUE_BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                </PrimaryButton>
            </CenteringBoxStyled>
        </>
    )
}

const UserRolesLoading = () => (
    <SetRoleOuterBoxPlaceholderStyled>
        <CircularProgressCustom />
    </SetRoleOuterBoxPlaceholderStyled>
)

const UserRolesError = () => (
    <SetRoleOuterBoxPlaceholderStyled>
        <CircularProgressCustom />
    </SetRoleOuterBoxPlaceholderStyled>
)

const UserRoles = () => {
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);

    return (
        <SetRoleOuterBoxStyled>
            {userInfoStatus === LoadingStatusEnum.Loading ? <UserRolesLoading /> : <></>}
            {userInfoStatus === LoadingStatusEnum.Error ? <UserRolesError /> : <></>}
            {userInfoStatus === LoadingStatusEnum.Loaded && userInfo ? <UserRolesLoaded userInfo={userInfo} /> : <></>}
        </SetRoleOuterBoxStyled>
    )
}

const SetRolePage = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);
    const dispatch = useAppDispatch();

    useEffect(() => {
        if (shortUserInfo?.stateId && shortUserInfo.stateId !== UserStateEnum.RoleSetup){
            location.replace(routes.home.path);
        }
    }, [shortUserInfo]);

    useEffect(() => {
        dispatch(setUserInfo());
    }, []);

    return (
        <Box sx={{ height: "100%", display: "flex", flexFlow: "column noWrap" }}>
            <NavbarWithoutLinks />
            <ContainerCustomStyled>
                <UserRoles />
            </ContainerCustomStyled>
        </Box>
    )
}

export default withAppClientSideProvider(SetRolePage);
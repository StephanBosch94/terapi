import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { createContext, useContext, useEffect, useState } from "react";
import Reservations, { ReservationTranslationKeys } from "components/profile/Reservations";
import { ReservationTimeEnum } from "core/constants/enums/ReservationTimeEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import CupIcon from "assets/emoji/cup.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";;
import { useTranslation } from "components/core/TranslationProvider";
import { selectMyTutors, selectMyTutorsStatus, setTutors, selectUserInfo, selectUserInfoStatus, setUserInfo, UserInfoResponse, selectShortUserInfo } from "core/redux/userSlice";
import H4Styled from "components/styles/typography/H4Styled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import InfoIcon from "assets/info.svg";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { TabsBoxStyled } from "components/styles/boxes/TabsBoxStyled";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import { DashboardBoxStyled } from "components/styles/boxes/DashboardBoxStyled";
import { CloseIconButton } from "components/styles/CloseIconModalStyled";
import Tooltip from "components/styles/Tooltip";
import DashboardUsers from "components/dashboard/DashboardUsers";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { useStudentRegistrationModal, withStudentRegistrationModalProvider } from "components/modals/StudentRegistrationModalProvider";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import MatchingTutors from "components/tutor/MatchingTutors";
import Box from "@mui/material/Box";
import CircularProgress, { CircularProgressProps, circularProgressClasses } from "@mui/material/CircularProgress";
import TextField from "@mui/material/TextField";
import ActiveSessionOptional from "components/navbar/ActiveSessionOptional";
import Transactions from "components/dashboard/Transactions";
import ErrorPage from "components/pages/ErrorPage";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { UIContext } from "components/core/UIContextProvider";
import PStyled from "components/styles/typography/PStyled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { useRouter } from "next/router";
import routes from "routes/routes";
import { ModalStyled } from "components/styles/ModalStyled";
import Wallet from "components/dashboard/Wallet";
import WaveIcon from "assets/emoji/wave.svg";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { GetStudentPrivateStatisticsResponse, selectStudentPrivateStatistics, selectStudentPrivateStatisticsStatus, setStudentPrivateStatistics } from "core/redux/userStatisticsSlice";
import ErrorLarge from "components/core/error/ErrorLarge";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import InjectIcon from "components/core/icons/InjectIcon";
import { ContainerStyled } from "components/styles/ContainerStyled";
import BookLessonOptional from "components/navbar/BookLessonOptional";

const BackgroundBoxStyled = styled(Box)(() => sx({
    py: { xs: "16px", md: "24px" }
})) as typeof Box;

const EmojiIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    width: "24px",
    height: "24px",
    justifyContent: "center",
    svg: {
        transform: "scale(1.5)"
    }
})) as typeof Box;

const GoalContentBox = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "16px",
    width: "100%",
    display: "flex",
    flexDirection: "row",
    justifyItems: "space-between",
    gap: "8px",
    borderRadius: "4px",
    height: "100%",
})) as typeof Box;

const StudentDashboardRowBox = styled(Box)(() => sx({
    display: "flex",
    flexDirection: { xs: "column", md: "row" },
    alignItems: "flexStart",
    gap: { xs: "24px", md: "24px" },
    width: "100%",
})) as typeof Box;

const StudentDashboardBox = styled(Box)(() => sx({
    display: "flex",
    gap: { xs: "24px", md: "24px" },
    flexDirection: "column",
    width: "100%",
})) as typeof Box;

const StatisticsContentBox = styled(Box)(() => sx({
    padding: "16px",
    display: "flex",
    flexDirection: "row",
    border: `1px solid ${COLORS.BLACK_10}`,
    height: "100%",
    borderRadius: "4px"
})) as typeof Box;

const StatisticsContentColumnBox = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    width: "100%",
    gap: "8px"
})) as typeof Box;

const HeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: "16px"
})) as typeof Box;

const IconStyledBoxWhite = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.WHITE
    }
})) as typeof Box;

const TitleBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "4px"
})) as typeof Box;

const InfoIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.BLACK_60,
        ":hover": {
            color: COLORS.BLACK,
            cursor: "pointer"
        }
    }
})) as typeof Box;

const TabsContentStyledBox = styled(Box)(() => sx({
    display: "flex",
    gap: { xs: "24px", md: "32px" },
    flexDirection: "column",
    paddingBottom: "24px",
    minHeight: "60vh",
})) as typeof Box;

const UpcomingTranslationKeys: ReservationTranslationKeys = {
    noEvents: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_UPCOMING_EVENTS,
    noEventsDescription: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_UPCOMING_EVENTS_DESCRIPTION,
    showMoreBtnText: TRANSLATION_KEYS.PROFILE.RESERVATIONS.SHOW_MORE_UPCOMING_EVENTS
}

const PastTranslationKeys: ReservationTranslationKeys = {
    noEvents: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_PAST_EVENTS,
    noEventsDescription: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_PAST_EVENTS_DESCRIPTION,
    showMoreBtnText: TRANSLATION_KEYS.PROFILE.RESERVATIONS.SHOW_MORE_PAST_EVENTS
}

type StudentDashboardTabType = "overview" | "matching_tutors" | "tutors" | "transactions"

type StudentDashboardInternalContextType = {
    userInfo: UserInfoResponse,
    currentTab: StudentDashboardTabType,
    setCurrentTab: (newTab: StudentDashboardTabType) => void
} | undefined;

const Tabs: { type: StudentDashboardTabType, translationKey: string }[] =
    [{ type: "overview", translationKey: TRANSLATION_KEYS.STUDENT_DASHBOARD.OVERVIEW },
    { type: "matching_tutors", translationKey: TRANSLATION_KEYS.MATCHING_TUTORS.SECTION_TITLE },
    { type: "tutors", translationKey: TRANSLATION_KEYS.STUDENT_DASHBOARD.TUTORS },
    { type: "transactions", translationKey: TRANSLATION_KEYS.DASHBOARD.TRANSACTIONS }]

const StudentDashboardInternalContext = createContext<StudentDashboardInternalContextType | undefined>(undefined);
const useInternalPageContext = contextFactoryCreate('StudentDashboardInternal', StudentDashboardInternalContext);

const WalletBox = () => {
    const { t } = useTranslation();
    const [showAddMoneyPopup, setShowAddMoneyPopup] = useState(false);

    const handleAddMoneyClick = () => {
        setShowAddMoneyPopup(!showAddMoneyPopup);
    }

    return (
        <>
            <DashboardBoxStyled >
                <H4Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.WALLET_TITLE)}</H4Styled>
                <Wallet />
            </DashboardBoxStyled>
            <DepositMoneyModal open={showAddMoneyPopup} onClose={handleAddMoneyClick} />
        </>
    )
}

const DepositMoneyModal = ({ open, onClose }: { open: boolean, onClose: () => void }) => {
    const { t } = useTranslation();

    const handleClose = () => {
        onClose();
    }

    return (
        <ModalStyled open={open} onClose={handleClose}>
            <ModalBoxStyled>
                <Box sx={{ backgroundColor: COLORS.WHITE, display: "flex", flexDirection: "column" }}>
                    <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                        <H3Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_MONEY)}</H3Styled>
                        <CloseIconButton onClick={handleClose} />
                    </Box>
                    <Body1Styled sx={{ marginBottom: "16px" }}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_DESCRIPTION)}</Body1Styled   >
                    <TextField id="outlined-basic" size="small" type="number" variant="outlined" placeholder={t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_PLACEHOLDER_TEXT_FIELD)} />
                    <Box sx={{ display: "flex", justifyContent: "space-between", marginTop: "24px" }}>
                        <PrimaryButtonRectangle sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT)}</PrimaryButtonRectangle>
                    </Box>
                </Box>
            </ModalBoxStyled>
        </ModalStyled>
    )
}

const CustomCircularProgress = (props: CircularProgressProps & { children: any }) => {
    return (
        <Box sx={{ width: "100px", height: "100px", position: "relative" }}>
            <CircularProgress
                variant="determinate"
                sx={{
                    color: COLORS.PRIMARY_10,
                    zIndex: "800"
                }}
                size={40}
                thickness={4}
                {...props}
                value={100}
            />
            <Box sx={{ display: "flex", position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)" }}>
                {props.children}
            </Box>
            <CircularProgress
                variant="indeterminate"
                sx={{
                    color: COLORS.PRIMARY,
                    animationDuration: '550ms',
                    position: 'absolute',
                    zIndex: "900",
                    left: 0,
                    [`& .${circularProgressClasses.circle}`]: {
                        strokeLinecap: 'round',

                    },
                    'svg': {
                        transform: "rotate(180deg)",
                    }
                }}
                size={40}
                thickness={4}
                {...props}
            />
        </Box>
    );
}

const CurrentWeekProgress = ({ statistics }: { statistics: GetStudentPrivateStatisticsResponse }) => {
    const { userInfo } = useInternalPageContext();
    const { t } = useTranslation();
    const count = statistics.numberOfCompletedLessonsThisWeek ?? 0;
    const total = userInfo.weeklyGoal?.weeklyGoalId ?? 0;

    return (
        <CustomCircularProgress variant="determinate" value={statistics.currentWeeklyGoalProgress ?? 0} thickness={7} size={100} >
            <Body1Styled sx={{ color: COLORS.BLACK, fontWeight: "500", lineHeight: "20px" }}>
                {t(TRANSLATION_KEYS.STUDENT_DASHBOARD.CURRENT_WEEK_GOAL_PROGRESS, { count, total })}
            </Body1Styled>
        </CustomCircularProgress>
    )
}

const rewardsConstants = {
    desktop: {
        numberOfCupsInRow: 8,
        numberOfRows: 3
    },
    mobile: {
        numberOfCupsInRow: 4,
        numberOfRows: 3
    }
}

const TotalProgressRewards = ({ statistics }: { statistics: GetStudentPrivateStatisticsResponse }) => {
    const { userInfo } = useInternalPageContext();
    const { screenType } = useContext(UIContext);
    const dimensions = screenType === "desktop" ? rewardsConstants.desktop : rewardsConstants.mobile;
    const maxNumberOfCups = dimensions.numberOfCupsInRow * dimensions.numberOfRows - 1;

    const totalCompleted = statistics.totalWeeklyGoalsCompleted ?? 0;
    const totalTruncated = totalCompleted <= maxNumberOfCups ? totalCompleted : maxNumberOfCups;
    const rest = totalCompleted - totalTruncated;
    const hasThirdRow = totalCompleted < dimensions.numberOfCupsInRow * 2;
    const { t } = useTranslation();

    return (
        <Box sx={{ display: "flex", height: "100%", flexFlow: "column noWrap", gap: "8px", justifyContent: hasThirdRow ? "space-between" : "flex-start" }}>
            <Box sx={{ display: "flex", flexFlow: "row wrap", gap: "8px" }}>
                {Array(totalTruncated).fill(1).map((x, idx) => (
                    <EmojiIconStyled key={idx}><CupIcon /></EmojiIconStyled>
                ))}
                {rest ? (
                    <Box sx={{ display: "flex", backgroundColor: COLORS.PRIMARY_10, border: `1px solid ${COLORS.PRIMARY}`, alignItems: "center", justifyContent: "center", borderRadius: "50%", padding: "4px" }}>
                        <CaptionTextStyled sx={{ color: COLORS.PRIMARY, fontWeight: "500", lineHeight: "16px" }}>+{rest}</CaptionTextStyled>
                    </Box>
                ) : <></>}
            </Box>
            {hasThirdRow && screenType === "desktop" ? (
                <CaptionTextStyled sx={{ color: COLORS.BLACK, fontWeight: "400", fontSize: "0.875rem" }}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.WEEKLY_GOAL_MOTIVATION)}</CaptionTextStyled>
            ) : <></>}
        </Box>
    )
}

const GoalSetLoaded = ({ statistics }: { statistics: GetStudentPrivateStatisticsResponse }) => {
    const { t } = useTranslation();
    const { userInfo } = useInternalPageContext();

    return (
        <GoalContentBox>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "16px", width: "50%" }}>
                <TitleBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.WEEKLY_GOAL)}</Body1Styled>
                    <Tooltip text={t(TRANSLATION_KEYS.STUDENT_DASHBOARD.WEEKLY_GOAL_TOOLTIP, { count: userInfo.weeklyGoal?.weeklyGoalId ?? 0 })}>
                        <InfoIconStyled><InfoIcon /></InfoIconStyled>
                    </Tooltip>
                </TitleBoxStyled>
                <CurrentWeekProgress statistics={statistics} />
            </Box>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "16px", width: "50%" }}>
                <TitleBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.CHALLENGE)}</Body1Styled>
                    <Tooltip text={t(TRANSLATION_KEYS.STUDENT_DASHBOARD.CHALLENGE_TOOLTIP)}>
                        <InfoIconStyled><InfoIcon /></InfoIconStyled>
                    </Tooltip>
                </TitleBoxStyled>
                <TotalProgressRewards statistics={statistics} />
            </Box>
        </GoalContentBox>
    )
}

const GoalSetLoading = () => (
    <GoalContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </GoalContentBox>
)

const GoalSetError = () => (
    <GoalContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </GoalContentBox>
)

const GoalSet = () => {
    const studentPrivateStatisticsStatus = useAppSelector(selectStudentPrivateStatisticsStatus);
    const studentPrivateStatistics = useAppSelector(selectStudentPrivateStatistics);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setStudentPrivateStatistics());
    }, []);

    return (
        <>
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Error ? <GoalSetError /> : <></>}
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Loading ? <GoalSetLoading /> : <></>}
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Loaded && studentPrivateStatistics ? <GoalSetLoaded statistics={studentPrivateStatistics} /> : <></>}
        </>
    )
}

const GoalEmpty = () => {
    const { t } = useTranslation();

    return (
        <GoalContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
            <Box sx={{ display: "inline-flex", gap: "6px", alignItems: "center" }}>
                <PStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.WEEKLY_GOAL_EMPTY_STATE_TEXT)}</PStyled>
                <CupIcon />
            </Box>
        </GoalContentBox>
    )
}

const Goal = () => {
    const { userInfo } = useInternalPageContext();
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.GOAL)}</H4Styled>
            {userInfo?.weeklyGoal ? <GoalSet /> : <GoalEmpty />}
        </DashboardBoxStyled>
    );
}

const StatisticsLoaded = ({ statistics }: { statistics: GetStudentPrivateStatisticsResponse }) => {
    const { t } = useTranslation();

    return (
        <StatisticsContentBox>
            <StatisticsContentColumnBox>
                <Body1Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.COMPLETED_LESSONS)}</Body1Styled>
                <H3Styled>{statistics.numberOfCompletedLessons ?? 0}</H3Styled>
            </StatisticsContentColumnBox>
            <StatisticsContentColumnBox>
                <Body1Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.UPCOMING_LESSONS)}</Body1Styled>
                <H3Styled>{statistics.numberOfUpcomingLessons ?? 0}</H3Styled>
            </StatisticsContentColumnBox>
        </StatisticsContentBox>
    )
}

const StatisticsLoading = () => (
    <StatisticsContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </StatisticsContentBox>
)

const StatisticsError = () => (
    <StatisticsContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </StatisticsContentBox>
)

const Statistics = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const studentPrivateStatisticsStatus = useAppSelector(selectStudentPrivateStatisticsStatus);
    const studentPrivateStatistics = useAppSelector(selectStudentPrivateStatistics);

    useEffect(() => {
        dispatch(setStudentPrivateStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.STATISTICS_TITLE)}</H4Styled>
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Error ? <StatisticsError /> : <></>}
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Loading ? <StatisticsLoading /> : <></>}
            {studentPrivateStatisticsStatus === LoadingStatusEnum.Loaded && studentPrivateStatistics ? <StatisticsLoaded statistics={studentPrivateStatistics} /> : <></>}
        </DashboardBoxStyled>
    )
}

const YourTutors = () => {
    const { t } = useTranslation();
    const users = useAppSelector(selectMyTutors);
    const usersStatus = useAppSelector(selectMyTutorsStatus);
    const dispatch = useAppDispatch();
    const router = useRouter();

    const EMPTY_CONTENT = {
        titleKey: TRANSLATION_KEYS.TUTORS.NO_RESULTS.TITLE,
        descriptionKey: TRANSLATION_KEYS.TUTORS.NO_RESULTS.DESCRIPTION,
        buttonTextKey: TRANSLATION_KEYS.TUTORS.NO_RESULTS.FIND_BUTTON,
        imagePath: "/images/not_found.png"
    }

    useEffect(() => {
        dispatch(setTutors());
    }, []);

    const handleSearchClick = () => {
        router.push(routes.search.path)
    }

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.YOUR_TUTORS)}</H4Styled>
            <DashboardUsers users={users} usersStatus={usersStatus} content={EMPTY_CONTENT} onClick={handleSearchClick} />
        </DashboardBoxStyled>
    )
}

const TransactionsTab = () => {
    const { t } = useTranslation();

    return (
        <Transactions />
    )
}

const MatchingTutorsTab = () => {
    const { t } = useTranslation();

    return (
        <Box>
            <Box sx={{ display: "flex", gap: "8px", flexFlow: "column noWrap" }}>
                <H4Styled>{t(TRANSLATION_KEYS.MATCHING_TUTORS.SECTION_TITLE)}</H4Styled>
                <Body1Styled>{t(TRANSLATION_KEYS.MATCHING_TUTORS.SECTION_DESCRIPTION)}</Body1Styled>
            </Box>
            <Box sx={{ marginTop: "16px" }}>
                <MatchingTutors />
            </Box>
        </Box>
    )
}

const OverviewTab = () => {
    const { t } = useTranslation();

    return (
        <TabsContentStyledBox>
            <StudentDashboardBox>
                <StudentDashboardRowBox>
                    <WalletBox />
                    <Goal />
                </StudentDashboardRowBox>
                <StudentDashboardRowBox>
                    <DashboardBoxStyled>
                        <H4Styled>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.UPCOMING_EVENTS_TITLE)}</H4Styled>
                        <Reservations time={ReservationTimeEnum.Upcoming} translationKeys={UpcomingTranslationKeys} />
                    </DashboardBoxStyled>
                    <DashboardBoxStyled>
                        <H4Styled>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.PAST_EVENTS_TITLE)}</H4Styled>
                        <Reservations time={ReservationTimeEnum.Past} translationKeys={PastTranslationKeys} />
                    </DashboardBoxStyled>
                </StudentDashboardRowBox >
            </StudentDashboardBox>
        </TabsContentStyledBox>
    )
}

const TutorsTab = () => {
    return (
        <TabsContentStyledBox>
            <Statistics />
            <YourTutors />
        </TabsContentStyledBox>
    )
}

const TransactionTab = () => {

    return (
        <TabsContentStyledBox>
            <TransactionsTab />
        </TabsContentStyledBox>
    )
}

const Navigation = () => {
    const { t } = useTranslation();
    const { currentTab, setCurrentTab } = useInternalPageContext();

    return (
        <TabsBoxStyled>
            {Tabs.map((x, idx) => (
                <TabItemButtonStyled key={idx} onClick={() => setCurrentTab(x.type)}
                    sx={{
                        backgroundColor: currentTab === x.type ? COLORS.PRIMARY : "transparent",
                        color: currentTab === x.type ? COLORS.WHITE : COLORS.BLACK,
                        ":hover": { backgroundColor: currentTab === x.type ? COLORS.PRIMARY : COLORS.PRIMARY_10, color: currentTab === x.type ? COLORS.WHITE : COLORS.BLACK }
                    }}>
                    {t(x.translationKey)}
                </TabItemButtonStyled>
            ))}
        </TabsBoxStyled>
    )
}

const StudentDashboardViews = () => {
    const { currentTab } = useInternalPageContext();

    return (
        <>
            {currentTab === "overview" ? <OverviewTab /> : <></>}
            {currentTab === "matching_tutors" ? <MatchingTutorsTab /> : <></>}
            {currentTab === "tutors" ? <TutorsTab /> : <></>}
            {currentTab === "transactions" ? <TransactionTab /> : <></>}
        </>
    )
}

const WelcomeStudentGoal = () => {
    const { userInfo } = useInternalPageContext();
    const { t } = useTranslation();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <Box sx={{ display: "flex", flexDirection: "row", gap: "4px" }}>
            <H4Styled>{t(userInfo.subject?.userGoalTitleKey ?? TRANSLATION_KEYS.STUDENT_DASHBOARD.WELCOME_BACK, { userName: shortUserInfo?.firstName ?? COMMON_CONSTANTS.DEFAULT_USER_NAME })}</H4Styled>
            {userInfo.subject?.iconName ? (
                <EmojiIconStyled>
                    <InjectIcon category="emoji" iconName={userInfo.subject?.iconName ?? ""} />
                </EmojiIconStyled>
            ) : <></>}
            <EmojiIconStyled><WaveIcon /></EmojiIconStyled>
        </Box>
    )
}

const StudentDashboard = () => {
    return (
        <Box>
            <Navbar source={NavbarSourceEnum.studentPage} />
            <BackgroundBoxStyled>
                <ContainerStyled>
                    <Box sx={{ display: "flex", gap: { xs: "16px", md: "24px" }, flexDirection: "column" }}>
                        <DashboardBoxStyled>
                            <DashboardBoxStyled>
                                <WelcomeStudentGoal />
                                <ActiveSessionOptional />
                                <BookLessonOptional />
                            </DashboardBoxStyled>
                            <Navigation />
                        </DashboardBoxStyled>
                        <StudentDashboardViews />
                    </Box>
                </ContainerStyled>
            </BackgroundBoxStyled>
        </Box>
    );
};

type StudentDashboardTabPathType = { tab: string };
export type StudentDashboardTabPathParsedType = { tab: StudentDashboardTabType };
const StudentDashboardWithContext = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const [currentTab, setTab] = useState<StudentDashboardTabType>("overview");
    const router = useRouter();

    useEffect(() => {
        const query = router.query as StudentDashboardTabPathType;
        setTab(getTabType(query.tab));
    }, [router.query, router.asPath]);

    const getTabType = (query: string): StudentDashboardTabType => {
        switch (query) {
            case "overview":
                return "overview";
            case "matching_tutors":
                return "matching_tutors";
            case "tutors":
                return "tutors";
            case "transactions":
                return "transactions";
            default:
                return "overview";
        }
    }

    const handleSetTab = (newTab: StudentDashboardTabType) => {
        var query: StudentDashboardTabPathType = { tab: newTab };
        router.replace({
            pathname: routes.studentDashboard.path,
            query
        }, undefined, { shallow: true })
        setTab(newTab)
    }

    return (
        <StudentDashboardInternalContext.Provider value={{
            userInfo,
            currentTab,
            setCurrentTab: handleSetTab
        }}>
            <StudentDashboard />
        </StudentDashboardInternalContext.Provider>
    )
}

const ModalHandler = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { setOpen } = useStudentRegistrationModal();

    useEffect(() => {
        if (!userInfo) return;
        if (userInfo.stateId === UserStateEnum.Registration) {
            setOpen();
        }
    }, []);

    return (
        <></>
    )
}

const StudentDashboardPage = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);

    useEffect(() => {
        dispatch(setUserInfo());
    }, []);

    return (
        <>
            {userInfoStatus === LoadingStatusEnum.Error ? <ErrorPage /> : <></>}
            {/* Omit userInfoStatus === LoadingStatusEnum.Loading as a workaround for blinking background during registration */}
            {!userInfo ? <LinearProgressStyled /> : <></>}
            {/* Omit userInfoStatus === LoadingStatusEnum.Loaded as a workaround for blinking background during registration */}
            {userInfo ? <StudentDashboardWithContext userInfo={userInfo} /> : <></>}
            {/* Move registration handler outside of the component to prevent modal reopen on the last step */}
            {userInfo ? <ModalHandler userInfo={userInfo} /> : <></>}
        </>
    )
}

export default withAppClientSideProvider(withStudentRegistrationModalProvider(StudentDashboardPage));

import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useEffect, useState, useContext, createContext } from "react";
import Navbar from "components/navbar/Navbar";
import Reservations, { ReservationTranslationKeys } from "components/profile/Reservations";
import { ReservationTimeEnum } from "core/constants/enums/ReservationTimeEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { setStudents, selectMyStudents, selectMyStudentsStatus, setUserInfo, selectUserInfo, selectUserInfoStatus, UserInfoResponse, selectShortUserInfo } from "core/redux/userSlice";
import { useTranslation } from "components/core/TranslationProvider";
import H4Styled from "components/styles/typography/H4Styled";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";;
import UserReview from "components/profile/reviews/Review";
import StarIconFilled from "assets/star-filled.svg";
import { TabsBoxStyled } from "components/styles/boxes/TabsBoxStyled";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import { DashboardBoxStyled } from "components/styles/boxes/DashboardBoxStyled";
import Calendar from "components/calendar/TutorPrivateCalendar";
import { UIContext, ViewScreenType } from "components/core/UIContextProvider";
import DashboardUsers from "components/dashboard/DashboardUsers";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Box from "@mui/material/Box";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import ErrorPage from "components/pages/ErrorPage";
import ActiveSessionOptional from "components/navbar/ActiveSessionOptional";
import WelcomeBackUser from "components/dashboard/WelcomeBackUser";
import Transactions from "components/dashboard/Transactions";
import { useRouter } from "next/router";
import routes from "routes/routes";
import Empty from "components/core/empty/Empty";
import Wallet from "components/dashboard/Wallet";
import { GetUserWalletStatisticsResponse, selectUserWalletStatistics, selectUserWalletStatisticsStatus, setUserWalletStatistics } from "core/redux/userWalletSlice";
import ErrorLarge from "components/core/error/ErrorLarge";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import StatisticsColumnsBoxStyled from "components/styles/boxes/StatisticsColumnsBoxStyled";
import StatisticsContentBoxStyled from "components/styles/boxes/StatisticsContentBoxStyled";
import { GetTutorPrivateStatisticsResponse, GetTutorPublicStatisticsResponse, selectTutorPrivateStatistics, selectTutorPrivateStatisticsStatus, selectTutorPublicStatistics, selectTutorPublicStatisticsStatus, setTutorPrivateStatistics, setTutorPublicStatistics } from "core/redux/userStatisticsSlice";
import { getLoadingStatus } from "core/services/Utils/apiUtil";
import StatisticWithBadge from "components/dashboard/StatisticWithBadge";
import StatisticsColumnBoxStyled from "components/styles/boxes/StatisticsColumnBoxStyled";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { usePriceLocalization } from "core/hooks/priceHook";

const OUTER_GAP = { xs: "16px", md: "24px" }
const INNER_GAP = { xs: "8px", md: "16px" };
type TutorPublicPrivateStatisticsType = GetTutorPublicStatisticsResponse & GetTutorPrivateStatisticsResponse;
type TutorAllStatisticsType = TutorPublicPrivateStatisticsType & GetUserWalletStatisticsResponse;

const BackgroundBoxStyled = styled(Box)(() => sx({
    paddingTop: { xs: "16px", md: "24px" },
    display: "flex",
    flex: "1 1 auto",
    gap: OUTER_GAP,
    flexDirection: "column",
    flexFlow: "column noWrap",
    overflow: "auto"
})) as typeof Box;

const ReviewsBox = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "16px",
    display: "flex",
    flexDirection: "column",
    gap: "8px",
    borderRadius: "4px"
})) as typeof Box;

const IconStyledBoxPrimary = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const HeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center"
})) as typeof Box;

const IconStyledBoxWhite = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.WHITE
    }
})) as typeof Box;

const TabContentBoxStyled = styled(Box)(() => sx({
    height: "100%",
    display: "flex",
    flexFlow: "column noWrap",
    gap: OUTER_GAP,
    paddingBottom: "24px",
    overflow: "auto",
})) as typeof Box;

const PrimaryButtonRectangleStyled = styled(PrimaryButtonRectangle)(() => sx({
    display: "flex",
    gap: "8px",
    alignItems: "center",
    padding: "8px 16px"
})) as typeof PrimaryButtonRectangle;

const PastTranslationKeys: ReservationTranslationKeys = {
    noEvents: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_PAST_EVENTS,
    noEventsDescription: TRANSLATION_KEYS.PROFILE.RESERVATIONS.NO_PAST_EVENTS_DESCRIPTION,
    showMoreBtnText: TRANSLATION_KEYS.PROFILE.RESERVATIONS.SHOW_MORE_PAST_EVENTS
}

type TutorDashboardTabType = "calendar" | "overview" | "students" | "past_lessons" | "reviews" | "transactions"

type TutorDashboardInternalContextType = {
    userInfo: UserInfoResponse,
    currentTab: TutorDashboardTabType,
    setCurrentTab: (newTab: TutorDashboardTabType) => void
} | undefined;

const TutorDashboardInternalContext = createContext<TutorDashboardInternalContextType | undefined>(undefined);
const useInternalPageContext = contextFactoryCreate('TutorDashboardInternal', TutorDashboardInternalContext);

const Tabs: { type: TutorDashboardTabType, screenTypes: ViewScreenType[], translationKey: string }[] = [
    { type: "overview", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.OVERVIEW },
    { type: "calendar", screenTypes: ["mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.TITLE },
    { type: "students", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.STUDENTS },
    { type: "past_lessons", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.PAST_LESSONS },
    { type: "reviews", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.REVIEWS },
    { type: "transactions", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.TRANSACTIONS }
]

const getTabs = (screenType: ViewScreenType) => Tabs.filter(x => x.screenTypes.some(y => y === screenType));


const WalletBox = () => {
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WALLET)}</H4Styled>
            <Wallet />
        </DashboardBoxStyled>
    )
}

const OverviewStatisticsLoaded = ({ statistics }: { statistics: TutorAllStatisticsType }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <StatisticsContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.COMPLETED_LESSONS)}</Body1Styled>
                    <H3Styled>{statistics.numberOfLessons ?? 0}</H3Styled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.UPCOMING_LESSONS)}</Body1Styled>
                    <H3Styled>{statistics.numberOfUpcomingLessons ?? 0}</H3Styled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.REVENUE_THIS_MONTH)}
                    valueText={localize({ price: statistics.revenueThisMonth })}
                    currentValue={statistics.revenueThisMonth}
                    previousValue={statistics.revenuePreviousMonth}
                />
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.TOTAL_REVENUE)}</Body1Styled>
                    <H3Styled>{localize({ price: statistics.totalRevenue })}</H3Styled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
        </StatisticsContentBoxStyled>
    )
}

const OverviewStatisticsLoading = () => (
    <StatisticsContentBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </StatisticsContentBoxStyled>
)

const OverviewStatisticsError = () => (
    <StatisticsContentBoxStyled>
        <ErrorLarge />
    </StatisticsContentBoxStyled>
)

const OverviewStatistics = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectShortUserInfo);
    const statitics = useAppSelector(selectTutorPublicStatistics);
    const privateStatistics = useAppSelector(selectTutorPrivateStatistics);
    const statisticsStatus = useAppSelector(selectTutorPublicStatisticsStatus);
    const privateStatisticsStatus = useAppSelector(selectTutorPrivateStatisticsStatus);
    const userWalletStatistics = useAppSelector(selectUserWalletStatistics);
    const userWalletStatisticsStatus = useAppSelector(selectUserWalletStatisticsStatus);
    const status = getLoadingStatus([statisticsStatus, privateStatisticsStatus, userWalletStatisticsStatus]);

    useEffect(() => {
        dispatch(setUserWalletStatistics());
        dispatch(setTutorPublicStatistics(userInfo?.userId ?? 0));
        dispatch(setTutorPrivateStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STATISTICS)}</H4Styled>
            {status === LoadingStatusEnum.Loading ? <OverviewStatisticsLoading /> : <></>}
            {status === LoadingStatusEnum.Error ? <OverviewStatisticsError /> : <></>}
            {status === LoadingStatusEnum.Loaded && statitics && privateStatistics && userWalletStatistics ? (
                <OverviewStatisticsLoaded statistics={{ ...statitics, ...privateStatistics, ...userWalletStatistics }} />
            ) : <></>}
        </DashboardBoxStyled>
    )
}

const StudentsStatisticsLoaded = ({ statistics }: { statistics: TutorPublicPrivateStatisticsType }) => {
    const { t } = useTranslation();

    return (
        <StatisticsContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STUDENTS)}</Body1Styled>
                    <H3Styled>{statistics.numberOfStudents ?? 0}</H3Styled>
                </StatisticsColumnBoxStyled>
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.NEW_STUDENTS)}
                    valueText={statistics.numberOfNewStudentsThisMonth ?? 0}
                    currentValue={statistics.numberOfNewStudentsThisMonth}
                    previousValue={statistics.numberOfNewStudentsPreviousMonth}
                />
            </StatisticsColumnsBoxStyled>
        </StatisticsContentBoxStyled>
    )
}

const StudentStatisticsError = () => (
    <StatisticsContentBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </StatisticsContentBoxStyled>
)

const StudentStatisticsLoading = () => (
    <StatisticsContentBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </StatisticsContentBoxStyled>
)

const StudentStatitics = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectShortUserInfo);
    const statitics = useAppSelector(selectTutorPublicStatistics);
    const privateStatistics = useAppSelector(selectTutorPrivateStatistics);
    const statisticsStatus = useAppSelector(selectTutorPublicStatisticsStatus);
    const privateStatisticsStatus = useAppSelector(selectTutorPrivateStatisticsStatus);
    const status = getLoadingStatus([statisticsStatus, privateStatisticsStatus]);

    useEffect(() => {
        dispatch(setTutorPublicStatistics(userInfo?.userId ?? 0));
        dispatch(setTutorPrivateStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STATISTICS)}</H4Styled>
            {status === LoadingStatusEnum.Error ? <StudentStatisticsError /> : <></>}
            {status === LoadingStatusEnum.Loading ? <StudentStatisticsLoading /> : <></>}
            {status === LoadingStatusEnum.Loaded && statitics && privateStatistics ? <StudentsStatisticsLoaded statistics={{ ...statitics, ...privateStatistics }} /> : <></>}
        </DashboardBoxStyled>
    )
}

const ReviewsStatistics = () => {
    const { t } = useTranslation();
    const { userInfo } = useInternalPageContext();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STATISTICS)}</H4Styled>
            <StatisticsContentBoxStyled>
                <StatisticsColumnsBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.YOUR_RATING)}</Body1Styled>
                        <Box sx={{ display: "flex", gap: "4px" }}>
                            <IconStyledBoxPrimary><StarIconFilled /></IconStyledBoxPrimary>
                            <H3Styled>{userInfo.rating ?? 5}</H3Styled>
                        </Box>
                    </StatisticsColumnBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.NUMBER_OF_REVIEWS)}</Body1Styled>
                        <H3Styled>{userInfo.reviews?.length ?? 0}</H3Styled>
                    </StatisticsColumnBoxStyled>
                </StatisticsColumnsBoxStyled>
            </StatisticsContentBoxStyled>
        </DashboardBoxStyled>
    )
}

const TransactionsTab = () => (
    <DashboardBoxStyled>
        <Transactions />
    </DashboardBoxStyled>
)

const TransactionsStatisticsLoaded = ({ statistics }: { statistics: GetUserWalletStatisticsResponse }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <StatisticsContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.REVENUE_THIS_MONTH)}
                    valueText={localize({ price: statistics.revenuePreviousMonth })}
                    currentValue={statistics.revenueThisMonth}
                    previousValue={statistics.revenuePreviousMonth}
                />
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.TUTOR_DASHBOARD.REVENUE_THIS_YEAR)}
                    valueText={localize({ price: statistics.revenuePreviousYear })}
                    currentValue={statistics.revenueThisYear}
                    previousValue={statistics.revenuePreviousYear}
                />
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WITHDRAW_THIS_MONTH)}</Body1Styled>
                    <H3Styled>{localize({ price: Math.abs(statistics.withdrawThisMonth ?? 0) })}</H3Styled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WITHDRAW_THIS_YEAR)}</Body1Styled>
                    <H3Styled>{localize({ price: Math.abs(statistics.withdrawThisYear ?? 0) })}</H3Styled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.TOTAL_REVENUE)}</Body1Styled>
                    <H3Styled>{localize({ price: statistics.totalRevenue })}</H3Styled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
        </StatisticsContentBoxStyled>
    )
}

const TransactionsStatistics = () => {
    const userWalletStatistics = useAppSelector(selectUserWalletStatistics);
    const userWalletStatisticsStatus = useAppSelector(selectUserWalletStatisticsStatus);
    const dispatch = useAppDispatch();
    const { t } = useTranslation();

    useEffect(() => {
        dispatch(setUserWalletStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STATISTICS)}</H4Styled>
            {userWalletStatisticsStatus === LoadingStatusEnum.Loading ? <OverviewStatisticsLoading /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Error ? <OverviewStatisticsError /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics ? <TransactionsStatisticsLoaded statistics={userWalletStatistics} /> : <></>}
        </DashboardBoxStyled>
    )
}

const YourStudents = () => {
    const { t } = useTranslation();
    const users = useAppSelector(selectMyStudents);
    const usersStatus = useAppSelector(selectMyStudentsStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setStudents());
    }, []);

    const EMPTY_CONTENT = {
        titleKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.NO_RESULTS_STUDENTS.TITLE,
        descriptionKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.NO_RESULTS_STUDENTS.DESCRIPTION,
        buttonTextKey: "",
        imagePath: "/images/not_found.png"
    }

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STUDENTS)}</H4Styled>
            <DashboardUsers users={users} usersStatus={usersStatus} content={EMPTY_CONTENT} />
        </DashboardBoxStyled>
    )
}

const CalendarTab = () => {
    const { t } = useTranslation();
    return (
        <DashboardBoxStyled sx={{ height: "100%", overflow: "auto" }}>
            <ContainerStyled>
                <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.TITLE)}</H4Styled>
            </ContainerStyled>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflow: "auto" }}>
                <Calendar />
            </Box>
        </DashboardBoxStyled>
    )
}

const OverviewTab = () => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    return (
        <ContainerStyled>
            <DashboardBoxStyled>
                <Box sx={{ display: "flex", flexFlow: { xs: "column noWrap", md: "row noWrap" }, gap: OUTER_GAP }}>
                    <WalletBox />
                    <OverviewStatistics />
                </Box>
                {screenType === "desktop" ? (
                    <DashboardBoxStyled>
                        <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.CALENDAR.TITLE)}</H4Styled>
                        <Calendar />
                    </DashboardBoxStyled>
                ) : <></>}
            </DashboardBoxStyled>
        </ContainerStyled>
    )
}

const StudentsTab = () => {

    return (
        <ContainerStyled>
            <TabContentBoxStyled>
                <StudentStatitics />
                <YourStudents />
            </TabContentBoxStyled>
        </ContainerStyled>
    )
}

const PastLessonsTab = () => {

    const { t } = useTranslation();

    return (
        <ContainerStyled>
            <TabContentBoxStyled>
                <DashboardBoxStyled>
                    <H4Styled>{t(TRANSLATION_KEYS.PROFILE.RESERVATIONS.PAST_EVENTS_TITLE)}</H4Styled>
                    <Reservations time={ReservationTimeEnum.Past} translationKeys={PastTranslationKeys} />
                </DashboardBoxStyled>
            </TabContentBoxStyled>
        </ContainerStyled>
    )
}
const ReviewsTab = () => {
    const { t } = useTranslation();
    const { userInfo } = useInternalPageContext();
    const usersStatus = useAppSelector(selectUserInfoStatus);

    const EMPTY_CONTENT = {
        titleKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.NO_RESULTS_REVIEWS.TITLE,
        descriptionKey: TRANSLATION_KEYS.TUTOR_DASHBOARD.NO_RESULTS_REVIEWS.DESCRIPTION,
        buttonTextKey: "",
        imagePath: "/images/no_reviews.png"
    }

    return (
        <ContainerStyled>
            <TabContentBoxStyled>
                <ReviewsStatistics />
                <DashboardBoxStyled>
                    <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.REVIEWS)}</H4Styled>
                    {usersStatus === LoadingStatusEnum.Loaded && userInfo && !userInfo.reviews?.length ? <ReviewsBox><Empty content={EMPTY_CONTENT} /></ReviewsBox> : <></>}
                    {usersStatus === LoadingStatusEnum.Loaded && userInfo && userInfo.reviews && userInfo.reviews.length ? <ReviewsBox> {userInfo.reviews?.map((x, idx) => <UserReview userReview={x} key={idx} />)}</ReviewsBox> : <></>}
                </DashboardBoxStyled>
            </TabContentBoxStyled>
        </ContainerStyled>
    )
}

const TransactionTab = () => (
    <ContainerStyled>
        <TabContentBoxStyled>
            <TransactionsStatistics />
            <TransactionsTab />
        </TabContentBoxStyled>
    </ContainerStyled>
)

const Navigation = ({ }) => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);
    const { currentTab, setCurrentTab } = useInternalPageContext();

    return (
        <TabsBoxStyled>
            {getTabs(screenType)
                .map((x, idx) => (
                    <TabItemButtonStyled
                        key={idx}
                        onClick={() => setCurrentTab(x.type)}
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

const TutorDashboardViews = () => {
    const { currentTab } = useInternalPageContext();

    return (
        <>
            {currentTab === "overview" ? <OverviewTab /> : <></>}
            {currentTab === "calendar" ? <CalendarTab /> : <></>}
            {currentTab === "students" ? <StudentsTab /> : <></>}
            {currentTab === "past_lessons" ? <PastLessonsTab /> : <></>}
            {currentTab === "reviews" ? <ReviewsTab /> : <></>}
            {currentTab === "transactions" ? <TransactionTab /> : <></>}
        </>
    )
}

const TutorDashboard = () => {
    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", height: "100%", overflow: "auto" }}>
            <Box sx={{ display: "flex", width: "100%", flex: "0 1 auto" }}>
                <Navbar />
            </Box>
            <BackgroundBoxStyled>
                <ContainerStyled sx={{ display: "flex", flexFlow: "column noWrap", flex: "0 1 auto", gap: INNER_GAP }}>
                    <WelcomeBackUser />
                    <ActiveSessionOptional />
                    <Navigation />
                </ContainerStyled>
                <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflowY: "auto" }}>
                    <TutorDashboardViews />
                </Box>
            </BackgroundBoxStyled>
        </Box>
    )
};

type TutorDashboardTabPathType = { tab: string };
export type TutorDashboardTabPathParsedType = { tab: TutorDashboardTabPathType };
const TutorDashboardWithContext = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const [currentTab, setTab] = useState<TutorDashboardTabType>("overview");
    const router = useRouter();

    useEffect(() => {
        const query = router.query as TutorDashboardTabPathType;
        setTab(getTabType(query.tab));
    }, [router.query, router.asPath]);

    const getTabType = (query: string): TutorDashboardTabType => {
        switch (query) {
            case "calendar":
                return "calendar";
            case "overview":
                return "overview";
            case "students":
                return "students";
            case "past_lessons":
                return "past_lessons";
            case "reviews":
                return "reviews";
            case "transactions":
                return "transactions"
            default:
                return "overview";
        }
    }

    const handleSetTab = (newTab: TutorDashboardTabType) => {
        var query: TutorDashboardTabPathType = { tab: newTab };
        router.replace({
            pathname: routes.tutorDashboard.path,
            query
        }, undefined, { shallow: true })
        setTab(newTab)
    }

    return (
        <TutorDashboardInternalContext.Provider value={{
            userInfo,
            currentTab,
            setCurrentTab: handleSetTab
        }}>
            <TutorDashboard />
        </TutorDashboardInternalContext.Provider>
    )
}


const RedirectHandler = () => {
    const userInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        if (!userInfo) return;
        if (userInfo.stateId !== UserStateEnum.Confirmed) {
            location.replace(routes.tutorRegistrationForm.path);
        }
    }, [userInfo]);

    return (
        <></>
    )
}

const TutorDashboardPage = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectUserInfo);
    const userInfoStatus = useAppSelector(selectUserInfoStatus);

    useEffect(() => {
        dispatch(setUserInfo());
    }, []);

    return (
        <>
            <RedirectHandler />
            {userInfoStatus === LoadingStatusEnum.Error ? <ErrorPage /> : <></>}
            {userInfoStatus === LoadingStatusEnum.Loading ? <LinearProgressStyled /> : <></>}
            {userInfoStatus === LoadingStatusEnum.Loaded && userInfo ? <TutorDashboardWithContext userInfo={userInfo} /> : <></>}
        </>
    )
}

export default withAppClientSideProvider(TutorDashboardPage);
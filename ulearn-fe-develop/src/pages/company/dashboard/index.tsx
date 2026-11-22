import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { UIContext, ViewScreenType } from "components/core/UIContextProvider";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Wallet from "components/dashboard/Wallet";
import WelcomeBackUser from "components/dashboard/WelcomeBackUser";
import Navbar from "components/navbar/Navbar";
import ErrorPage from "components/pages/ErrorPage";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import { DashboardBoxStyled } from "components/styles/boxes/DashboardBoxStyled";
import LinearProgressStyled from "components/styles/boxes/LinearProgressStyled";
import { TabsBoxStyled } from "components/styles/boxes/TabsBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H4Styled from "components/styles/typography/H4Styled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { FilterEmployeesResponseBody, UserInfoResponse, selectEmployeesFilter, selectEmployeesFilterStatus, selectUserInfo, selectUserInfoStatus, setEmployeesFilter, setUserInfo } from "core/redux/userSlice";
import { useRouter } from "next/router";
import React, { createContext, useContext, useEffect } from "react";
import { useState } from "react";
import routes from "routes/routes";
import Transactions from "components/dashboard/Transactions";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import ErrorLarge from "components/core/error/ErrorLarge";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";
import StatisticsColumnsBoxStyled from "components/styles/boxes/StatisticsColumnsBoxStyled";
import StatisticsColumnBoxStyled from "components/styles/boxes/StatisticsColumnBoxStyled";
import StatisticsContentBoxStyled from "components/styles/boxes/StatisticsContentBoxStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import InviteIcon from "assets/invite_people.svg";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import H5Styled from "components/styles/typography/H5Styled";
import { TableHead } from "@mui/material";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import ThreeDotsMenu from "components/menu/ThreeDotsMenu";
import MenuItem from "components/menu/MenuItem";
import SendMoneyIcon from "assets/arrow-right.svg";
import WithdrawMoneyIcon from "assets/arrow-left.svg";
import DeleteAccountIcon from "assets/cancel_lesson.svg";
import PencilSimpleIcon from "assets/pencil-simple.svg";
import SendMoneySingleModalProvider, { useSendMoneySingleModalContext } from "components/modals/SendMoneySingleModalProvider";
import { SecondaryButton } from "components/styles/SecondaryButton";
import SendMoneyMultiModalProvider, { useSendMoneyMultiModalContext } from "components/modals/SendMoneyMultiModalProvider";
import WithdrawMoneyStudentModalProvider, { useWithdrawMoneyStudentModalContext } from "components/modals/WithdrawMoneyStudentModalProvider";
import InvitePeopleModalProvider, { useInvitePeopleModalContext } from "components/modals/InvitePeopleModalProvider";
import DeleteEmployeeModalProvider, { useDeleteStudentModalContext } from "components/modals/DeleteEmployeeModalProvider";
import { useCompanyRegistrationModal, withCompanyRegistrationModalProvider } from "components/modals/CompanyRegistrationModalProvider";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import UserBoxCell from "components/dashboard/UserBoxCell";
import moment from "moment";
import PStyled from "components/styles/typography/PStyled";
import EmptyPeopleIcon from "assets/empty_people.svg";
import { GetCompanyPrivateStatisticsResponse, selectCompanyPrivateStatistics, selectCompanyPrivateStatisticsStatus, setCompanyPrivateStatistics } from "core/redux/userStatisticsSlice";
import StatisticWithBadge from "components/dashboard/StatisticWithBadge";
import { ContainerStyled } from "components/styles/ContainerStyled";
import EditEmployeeModalProvider, { useEditEmployeeModalContext } from "components/modals/EditEmployeeModalProvider";
import { usePriceLocalization } from "core/hooks/priceHook";

const OUTER_GAP = { xs: "16px", md: "24px" }
const INNER_GAP = { xs: "8px", md: "16px" };
type ScroreFulfilmentType = "not_yet" | "in_progress" | "fulfilled";
const BackgroundBoxStyled = styled(Box)(() => sx({
    paddingTop: { xs: "16px", md: "24px" },
    display: "flex",
    flex: "1 1 auto",
    gap: OUTER_GAP,
    flexDirection: "column",
    flexFlow: "column noWrap",
    overflow: "auto"
})) as typeof Box;

const HeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center"
})) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButton)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    padding: "8px 16px",
    flexFlow: "row noWrap"
})) as typeof PrimaryButton;

const SecondaryButtonStyled = styled(SecondaryButton)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    padding: "8px 16px",
    flexFlow: "row noWrap"
})) as typeof SecondaryButton;

const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        padding: "16px 0px",
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
        padding: "12px 0px"
    },
}));

const StyledTableCellWithPadding = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        padding: "12px 8px",
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
        padding: "12px 8px",
        whiteSpace: "noWrap"
    },
}));

const TableBodyStyled = styled(TableBody)({
    padding: "0",
    border: "none"
}) as typeof TableBody;

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
    },
    // hide last border
    '&:last-child td, &:last-child th': {
        border: 0,
    }
}));

const Body1BlackStyled = styled(Body1Styled)({
    color: COLORS.BLACK
}) as typeof Body1Styled;

const PeopleBoxStyled = styled(Box)(() => sx({
    width: "100%",
    borderRadius: "4px",
    display: "flex",
    gap: "24px",
    border: `1px solid ${COLORS.BLACK_10}`,
    flexDirection: "row",
    padding: "0px 16px",
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;

const RedIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.RED
    }
})) as typeof Box;

const HorizontalDividerBoxStyled = styled(Box)({
    width: "100%",
    minHeight: "1px",
    display: "flex",
    flexFlow: "column noWrap",
    backgroundColor: COLORS.BLACK_10
}) as typeof Box;

const EmptyPeopleHeader = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "column noWrap"
}) as typeof Box;

const EmptyPeopleContentStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "24px",
    justifyContent: "center",
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "80px 0px",
    borderRadius: "4px",
    flexFlow: "column noWrap",
    height: "100%",
    alignContent: "center"
}) as typeof Box;


const getFulfillmentColor = (fulfillment: ScroreFulfilmentType) => {
    if (fulfillment === "not_yet") return COLORS.RED;
    if (fulfillment === "in_progress") return COLORS.PRIMARY;
    return COLORS.GREEN;
}

const getFulfillmentBackgroundColor = (fulfillment: ScroreFulfilmentType) => {
    if (fulfillment === "not_yet") return COLORS.RED_10;
    if (fulfillment === "in_progress") return COLORS.PRIMARY_10;
    return COLORS.GREEN_10;
}

const WeeklyProgressBadgeStyled = styled(Box)<{ fulfillment: ScroreFulfilmentType }>(({ fulfillment }) => sx({
    display: "flex",
    width: "auto",
    padding: "2px 8px",
    borderRadius: "26px",
    color: getFulfillmentColor(fulfillment),
    backgroundColor: getFulfillmentBackgroundColor(fulfillment)
}));

const WeeklyGoalRowBox = styled(Box)({
    display: "flex",
    flexFlow: "row wrap",
    gap: "8px",
    alignItems: "center"
}) as typeof Box;

type CompanyDashboardTabType = "overview" | "transactions";

type CompanyDashboardInternalContextType = {
    userInfo: UserInfoResponse,
    currentTab: CompanyDashboardTabType,
    setCurrentTab: (newTab: CompanyDashboardTabType) => void
} | undefined;


const CompanyDashboardInternalContext = createContext<CompanyDashboardInternalContextType | undefined>(undefined);
const useInternalPageContext = contextFactoryCreate('CompanyDashboardInternal', CompanyDashboardInternalContext);

const Tabs: { type: CompanyDashboardTabType, screenTypes: ViewScreenType[], translationKey: string }[] = [
    { type: "overview", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.COMPANY_DASHBOARD.OVERVIEW },
    { type: "transactions", screenTypes: ["desktop", "mobile"], translationKey: TRANSLATION_KEYS.COMPANY_DASHBOARD.TRANSACTIONS }
]

const getTabs = (screenType: ViewScreenType) => Tabs.filter(x => x.screenTypes.some(y => y === screenType));

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

const CompanyDashboardViews = () => {
    const { currentTab } = useInternalPageContext();

    return (
        <>
            {currentTab === "overview" ? <OverviewTab /> : <></>}
            {currentTab === "transactions" ? <TransactionsTab /> : <></>}
        </>
    )
}

const WalletBox = () => {
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WALLET)}</H4Styled>
            <Wallet />
        </DashboardBoxStyled>
    )
}

const PeopleThreeDotsMenu = () => {
    const { t } = useTranslation();
    const { setOpen: setSendMoneySingleOpen } = useSendMoneySingleModalContext();
    const { setOpen: setWithdrawMoneyEmployeeOpen } = useWithdrawMoneyStudentModalContext();
    const { setOpen: setDeleteEmployeeAccountOpen } = useDeleteStudentModalContext();
    const { setOpen: setEditAccountOpen } = useEditEmployeeModalContext();
    return (
        <ThreeDotsMenu
            innerMenu={
                <MenuBoxStyled gap="4px">
                    <MenuItem text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.EDIT_ACCOUNT_BUTTON)} icon={<PencilSimpleIcon />} onClick={() => setEditAccountOpen()} />
                    <MenuItem text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.SEND_MONEY_BUTTON)} icon={<SendMoneyIcon />} onClick={() => setSendMoneySingleOpen()} />
                    <MenuItem text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.WITHDRAW_MONEY_BUTTON)} icon={<WithdrawMoneyIcon />} onClick={() => setWithdrawMoneyEmployeeOpen()} />
                    <HorizontalDividerBoxStyled />
                    <MenuItem text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.DELETE_ACCOUNT_BUTTON)} icon={<RedIconStyled><DeleteAccountIcon /></RedIconStyled>} onClick={() => setDeleteEmployeeAccountOpen()} />
                </MenuBoxStyled>
            }
            autoClose={true}
        />
    )
}
const PeopleThreeDotsMenuWithProviders = ({ user }: { user: FilterEmployeesResponseBody['data'][0] }) => (
    <SendMoneySingleModalProvider user={user}>
        <WithdrawMoneyStudentModalProvider user={user}>
            <DeleteEmployeeModalProvider user={user}>
                <EditEmployeeModalProvider user={user}>
                    <PeopleThreeDotsMenu />
                </EditEmployeeModalProvider>
            </DeleteEmployeeModalProvider>
        </WithdrawMoneyStudentModalProvider>
    </SendMoneySingleModalProvider>
)

const getScore = (stats: { currentValue: number | undefined, goalId: number | undefined }) => {
    const { currentValue, goalId } = stats;
    if (!goalId) return 0;
    if (!currentValue) return 0;
    const result = (currentValue * 100) / goalId;
    return result <= 100 ? result : 100;
}

const getScoreFullfillment = (score: number): ScroreFulfilmentType => {
    if (score === 0) return "not_yet";
    if (score > 0 && score < 100) return "in_progress";
    return "fulfilled";
}

const WeeklyProgressBadge = ({ currentWeeklyGoalProgress, weeklyGoalId }: { currentWeeklyGoalProgress: number | undefined, weeklyGoalId: number | null | undefined }) => {
    const { t } = useTranslation();
    const fulfilment = getScoreFullfillment(getScore({ currentValue: currentWeeklyGoalProgress ?? 0, goalId: weeklyGoalId ?? 0 }));

    return (
        <WeeklyProgressBadgeStyled fulfillment={fulfilment}>
            {fulfilment === "not_yet" ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.WEEKLY_GOAL_STATISTICS.NOT_YET) : ""}
            {fulfilment === "in_progress" ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.WEEKLY_GOAL_STATISTICS.IN_PRORESS) : ""}
            {fulfilment === "fulfilled" ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.WEEKLY_GOAL_STATISTICS.FULLFILED) : ""}
        </WeeklyProgressBadgeStyled>
    )
}

const PeopleTable = ({ filter }: { filter: FilterEmployeesResponseBody }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <PeopleBoxStyled>
            <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                <Table sx={{ minWidth: 650, padding: "0", border: "none" }}>
                    <TableHead>
                        <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.COLUMN_NAME)}</H5Styled></StyledTableCellWithPadding>
                        <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.COLUMN_WALLET_BALANCE)}</H5Styled></StyledTableCellWithPadding>
                        <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.WEEKLY_GOAL)}</H5Styled></StyledTableCellWithPadding>
                        <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.COLUMN_LAST_TOP_UP)}</H5Styled></StyledTableCellWithPadding>
                        <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE_TABLE.COLUMN_LAST_LESSON)}</H5Styled></StyledTableCellWithPadding>
                        <StyledTableCellWithPadding />
                    </TableHead>
                    <TableBodyStyled>
                        {filter.data.map(x => (
                            <StyledTableRow key={x.userId!}>
                                <StyledTableCellWithPadding>
                                    <UserBoxCell user={x} typeKey={x.state?.companyAdminNameKey} />
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding>
                                    <H5Styled>{localize({ price: x.wallet?.amount ?? 0 })}</H5Styled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding>
                                    <WeeklyGoalRowBox>
                                        <H5Styled>{t(TRANSLATION_KEYS.COMMON.PROGRESS, { currentValue: x.currentWeeklyGoalProgress ?? 0, maxValue: x.weeklyGoalId ?? 0 })}</H5Styled>
                                        <WeeklyProgressBadge currentWeeklyGoalProgress={x.currentWeeklyGoalProgress} weeklyGoalId={x.weeklyGoalId} />
                                    </WeeklyGoalRowBox>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding>
                                    <Body1BlackStyled>
                                        {x.wallet?.lastMoneyTransferDate ? moment(x.wallet?.lastMoneyTransferDate).format("yyyy-MM-DD HH:mm") : ""}
                                    </Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding>
                                    <Body1BlackStyled>
                                        {x.lessonStatistics
                                            ? `${moment(x.lessonStatistics.lastLessonFromDate).format("yyyy-MM-dd HH:mm")} - ${moment(x.lessonStatistics.lastLessonToDate).format("HH:mm")}`
                                            : ""}
                                    </Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCell align="right">
                                    <PeopleThreeDotsMenuWithProviders user={x} />
                                </StyledTableCell>
                            </StyledTableRow>
                        ))}
                    </TableBodyStyled>
                </Table>
            </TableContainer>
        </PeopleBoxStyled>
    )
}

const PeopleLoading = () => (
    <PeopleBoxStyled sx={{ alignItems: "center", justifyContent: "center", minHeight: "60px" }}>
        <CircularProgressCustom />
    </PeopleBoxStyled>
)

const PeopleError = () => (
    <PeopleBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </PeopleBoxStyled>
)

const PeopleTableWithHeader = ({ filter }: { filter: FilterEmployeesResponseBody }) => {
    const { t } = useTranslation();
    const { setOpen: setSendMoneyMultiOpen } = useSendMoneyMultiModalContext();
    const { setOpen: setInvitePeopleOpen } = useInvitePeopleModalContext();

    return (
        <DashboardBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE, { count: filter.size ?? 0 })}</H4Styled>
                <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "8px" }}>
                    {filter.size ? (
                        <SecondaryButtonStyled onClick={setSendMoneyMultiOpen}>
                            <SendMoneyIcon />
                            {t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.SEND_MONEY_TO_ALL_BUTTON)}
                        </SecondaryButtonStyled>
                    ) : <></>}
                    <PrimaryButtonStyled onClick={setInvitePeopleOpen}>
                        <InviteIcon />
                        {t(TRANSLATION_KEYS.COMPANY_DASHBOARD.EMPLOYEES_TABLE.INVITE_PEOPLE)}
                    </PrimaryButtonStyled>
                </Box>
            </HeaderBoxStyled>
            <PeopleTable filter={filter} />
        </DashboardBoxStyled>
    )
}
const CompanyPeopleTableWithHeaderWithProviders = ({ filter }: { filter: FilterEmployeesResponseBody }) => (
    <SendMoneyMultiModalProvider filter={filter}>
        <InvitePeopleModalProvider>
            <PeopleTableWithHeader filter={filter} />
        </InvitePeopleModalProvider>
    </SendMoneyMultiModalProvider>
)

const EmptyPeople = () => {
    const { t } = useTranslation();
    const { setOpen: setInvitePeopleOpen } = useInvitePeopleModalContext();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.PEOPLE, { count: 0 })}</H4Styled>
            <EmptyPeopleContentStyled>
                <EmptyPeopleHeader>
                    <H4Styled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.EMPTY_PEOPLE.TITLE)}</H4Styled>
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.EMPTY_PEOPLE.DESCRIPTION)}</PStyled>
                    <PrimaryButton sx={{ gap: "4px" }} onClick={setInvitePeopleOpen}>{t(TRANSLATION_KEYS.EMPTY_PEOPLE.INVITE_PEOPLE)} <InviteIcon /></PrimaryButton>
                </EmptyPeopleHeader>
                <EmptyPeopleIcon />
            </EmptyPeopleContentStyled>
        </DashboardBoxStyled>
    )
}

const EmptyPeopleWithProviders = () => (
    <InvitePeopleModalProvider>
        <EmptyPeople />
    </InvitePeopleModalProvider>
)

const CompanyPeople = () => {
    const dispatch = useAppDispatch();
    const filterStatus = useAppSelector(selectEmployeesFilterStatus);
    const filter = useAppSelector(selectEmployeesFilter);

    useEffect(() => {
        dispatch(setEmployeesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                skip: 0,
                take: 200
            }
        }))
    }, []);

    return (
        <>
            {filterStatus === LoadingStatusEnum.Error ? <PeopleError /> : <></>}
            {filterStatus === LoadingStatusEnum.Loading ? <PeopleLoading /> : <></>}
            {filterStatus === LoadingStatusEnum.Loaded && filter?.size ? <CompanyPeopleTableWithHeaderWithProviders filter={filter} /> : <></>}
            {filterStatus === LoadingStatusEnum.Loaded && !filter?.size ? <EmptyPeopleWithProviders /> : <></>}
        </>
    )
}

const OverviewStatisticsLoaded = ({ statistics }: { statistics: GetCompanyPrivateStatisticsResponse }) => {
    const { t } = useTranslation();

    return (
        <StatisticsContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.STATISTICS.NUMBER_OF_UPCOMING_LESSONS_THIS_MONTH_TITLE)}
                    valueText={statistics.numberOfLessonsThisMonth ?? 0}
                    currentValue={statistics.numberOfLessonsThisMonth}
                    previousValue={statistics.numberOfLessonPreviousMonth}
                />
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.STATISTICS.NUMBER_OF_COMPLETED_LESSONS_TITLE)}</Body1Styled>
                    <H3Styled>{statistics.numberOfCompletedLessons ?? 0}</H3Styled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticWithBadge
                    titleText={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.STATISTICS.EMPLOYEES_WEEKLY_GOALS_TITLE)}
                    valueText={t(TRANSLATION_KEYS.COMMON.PROGRESS, { currentValue: statistics.employeesWeeklyGoalThisWeek ?? 0, maxValue: statistics.numberOfEmployees ?? 0 })}
                    currentValue={statistics.employeesWeeklyGoalThisWeek}
                    previousValue={statistics.employeesWeeklyGoalPreviousWeek}
                />
            </StatisticsColumnsBoxStyled>
        </StatisticsContentBoxStyled>
    )
}

const OverviewStatisticsError = () => (
    <StatisticsContentBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <ErrorLarge />
    </StatisticsContentBoxStyled>
)

const OverviewStatisticsLoading = () => (
    <StatisticsContentBoxStyled sx={{ alignItems: "center", justifyContent: "center" }}>
        <CircularProgressCustom />
    </StatisticsContentBoxStyled>
)

const OverviewStatistics = () => {
    const dispatch = useAppDispatch();
    const { t } = useTranslation();
    const companyStatisticsStatus = useAppSelector(selectCompanyPrivateStatisticsStatus);
    const comapanyStatistics = useAppSelector(selectCompanyPrivateStatistics);

    useEffect(() => {
        dispatch(setCompanyPrivateStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.STATISTICS)}</H4Styled>
            {companyStatisticsStatus === LoadingStatusEnum.Error ? <OverviewStatisticsError /> : <></>}
            {companyStatisticsStatus === LoadingStatusEnum.Loading ? <OverviewStatisticsLoading /> : <></>}
            {companyStatisticsStatus === LoadingStatusEnum.Loaded && comapanyStatistics ? <OverviewStatisticsLoaded statistics={comapanyStatistics} /> : <></>}
        </DashboardBoxStyled>
    )
}

const OverviewTab = () => {

    return (
        <ContainerStyled>
            <DashboardBoxStyled>
                <Box sx={{ display: "flex", flexFlow: { xs: "column noWrap", md: "row noWrap" }, gap: OUTER_GAP }}>
                    <WalletBox />
                    <OverviewStatistics />
                </Box>
                <CompanyPeople />
            </DashboardBoxStyled>
        </ContainerStyled>
    )
}

const TransactionsTab = () => {

    return (
        <DashboardBoxStyled>
            <ContainerStyled>
                <Transactions />
            </ContainerStyled>
        </DashboardBoxStyled>
    )
}

const CompanyDashboard = () => {
    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", height: "100%", overflow: "auto" }}>
            <Box sx={{ display: "flex", width: "100%", flex: "0 1 auto" }}>
                <Navbar />
            </Box>
            <BackgroundBoxStyled>
                <ContainerStyled sx={{ display: "flex", flexFlow: "column noWrap", flex: "0 1 auto", gap: INNER_GAP }}>
                    <WelcomeBackUser />
                    <Navigation />
                </ContainerStyled>
                <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", overflowY: "auto" }}>
                    <CompanyDashboardViews />
                </Box>
            </BackgroundBoxStyled>
        </Box>
    )
};

type CompanyDashboardTabPathType = { tab: string };
export type CompanyDashboardTabPathParsedType = { tab: CompanyDashboardTabPathType };

const CompanyDashboardWithContext = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const [currentTab, setTab] = useState<CompanyDashboardTabType>("overview");
    const router = useRouter();

    useEffect(() => {
        const query = router.query as CompanyDashboardTabPathType;
        setTab(getTabType(query.tab));
    }, [router.query, router.asPath]);

    const getTabType = (query: string): CompanyDashboardTabType => {
        switch (query) {
            case "overview":
                return "overview";
            case "transactions":
                return "transactions"
            default:
                return "overview";
        }
    }

    const handleSetTab = (newTab: CompanyDashboardTabType) => {
        var query: CompanyDashboardTabPathType = { tab: newTab };
        router.replace({
            pathname: routes.companyDashboard.path,
            query
        }, undefined, { shallow: true })
        setTab(newTab)
    }

    return (
        <CompanyDashboardInternalContext.Provider value={{
            userInfo,
            currentTab,
            setCurrentTab: handleSetTab
        }}>
            <CompanyDashboard />
        </CompanyDashboardInternalContext.Provider>
    )
}

const ModalHandler = ({ userInfo }: { userInfo: UserInfoResponse }) => {
    const { setOpen } = useCompanyRegistrationModal();

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

const CompanyDashboardPage = () => {
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
            {userInfo ? <CompanyDashboardWithContext userInfo={userInfo} /> : <></>}
            {/* Move registration handler outside of the component to prevent modal reopen on the last step */}
            {userInfo ? <ModalHandler userInfo={userInfo} /> : <></>}
        </>
    )
}

export default withAppClientSideProvider(withCompanyRegistrationModalProvider(CompanyDashboardPage));
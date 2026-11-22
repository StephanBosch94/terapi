import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TabsBoxStyled } from "components/styles/boxes/TabsBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TabItemButtonStyled } from "components/styles/TabItemButtonStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectFilteredTutors, setPublicTutorInfo, setTutorsFilter, setStudentsFilter, selectFilteredStudents, approveUser, selectApproveUserStatus, FilterTutorsResponseBody, refuseUser, selectRefuseUserStatus } from "core/redux/userSlice";
import { useTranslation } from "components/core/TranslationProvider";
import React, { useEffect, useState, useRef, createContext, } from "react";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { selectShortUserInfo } from "core/redux/userSlice";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import Body1Styled from "components/styles/typography/Body1Styled";
import H4Styled from "components/styles/typography/H4Styled";
import H5Styled from "components/styles/typography/H5Styled";
import { getProfilePhotoThumbnailUrl, getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { getUserName, getUserRating } from "core/services/Utils/userUtils";
import ChatIcon from "assets/chat.svg";
import { IconButton } from "components/styles/IconButton";
import ThreeDotsIcon from "assets/3_dots.svg";
import { MenuStyled } from "components/styles/MenuStyled";
import DeleteIcon from "assets/delete.svg";
import ApproveIcon from "assets/approve.svg";
import RefuseIcon from "assets/close.svg";
import H3Styled from "components/styles/typography/H3Styled";;
import { DashboardBoxStyled } from "components/styles/boxes/DashboardBoxStyled";
import StarIcon from "assets/star-filled.svg";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { SecondaryButtonCompacted } from "components/styles/SecondaryButtonCompacted";
import PStyled from "components/styles/typography/PStyled";
import StarEmpty from "assets/star.svg";
import StarFilled from "assets/star-filled.svg";
import MenuItem from "components/menu/MenuItem";
import CheckCircleIcon from "assets/check-circle.svg";
import XCircleIcon from "assets/x-circle.svg";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import { UserStateEnum } from "core/constants/enums/UserStateEnum";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import routes from "routes/routes";
import { useSnackbar } from "notistack";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Divider from "@mui/material/Divider";
import Link from "next/link";
import Transactions from "components/dashboard/Transactions";
import { GetUserWalletStatisticsResponse, selectUserWalletStatistics, selectUserWalletStatisticsStatus, setUserWalletStatistics } from "core/redux/userWalletSlice";
import TableRow from "@mui/material/TableRow";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import ContactUserModalProvider, { useContactUserModalContext } from "components/modals/ContactUserModalProvider";
import TableHead from "@mui/material/TableHead";
import { selectCompleteTransactionStatus, selectWithdrawTransactionFilter, setCompleteTrasnsatction, setWithdrawFilter } from "core/redux/transactionSlice";
import { TransactionStateEnum } from "core/constants/enums/TransactionStateEnum";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import moment from "moment";
import UserBoxCell from "components/dashboard/UserBoxCell";
import { snackbarConstants } from "components/core/Snackbar";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ReservationIssueFilterResponse, approveReservationIssue, declineReservationIssue, selectReservationIssueApproveStatus, selectReservationIssueDeclineStatus, selectReservationIssueFilter, selectReservationIssueFilterStatus, setReservationIssuesFilter } from "core/redux/reservationIssueSlice";
import { ReservationIssueStateEnum } from "core/constants/enums/ReservationIssueStateEnum";
import ThreeDotsMenu from "components/menu/ThreeDotsMenu";
import { ModalStyled } from "components/styles/ModalStyled";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import ModalsHeader from "components/modals/common/ModalsHeader";
import { AvatarStyled } from "components/styles/user/AvatarStyled";
import CircleProgressStyled from "components/styles/CircularProgressStyled";
import { TextareaBoxStyled } from "components/styles/boxes/TextareaBoxStyled";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import StatisticsColumnBoxStyled from "components/styles/boxes/StatisticsColumnBoxStyled";
import StatisticsColumnsBoxStyled from "components/styles/boxes/StatisticsColumnsBoxStyled";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { usePriceLocalization } from "core/hooks/priceHook";

const BackgroundBoxStyled = styled(Box)(() => sx({
    py: { xs: "24px", md: "32px" }
})) as typeof Box;

const AvatarStyledSmall = styled(Avatar)(() => sx({
    width: "40px",
    height: "40px"
})) as typeof Avatar;

const HeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
})) as typeof Box;

const CellButtonBoxStyled = styled(Box)(() => sx({
    width: "fit-content",
    padding: "4px 8px",
    borderRadius: "4px",
    ":hover": { backgroundColor: COLORS.PRIMARY_10, cursor: "pointer" }
})) as typeof Box;

const UserBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    minWidth: "220px",
    gap: "16px",
})) as typeof Box;

const ActionBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    justifyContent: "end",
    padding: "0px 16px"
})) as typeof Box;

const IconStyledBoxWhite = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.WHITE
    }
})) as typeof Box;

const UserOuterBoxStyled = styled(Box)(() => sx({
    userSelect: "false",
    display: 'flex',
    alignItems: 'center',
    textAlign: 'center'
})) as typeof Box;

const MainBoxStyled = styled(Box)(() => sx({
    userSelect: "false"
})) as typeof Box;

const IconDotsStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.5)",
    }
})) as typeof Box;

const StarIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.25)"
    }
})) as typeof Box;

const ContentBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "16px",
    display: "flex",
    flexDirection: "column",
    gap: "16px",
    borderRadius: "4px",
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
})) as typeof Box;

const ContentTableBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "0px 16px",
    display: "flex",
    flexDirection: "column",
    gap: "16px",
    borderRadius: "4px",
    overflowX: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
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

const StarBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "8px",
    flexFlow: "row noWrap",
    gap: "8px",
    svg: { color: COLORS.PRIMARY }
})) as typeof Box;

const TabContentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "32px",
    flexDirection: { xs: "column", md: "row" }
})) as typeof Box;

const DashboardRowBox = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: "flexStart",
    gap: "32px",
    width: "100%",
})) as typeof Box;

const RowBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "start",
    justifyContent: "center",
    flexDirection: "column",
    gap: "8px"
})) as typeof Box;

const ReviewNameBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
    flexDirection: "row",
    alignItems: "center"
})) as typeof Box;

const ColumnTableBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "4px"
})) as typeof Box;

const ColumnsTableBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "16px"
})) as typeof Box;

const OverviewItemStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    width: "100%",
    minWidth: "400px",
    gap: "32px",
    alignItems: "center",
    justifyContent: "space-between"
})) as typeof Box;

const StarAndTextBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    gap: "4px"
})) as typeof Box;

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "4px"
})) as typeof Box;

const StudentTutorBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "8px",
    alignItems: "center",
    width: "100%"
})) as typeof Box;

const TransactionHeaderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    marginBottom: "16px",
    justifyContent: "space-between"
})) as typeof Box;

const LinkItem = styled(Box)(() => sx({
    "a": {
        color: COLORS.BLACK,
        textDecoration: "none",
        ":hover": {
            textDecoration: "underline"
        }
    }
})) as typeof Box;

const TabBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "16px",
    flexFlow: "column noWrap",
    width: "100%"
})) as typeof Box;

const ModalUserBoxStyled = styled(Box)({
    display: "inline-flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "12px"
}) as typeof Box;

const UserAvatarBoxStyled = styled(AvatarStyled)({
    width: "40px",
    height: "40px"
}) as typeof AvatarStyled;

const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        padding: "16px 0px",
        minWidth: "200px"
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
        padding: "12px 0px",
        minWidth: "200px"
    },
}));

const StyledTableCellWithPadding = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        padding: "12px 8px",
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
        padding: "16px 8px",
    },
}));

const Body1BlackStyled = styled(Body1Styled)({
    color: COLORS.BLACK
}) as typeof Body1Styled;

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
    },
    // hide last border
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}));

const TableBodyStyled = styled(TableBody)({
    padding: "0",
    border: "none"
}) as typeof TableBody;

const LabelBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "4px"
}) as typeof Box;

const AttributeLabelTypographyStyled = styled(Body1Styled)({
    fontWeight: "600",
    color: COLORS.BLACK
});


const Students = () => {
    const { t } = useTranslation();
    const students = useAppSelector(selectFilteredStudents);
    const dispatch = useAppDispatch();


    useEffect(() => {
        dispatch(setStudentsFilter({
            request: {
                filter: {},
                skip: 0,
                take: 20
            },
            appendType: AppendTypeEnum.None
        }));
    }, []);

    return (
        <TabBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.STUDENTS)}</H4Styled>
            </HeaderBoxStyled>
            <ContentTableBoxStyled>
                <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                    <Table sx={{ minWidth: 650, padding: "0", border: "none" }} aria-label="simple table">
                        <TableHead>
                            <StyledTableCell><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.NAME)}</H5Styled></StyledTableCell>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.LANGUAGE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.LAST_ACTIVITY)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.NUMBER_OF_LESSONS)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding />
                        </TableHead>
                        <TableBodyStyled>
                            {students?.data.map((row) => (
                                <StyledTableRow key={row.subjectId!} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                    <StyledTableCell component="th" scope="row">
                                        <UserBoxStyled>
                                            <AvatarStyledSmall src={getProfilePhotoThumbnailUrl(row)} />
                                            <H5Styled>{getUserName(row)}</H5Styled>
                                        </UserBoxStyled>
                                    </StyledTableCell>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>English</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>22.05.2022 15:30</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>14</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCell align="right"> <MessageButtonWithProvider userInfo={row} /></StyledTableCell>
                                </StyledTableRow>
                            ))}
                        </TableBodyStyled>
                    </Table>
                </TableContainer>
            </ContentTableBoxStyled>
        </TabBoxStyled>
    )
}

const Tutors = () => {
    const { t } = useTranslation();
    const tutors = useAppSelector(selectFilteredTutors);
    const dispatch = useAppDispatch();
    const approveStatus = useAppSelector(selectApproveUserStatus);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        if (approveStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.APPROVED), { variant: "success" });
        }
        else if (approveStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.FAILED_TO_APPROVE), { variant: "error" });
        }
    }, [approveStatus]);

    useEffect(() => {
        dispatch(setTutorsFilter({
            request: {
                filter: {},
                skip: 0,
                take: 20
            },
            appendType: AppendTypeEnum.None
        }));
    }, []);

    return (
        <TabBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.TUTORS)}</H4Styled>
            </HeaderBoxStyled>
            <ContentTableBoxStyled>
                <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                    <Table sx={{ minWidth: 650, padding: "0", border: "none" }} aria-label="simple table">
                        <TableHead>
                            <StyledTableCell><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.NAME)}</H5Styled></StyledTableCell>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.LANGUAGE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.LAST_ACTIVITY)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.NUMBER_OF_LESSONS)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding />
                        </TableHead>
                        <TableBodyStyled>
                            {tutors?.data.map((row) => (
                                <StyledTableRow key={row.subjectId!} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                    <StyledTableCell component="th" scope="row">
                                        <UserBoxStyled>
                                            <AvatarStyledSmall src={getProfilePhotoThumbnailUrl(row)} />
                                            <LinkItem><Link href={routes.tutorIdPage.path(row.userId!)}><H5Styled>{getUserName(row)}</H5Styled></Link></LinkItem>
                                        </UserBoxStyled>
                                    </StyledTableCell>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>English</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>22.05.2022 15:30</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>14</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCell align="right"> <MessageButtonWithProvider userInfo={row} /></StyledTableCell>
                                </StyledTableRow>
                            ))}
                        </TableBodyStyled>
                    </Table>
                </TableContainer>
            </ContentTableBoxStyled>
        </TabBoxStyled>
    )
}

const TutorsActions = ({ user }: { user: (AppUserOverviewVM) }) => {
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const submitted = useRef(false);
    const approveStatus = useAppSelector(selectApproveUserStatus);
    const refuseStatus = useAppSelector(selectRefuseUserStatus);
    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        if (!submitted.current) return;
        if (approveStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.APPROVED), { variant: "success" });
            submitted.current = false;
        }
        else if (approveStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.FAILED_TO_APPROVE), { variant: "error" });
            submitted.current = false;
        }
    }, [approveStatus]);

    useEffect(() => {
        if (!submitted.current) return;
        if (refuseStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.REFUSED), { variant: "success" });
            submitted.current = false;
        }
        else if (refuseStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.FAILED_TO_REFUSE), { variant: "error" });
            submitted.current = false;
        }
    }, [refuseStatus]);

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const refreshUsersInValidation = () => {
        return dispatch(setTutorsFilter({
            request: {
                filter: {
                    stateId: UserStateEnum.ProfileValidation
                },
                skip: 0,
                take: 20
            },
            appendType: AppendTypeEnum.None
        }));
    }

    const handleApprove = async () => {
        submitted.current = true;
        await dispatch(approveUser({ userId: user.userId! }));
        await refreshUsersInValidation();
    }

    const handleRefuse = async () => {
        submitted.current = true;
        await dispatch(refuseUser({ userId: user.userId! }));
        await refreshUsersInValidation();
    }

    return (
        <MainBoxStyled>
            <UserOuterBoxStyled>
                <Box
                    onClick={handleClick}
                    aria-controls={open ? 'past-lessons' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                    sx={{ display: "flex", justifyContent: 'center' }}
                >
                    <IconButton><IconDotsStyled><ThreeDotsIcon /></IconDotsStyled></IconButton>
                </Box>
            </UserOuterBoxStyled>
            <MenuStyled
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                onClick={handleClose}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                <MenuBoxStyled gap="4px">
                    <MenuItem icon={<ChatIcon />} text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WRITE_A_MESSAGE)} />
                    <Divider sx={{ color: COLORS.BLACK_5 }} />
                    <MenuItem icon={<Box sx={{ svg: { color: COLORS.GREEN }, display: "flex", alignItems: "center", gap: "8px" }} ><ApproveIcon /></Box>} onClick={handleApprove} text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.APPROVE)} />
                    <MenuItem icon={<Box sx={{ svg: { color: COLORS.RED }, display: "flex", alignItems: "center", gap: "8px" }}><RefuseIcon /></Box>} onClick={handleRefuse} text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.REFUSE)} />
                    <Divider sx={{ color: COLORS.BLACK_5 }} />
                    <MenuItem icon={<Box sx={{ svg: { color: COLORS.RED }, display: "flex", alignItems: "center", gap: "8px" }}> <DeleteIcon /></Box >} text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.DELETE)} />
                </MenuBoxStyled>
            </MenuStyled>
        </MainBoxStyled>
    )
}

const ReviewsStatistics = () => {
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.STATISTICS)}</H4Styled>
            <ContentBoxStyled>
                <StatisticsColumnsBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.AVERAGE_REVIEW_SCORE)}</Body1Styled>
                        <StarBoxStyled>
                            <StarIconStyled><StarIcon /></StarIconStyled>
                            <H3Styled>4.8</H3Styled>
                        </StarBoxStyled>
                    </StatisticsColumnBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.NUMBER_OF_RATINGS)}</Body1Styled>
                        <H3Styled>4</H3Styled>
                    </StatisticsColumnBoxStyled>
                </StatisticsColumnsBoxStyled>
            </ContentBoxStyled>
        </DashboardBoxStyled>
    )
}

const TransactionsStatisticsLoaded = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <ContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticsColumnBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.REVENUE_THIS_MONTH)}</Body1Styled>
                        <H3Styled>120 000 Kč</H3Styled>
                    </RowBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.REVENUE_THIS_YEAR)}</Body1Styled>
                        <H3Styled>120 000 Kč</H3Styled>
                    </RowBoxStyled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <RowBoxStyled>
                        {/* TODO Odecist stripe komisi + azure + VM */}
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.NET_INCOME_THIS_MONTH)}</Body1Styled>
                        <H3Styled>{localize({ price: userWalletStatistics.revenueThisMonth })}</H3Styled>
                    </RowBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.NET_INCOME_THIS_YEAR)}</Body1Styled>
                        <H3Styled>{localize({ price: userWalletStatistics.revenueThisYear })}</H3Styled>
                    </RowBoxStyled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAW_THIS_MONTH)}</Body1Styled>
                        <H3Styled>{localize({ price: userWalletStatistics.withdrawThisMonth })}</H3Styled>
                    </RowBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAW_THIS_YEAR)}</Body1Styled>
                        <H3Styled>{localize({ price: userWalletStatistics.withdrawThisYear })}</H3Styled>
                    </RowBoxStyled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <RowBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.TOTAL_REVENUE)}</Body1Styled>
                        <H3Styled>{localize({ price: userWalletStatistics.totalRevenue })}</H3Styled>
                    </RowBoxStyled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
        </ContentBoxStyled>
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
            <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.STATISTICS)}</H4Styled>
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics ? <TransactionsStatisticsLoaded userWalletStatistics={userWalletStatistics} /> : <></>}
        </DashboardBoxStyled>
    )
}


const OverviewFinancialStatisticsLoaded = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <ContentBoxStyled>
            <StatisticsColumnsBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.REVENUE_THIS_MONTH)}</Body1Styled>
                    <H3Styled>{localize({ price: userWalletStatistics.revenueThisMonth })}</H3Styled>
                </StatisticsColumnBoxStyled>
                <StatisticsColumnBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAW_THIS_MONTH)}</Body1Styled>
                    <H3Styled>{localize({ price: userWalletStatistics.withdrawThisMonth })}</H3Styled>
                </StatisticsColumnBoxStyled>
            </StatisticsColumnsBoxStyled>
        </ContentBoxStyled>
    )
}


const OverviewFinancialStatistics = () => {
    const userWalletStatistics = useAppSelector(selectUserWalletStatistics);
    const userWalletStatisticsStatus = useAppSelector(selectUserWalletStatisticsStatus);
    const dispatch = useAppDispatch();
    const { t } = useTranslation();

    useEffect(() => {
        dispatch(setUserWalletStatistics());
    }, []);

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.FINANCIAL_STATISTICS)}</H4Styled>
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics ? <OverviewFinancialStatisticsLoaded userWalletStatistics={userWalletStatistics} /> : <></>}
        </DashboardBoxStyled>
    )
}

const OverviewVideoQualityStatistics = () => {
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_CALL_STATISTICS)}</H4Styled>
            <ContentBoxStyled>
                <StatisticsColumnsBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_CALL_RATING)}</Body1Styled>
                        <StarBoxStyled>
                            <StarIconStyled><StarIcon /></StarIconStyled>
                            <H3Styled>4.8</H3Styled>
                        </StarBoxStyled>
                    </StatisticsColumnBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.NUMBER_OF_RATINGS)}</Body1Styled>
                        <H3Styled>8</H3Styled>
                    </StatisticsColumnBoxStyled>
                </StatisticsColumnsBoxStyled>
            </ContentBoxStyled>
        </DashboardBoxStyled>
    )
}

const OverviewStudents = () => {
    const { t } = useTranslation();
    const students = useAppSelector(selectFilteredStudents);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setStudentsFilter({
            request: {
                filter: {},
                skip: 0,
                take: 20
            },
            appendType: AppendTypeEnum.None
        }));
    }, []);

    return (
        <DashboardBoxStyled>
            <HeadingBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.STUDENTS)}</H4Styled>
                <H4Styled>({students?.size})</H4Styled>
            </HeadingBoxStyled>
            <ContentBoxStyled>
                {students ? students.data.map((x, idx) => (
                    <OverviewItemStyled key={idx}>
                        <StudentTutorBoxStyled>
                            <Avatar src={getProfilePhotoThumbnailUrl(x)} />
                            <H5Styled>{getUserName(x)}</H5Styled>
                            <Body1Styled>12</Body1Styled>
                        </StudentTutorBoxStyled>
                        <MessageButtonWithProvider userInfo={x} />
                    </OverviewItemStyled>
                )) : <></>}
            </ContentBoxStyled>
        </DashboardBoxStyled>);
}

const OverviewTutors = () => {
    const { t } = useTranslation();
    const tutors = useAppSelector(selectFilteredTutors);
    const dispatch = useAppDispatch();


    useEffect(() => {
        dispatch(setTutorsFilter({
            request: {
                filter: {
                    stateId: UserStateEnum.ProfileValidation
                },
                skip: 0,
                take: 20
            },
            appendType: AppendTypeEnum.None
        }));
    }, []);

    return (
        <DashboardBoxStyled>
            <HeadingBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.TUTORS_TO_BE_APPROVED)}</H4Styled>
                <H4Styled>({tutors?.size})</H4Styled>
            </HeadingBoxStyled>
            <ContentBoxStyled>
                {tutors ? tutors.data.map((x, idx) => (
                    <OverviewItemStyled key={idx}>
                        <StudentTutorBoxStyled>
                            <Avatar src={getProfilePhotoThumbnailUrl(x)} />
                            <LinkItem><Link href={routes.tutorIdPage.path(x.userId!)}><H5Styled>{getUserName(x)}</H5Styled></Link></LinkItem>
                            <StarAndTextBoxStyled>
                                <StarBoxStyled>
                                    <StarFilled />
                                </StarBoxStyled>
                                <H5Styled>{getUserRating(x)}</H5Styled>
                            </StarAndTextBoxStyled>
                        </StudentTutorBoxStyled>
                        <TutorsActions user={x} />
                    </OverviewItemStyled>
                )) : <></>}
            </ContentBoxStyled>
        </DashboardBoxStyled>
    );
}

const VideoQualityStatistics = () => {
    const { t } = useTranslation();

    return (
        <DashboardBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.STATISTICS)}</H4Styled>
            <ContentBoxStyled>
                <StatisticsColumnsBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_QUALITY)}</Body1Styled>
                        <StarBoxStyled>
                            <StarIconStyled><StarIcon /></StarIconStyled>
                            <H3Styled>4.8</H3Styled>
                        </StarBoxStyled>
                    </StatisticsColumnBoxStyled>
                    <StatisticsColumnBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.NUMBER_OF_RATINGS)}</Body1Styled>
                        <H3Styled>4</H3Styled>
                    </StatisticsColumnBoxStyled>
                </StatisticsColumnsBoxStyled>
            </ContentBoxStyled>
        </DashboardBoxStyled>
    )
}

const OverviewTab = () => {
    return (
        <DashboardRowBox>
            <TabContentBoxStyled>
                <OverviewFinancialStatistics />
                <OverviewVideoQualityStatistics />
            </TabContentBoxStyled>
            <TabContentBoxStyled>
                <OverviewStudents />
                <OverviewTutors />
            </TabContentBoxStyled>
        </DashboardRowBox>
    )
}

const MessageButton = () => {
    const { t } = useTranslation();

    const { setOpen: setContactUserModalOpen } = useContactUserModalContext();

    return (
        <ActionBoxStyled>
            <SecondaryButtonCompacted onClick={setContactUserModalOpen}>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.MESSAGE_BUTTON)}</SecondaryButtonCompacted>
        </ActionBoxStyled>
    )
}

const MessageButtonWithProvider = ({ userInfo }: { userInfo: FilterTutorsResponseBody["data"][0] }) => {
    return (
        <ContactUserModalProvider userInfo={userInfo}>
            <MessageButton />
        </ContactUserModalProvider >
    )
}

const AttendenceMonitoring = () => {
    const { t } = useTranslation();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <TabBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.ATTENDENCE_MONITORING)}</H4Styled>
            </HeaderBoxStyled>
            <ContentTableBoxStyled>
                <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                    <Table sx={{ minWidth: 650, padding: "0", border: "none" }} aria-label="simple table">
                        <TableHead>
                            <StyledTableCell><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.USERS_TABLE.NAME)}</H5Styled></StyledTableCell>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.ATTENDANCE_MONITORING_TABLE.LATENESS_SCORE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.ATTENDANCE_MONITORING_TABLE.NOT_ATTENDED_LESSONS)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding />
                        </TableHead>
                        <TableBodyStyled>
                            <StyledTableRow sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                <StyledTableCell align="left">
                                    <Body1BlackStyled>Stefan Boskovic</Body1BlackStyled>
                                </StyledTableCell>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>03:11</Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>3</Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCell align="right"><SecondaryButtonCompacted>Message</SecondaryButtonCompacted></StyledTableCell>
                            </StyledTableRow>
                            <StyledTableRow sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                <StyledTableCell align="left">
                                    <Body1BlackStyled>Stefan Boskovic</Body1BlackStyled>
                                </StyledTableCell>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>03:11</Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>3</Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCell align="right"><SecondaryButtonCompacted>Message</SecondaryButtonCompacted></StyledTableCell>
                            </StyledTableRow>
                        </TableBodyStyled>
                    </Table>
                </TableContainer>
            </ContentTableBoxStyled>
        </TabBoxStyled>
    )
}

const TutorsTab = () => {
    return (
        <Tutors />
    )
}
const StudentsTab = () => {
    return (
        <Students />
    )
}
const ReviewsTab = () => {
    const dispatch = useAppDispatch();
    const userInfo = useAppSelector(selectShortUserInfo);
    const { t } = useTranslation();

    useEffect(() => {
        dispatch(setPublicTutorInfo({ userId: userInfo!.userId! }));
    }, [dispatch, userInfo]);

    return (
        <React.Fragment>
            <ReviewsStatistics />
            <Box>
                <H4Styled sx={{ marginBottom: "16px" }}>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_QUALITY)}</H4Styled>
                <ReviewsBox>
                    <DashboardBoxStyled >
                        <ColumnsTableBoxStyled>
                            <Avatar sx={{ width: 48, height: 48 }} src={"/images/tutor_hero.png"} />
                            <ColumnTableBoxStyled>
                                <Box sx={{ display: "flex", gap: "8px", flexDirection: "row", alignItems: "center" }}>
                                    <H5Styled>Stefan Boskovic</H5Styled>
                                    <PStyled>to</PStyled>
                                    <Avatar sx={{ width: 32, height: 32 }} src={"/images/tutor_hero.png"} />
                                    <H5Styled>Jessica Alba</H5Styled>
                                    <Body1Styled sx={{ display: { xs: "none", md: "flex" } }}>
                                        2 day(s)ago
                                    </Body1Styled>
                                </Box>
                                <StarBoxStyled>
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarEmpty />
                                </StarBoxStyled>
                                <Body1Styled sx={{ display: { xs: "flex", md: "none" } }}>
                                    2 day(s)ago
                                </Body1Styled>
                                <Body1Styled>Good tutor and very patient and helpful with good course material, but suffers from connection problems so not reliable. I found a different tutor due to the connection issues. 7/10</Body1Styled>
                            </ColumnTableBoxStyled>
                        </ColumnsTableBoxStyled>
                        <ColumnsTableBoxStyled>
                            <Avatar sx={{ width: 48, height: 48 }} src={"/images/tutor_hero.png"} />
                            <ColumnTableBoxStyled>
                                <Box sx={{ display: "flex", gap: "8px", flexDirection: "row", alignItems: "center" }}>
                                    <H5Styled>Stefan Boskovic</H5Styled>
                                    <PStyled>to</PStyled>
                                    <Avatar sx={{ width: 32, height: 32 }} src={"/images/tutor_hero.png"} />
                                    <H5Styled>Jessica Alba</H5Styled>
                                    <Body1Styled sx={{ display: { xs: "none", md: "flex" } }}>
                                        2 day(s)ago
                                    </Body1Styled>
                                </Box>
                                <StarBoxStyled>
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarEmpty />
                                </StarBoxStyled>
                                <Body1Styled sx={{ display: { xs: "flex", md: "none" } }}>
                                    2 day(s)ago
                                </Body1Styled>
                                <Body1Styled>Good tutor and very patient and helpful with good course material, but suffers from connection problems so not reliable. I found a different tutor due to the connection issues. 7/10</Body1Styled>
                            </ColumnTableBoxStyled>
                        </ColumnsTableBoxStyled>
                    </DashboardBoxStyled>
                </ReviewsBox>
            </Box>
        </React.Fragment>
    )
}
const VideoQualityTab = () => {
    const { t } = useTranslation();

    return (
        <React.Fragment>
            <VideoQualityStatistics />
            <Box>
                <H4Styled sx={{ marginBottom: "16px" }}>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_QUALITY)}</H4Styled>
                <ReviewsBox>
                    <DashboardBoxStyled>
                        <ColumnsTableBoxStyled>
                            <Avatar sx={{ width: 48, height: 48 }} src={"/images/tutor_hero.png"} />
                            <ColumnTableBoxStyled>
                                <ReviewNameBoxStyled>
                                    <H5Styled>Stefan Boskovic</H5Styled>
                                    <Body1Styled>
                                        2 day(s)ago
                                    </Body1Styled>
                                </ReviewNameBoxStyled>
                                <StarBoxStyled>
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarFilled />
                                    <StarEmpty />
                                </StarBoxStyled>
                                <Body1Styled>Good tutor and very patient and helpful with good course material, but suffers from connection problems so not reliable. I found a different tutor due to the connection issues. 7/10</Body1Styled>
                            </ColumnTableBoxStyled>
                        </ColumnsTableBoxStyled>
                    </DashboardBoxStyled>
                </ReviewsBox>
            </Box>
        </React.Fragment>
    )
}
const AttendeceMonitoringTab = () => {
    return (
        <AttendenceMonitoring />
    )
}

const TransactionsTab = () => {

    return (
        <React.Fragment>
            <TransactionsStatistics />
            <Transactions />
        </React.Fragment>
    )
}

const WithdrawlsTab = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const withdrawFilter = useAppSelector(selectWithdrawTransactionFilter);
    const completeState = useAppSelector(selectCompleteTransactionStatus);
    const { enqueueSnackbar } = useSnackbar();
    const { locale } = useAppDataContext();
    const reqAcc = useRef(false);
    const { localize } = usePriceLocalization();

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        if (!reqAcc.current) return;
        if (completeState === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.FAILED_TO_COMPLETE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (completeState === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.SUCCESSFULLY_COMPLETED), { variant: "success" });
        }
    }, [completeState]);

    const fetchData = () => {
        dispatch(setWithdrawFilter({
            request: {
                filter: {
                    transactionStateId: TransactionStateEnum.Pending
                },
                take: 200
            },
            appendType: AppendTypeEnum.None
        }));
    }

    const handleClick = (transactionId: number) => {
        if (completeState === LoadingStatusEnum.Loading) return;

        reqAcc.current = true;
        dispatch(setCompleteTrasnsatction({ transactionId }))
            .then(() => {
                fetchData()
                reqAcc.current = false;
            })
            .catch(() => {
                reqAcc.current = false;
            })
    }

    return (
        <TabBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS)}</H4Styled>
            </HeaderBoxStyled>
            <ContentTableBoxStyled>
                <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                    <Table sx={{ minWidth: 650, padding: "0", border: "none" }} aria-label="simple table">
                        <TableHead>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.NAME)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.USER_TYPE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.BALANCE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.DATE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.STATE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.AMOUNT)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.BANK_ACCOUNT)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.ACTION)}</H5Styled></StyledTableCellWithPadding>
                        </TableHead>
                        <TableBodyStyled>
                            {withdrawFilter?.data?.map(x => (
                                <StyledTableRow key={x.transactionId!} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                    <StyledTableCellWithPadding align="left">
                                        <UserBoxCell user={x.creator} />
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{x.creator?.userRoleId === UserRoleEnum.Tutor ? "Tutor" : "Student"}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{localize({ price: x.wallet?.amount })}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{moment(x.created).format("DD.MM.yyyy HH:mm")}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{t(x.state?.nameKey ?? "")}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{localize({ price: Math.abs(x.amountAfterFee ?? 0) })}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{`${x.bankAccountNumber}/${x.bankCode}`}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <CellButtonBoxStyled onClick={() => handleClick(x.transactionId ?? 0)}>
                                            {completeState === LoadingStatusEnum.Loading
                                                ? <CircularProgressCustom />
                                                : <Body1BlackStyled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS_TABLE.MARK_AS_COMPLETED)}</Body1BlackStyled>
                                            }
                                        </CellButtonBoxStyled>
                                    </StyledTableCellWithPadding>
                                </StyledTableRow>
                            ))}
                        </TableBodyStyled>
                    </Table>
                </TableContainer>
            </ContentTableBoxStyled>
        </TabBoxStyled>
    )
}


type ReservationIssueApproveModalContextType = { setOpen: () => void } | undefined;
type ReservationIssueRefuseModalContextType = { setOpen: () => void } | undefined;
const ReservationIssueApproveModalContext = createContext<ReservationIssueApproveModalContextType>(undefined);
const ReservationIssueRefuseModalContext = createContext<ReservationIssueRefuseModalContextType>(undefined);
export const useReservationIssueApproveModal = contextFactoryCreate('ReservationIssueApproveModal', ReservationIssueApproveModalContext);
export const useReservationIssueRefuseModal = contextFactoryCreate('ReservationIssueRefuseModal', ReservationIssueRefuseModalContext);

const ReservationIssueApproveModalProvider = ({ reservationIssue, children }: { reservationIssue: NonNullable<ReservationIssueFilterResponse["data"][0]>, children: any }) => {
    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState("");
    const [tutorPercentage, setTutorPercentage] = useState<number | undefined>();
    const [studentPercentage, setStudentPercentage] = useState<number | undefined>();
    const { t } = useTranslation();
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const requestSent = useRef(false);
    const dispatch = useAppDispatch();
    const { enqueueSnackbar } = useSnackbar();
    const refuseStatus = useAppSelector(selectReservationIssueApproveStatus);

    useEffect(() => {
        if (!requestSent.current) return;
        if (refuseStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.ERROR), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (refuseStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.SUCCESS), { variant: "success" });
            setOpen(false);
        }
    }, [refuseStatus]);

    const handleMessageChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setMessage(value as string);
    }

    const handleTutorPercentageChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        var value = parseInt(e.target.value as string);
        if (value > 100) return;
        setTutorPercentage(value);
    }

    const handleStudentPercentageChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        var value = parseInt(e.target.value as string);
        if (value > 100) return;
        setStudentPercentage(value);
    }

    const handleClick = async () => {
        if (studentPercentage === undefined) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.STUDENT_VALUE_CANNOT_BE_EMPTY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        if (studentPercentage <= 0) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.STUDENT_VALUE_MUST_BE_GREATER_THAN_ZERO), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        if (tutorPercentage === undefined) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.TUTOR_VALUE_CANNOT_BE_EMPTY), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        const sum = (studentPercentage ?? 0) + (tutorPercentage ?? 0);
        if (sum < 100) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.PERCENTAGE_SUM_ERROR), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            return;
        }

        requestSent.current = true;
        await dispatch(approveReservationIssue({
            reservationIssueId: reservationIssue.reservationIssueId,
            decisionText: message,
            studentPercentage: (studentPercentage ?? 0) / 100
        }));

        await dispatch(setReservationIssuesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                filter: {
                    issueStateId: ReservationIssueStateEnum.Waiting
                },
                skip: 0,
                take: 200
            }
        }));
    }

    return (
        <ReservationIssueApproveModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.TITLE)} handleClose={handleClose} />
                    <ModalUserBoxStyled>
                        <UserAvatarBoxStyled src={getProfilePhotoUrl(reservationIssue.reservation?.attender)} />
                        <Box>
                            <H5Styled>{getUserName(reservationIssue.reservation?.attender)}</H5Styled>
                            <PStyled>{moment(reservationIssue.reservation?.startTime).format("DD.MM.yyyy HH:mm")}-{moment(reservationIssue.reservation?.endTime).format("HH:mm")}</PStyled>
                        </Box>
                    </ModalUserBoxStyled>

                    <LabelBoxStyled>
                        <AttributeLabelTypographyStyled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.TUTOR_INPUT_TITLE)}</AttributeLabelTypographyStyled>
                        <TextFieldStyled
                            type="number"
                            fullWidth
                            placeholder={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.TUTOR_INPUT_PLACEHOLDER)}
                            value={`${tutorPercentage ?? ""}`}
                            onChange={handleTutorPercentageChange}
                        />
                    </LabelBoxStyled>

                    <LabelBoxStyled>
                        <AttributeLabelTypographyStyled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.STUDENT_INPUT_TITLE)}</AttributeLabelTypographyStyled>
                        <TextFieldStyled
                            type="number"
                            fullWidth
                            placeholder={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.STUDENT_INPUT_PLACEHOLDER)}
                            value={`${studentPercentage ?? ""}`}
                            onChange={handleStudentPercentageChange}
                        />
                    </LabelBoxStyled>

                    <LabelBoxStyled>
                        <AttributeLabelTypographyStyled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.MESSAGE_INPUT_HEADER)}</AttributeLabelTypographyStyled>
                        <TextareaBoxStyled>
                            <textarea
                                value={message}
                                placeholder={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.MESSAGE_INPUT_PLACEHOLDER)}
                                onChange={handleMessageChange}
                            />
                        </TextareaBoxStyled>
                    </LabelBoxStyled>

                    <PrimaryButtonRectangle
                        onClick={handleClick}
                        sx={{ backgroundColor: COLORS.GREEN, ":hover": { backgroundColor: COLORS.GREEN_HOVER } }}
                    >
                        {refuseStatus === LoadingStatusEnum.Loading
                            ? <CircleProgressStyled />
                            : t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.APPROVE_MODAL.APPOVE_BUTTON)
                        }
                    </PrimaryButtonRectangle>
                </ModalBoxStyled>
            </ModalStyled>
        </ReservationIssueApproveModalContext.Provider>
    )
}

const ReservationIssueRefuseModalProvider = ({ reservationIssue, children }: { reservationIssue: NonNullable<ReservationIssueFilterResponse["data"][0]>, children: any }) => {
    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState("");
    const { t } = useTranslation();
    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const requestSent = useRef(false);
    const dispatch = useAppDispatch();
    const { enqueueSnackbar } = useSnackbar();
    const refuseStatus = useAppSelector(selectReservationIssueDeclineStatus);

    useEffect(() => {
        if (!requestSent.current) return;
        if (refuseStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.ERROR), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        else if (refuseStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.SUCCESS), { variant: "success" });
            setOpen(false);
        }
    }, [refuseStatus]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const value = e.target.value;
        setMessage(value as string);
    }

    const handleClick = async () => {
        requestSent.current = true;
        await dispatch(declineReservationIssue({
            reservationIssueId: reservationIssue.reservationIssueId,
            decisionText: message
        }));
        await dispatch(setReservationIssuesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                filter: {
                    issueStateId: ReservationIssueStateEnum.Waiting
                },
                skip: 0,
                take: 200
            }
        }));
    }

    return (
        <ReservationIssueRefuseModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.TITLE)} handleClose={handleClose} />
                    <ModalUserBoxStyled>
                        <UserAvatarBoxStyled src={getProfilePhotoUrl(reservationIssue.reservation?.attender)} />
                        <Box>
                            <H5Styled>{getUserName(reservationIssue.reservation?.attender)}</H5Styled>
                            <PStyled>{moment(reservationIssue.reservation?.startTime).format("DD.MM.yyyy HH:mm")}-{moment(reservationIssue.reservation?.endTime).format("HH:mm")}</PStyled>
                        </Box>
                    </ModalUserBoxStyled>

                    <LabelBoxStyled>
                        <AttributeLabelTypographyStyled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.MESSAGE_INPUT_HEADER)}</AttributeLabelTypographyStyled>
                        <TextareaBoxStyled>
                            <textarea
                                value={message}
                                placeholder={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.MESSAGE_INPUT_PLACEHOLDER)}
                                onChange={handleChange}
                            />
                        </TextareaBoxStyled>
                    </LabelBoxStyled>

                    <PrimaryButtonRectangle
                        onClick={handleClick}
                        sx={{ backgroundColor: COLORS.RED, ":hover": { backgroundColor: COLORS.RED_HOVER } }}
                    >
                        {refuseStatus === LoadingStatusEnum.Loading
                            ? <CircleProgressStyled />
                            : t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.REFUSE_MODAL.REFUSE_BUTTON)
                        }
                    </PrimaryButtonRectangle>
                </ModalBoxStyled>
            </ModalStyled>
        </ReservationIssueRefuseModalContext.Provider>
    )
}

const ReservationIssueAction = () => {
    const { t } = useTranslation();
    const { setOpen: setApproveModalOpen } = useReservationIssueApproveModal();
    const { setOpen: setRefuseModalOpen } = useReservationIssueRefuseModal();

    return (
        <ThreeDotsMenu innerMenu={
            <MenuBoxStyled gap="4px">
                <MenuItem
                    icon={<Box sx={{ display: "flex", alignItems: "center", color: COLORS.GREEN }}><CheckCircleIcon /></Box>}
                    text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.ACTIONS.APPROVE)}
                    onClick={setApproveModalOpen}
                />
                <MenuItem
                    icon={<Box sx={{ display: "flex", alignItems: "center", color: COLORS.RED }}><XCircleIcon /></Box>}
                    text={t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.ACTIONS.REFUSE)}
                    onClick={setRefuseModalOpen}
                />
            </MenuBoxStyled>
        } />
    )
}

const ReservationIssueActionWithProviders = ({ reservationIssue }: { reservationIssue: NonNullable<ReservationIssueFilterResponse["data"][0]> }) => {

    return (
        <ReservationIssueApproveModalProvider reservationIssue={reservationIssue}>
            <ReservationIssueRefuseModalProvider reservationIssue={reservationIssue}>
                <ReservationIssueAction />
            </ReservationIssueRefuseModalProvider>
        </ReservationIssueApproveModalProvider>
    )
}

const ReservationIssuesTab = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const reservationIssuesFilter = useAppSelector(selectReservationIssueFilter);
    const reservationIssuesFilterStatus = useAppSelector(selectReservationIssueFilterStatus);

    useEffect(() => {
        dispatch(setReservationIssuesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                filter: {
                    issueStateId: ReservationIssueStateEnum.Waiting
                },
                skip: 0,
                take: 200
            }
        }))
    }, []);

    return (
        <TabBoxStyled>
            <HeaderBoxStyled>
                <H4Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.CLAIMS)}</H4Styled>
            </HeaderBoxStyled>
            <ContentTableBoxStyled>
                <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none" }}>
                    <Table sx={{ minWidth: 650, padding: "0", border: "none" }} aria-label="simple table">
                        <TableHead>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.TYPE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.STUDENT_NAME)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.TUTOR_NAME)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.LESSON_DATE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.MESSAGE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.ADMIN_DASHBOARD.RESERVATION_ISSUE_TABLE.STATE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding />
                        </TableHead>
                        <TableBodyStyled>
                            {reservationIssuesFilter?.data?.map(x => (
                                <StyledTableRow key={x.reservationIssueId!} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{t(x.type?.nameKey ?? "")}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCell align="left">
                                        <UserBoxCell user={x.reservation?.attender} />
                                    </StyledTableCell>
                                    <StyledTableCell align="left">
                                        <UserBoxCell user={x.reservation?.host} />
                                    </StyledTableCell>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{moment(x.created).format('MM.DD.yyyy HH:mm')}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left" style={{ whiteSpace: "pre-line", overflow: "hidden" }}>
                                        <Body1BlackStyled>{x.description ?? ""}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <Body1BlackStyled>{t(x.state?.nameKey ?? "")}</Body1BlackStyled>
                                    </StyledTableCellWithPadding>
                                    <StyledTableCellWithPadding align="left">
                                        <ReservationIssueActionWithProviders reservationIssue={x} />
                                    </StyledTableCellWithPadding>
                                </StyledTableRow>
                            ))}
                        </TableBodyStyled>
                    </Table>
                </TableContainer>
            </ContentTableBoxStyled>
        </TabBoxStyled>
    )
}

type AdminDashboardTabType = "overview" | "tutors" | "students" | "reviews" | "videoQuality" | "attendenceMonitoring" | "transactions" | "claims" | "withdrawals"

const Tabs: { type: AdminDashboardTabType, translationKey: string }[] =
    [{ type: "overview", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.OVERVIEW },
    { type: "tutors", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.TUTORS },
    { type: "students", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.STUDENTS },
    { type: "reviews", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.REVIEWS },
    { type: "videoQuality", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.VIDEO_QUALITY },
    { type: "attendenceMonitoring", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.ATTENDENCE_MONITORING },
    { type: "transactions", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.TRANSACTIONS },
    { type: "claims", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.CLAIMS },
    { type: "withdrawals", translationKey: TRANSLATION_KEYS.ADMIN_DASHBOARD.WITHDRAWALS }]

const AdminDashboard = () => {
    const [currentTab, setTab] = useState<AdminDashboardTabType>("overview");
    const { t } = useTranslation();

    return (
        <React.Fragment>
            <Navbar source={NavbarSourceEnum.tutorPage} />
            <BackgroundBoxStyled>
                <ContainerStyled>
                    <Box sx={{ display: "flex", gap: "32px", flexDirection: "column" }}>
                        <TabsBoxStyled>
                            {Tabs.map((x, idx) => (
                                <TabItemButtonStyled key={idx} onClick={() => setTab(x.type)}
                                    sx={{
                                        backgroundColor: currentTab === x.type ? COLORS.PRIMARY : "transparent",
                                        color: currentTab === x.type ? COLORS.WHITE : COLORS.BLACK,
                                        ":hover": { backgroundColor: currentTab === x.type ? COLORS.PRIMARY : COLORS.PRIMARY_10, color: currentTab === x.type ? COLORS.WHITE : COLORS.BLACK }
                                    }}>
                                    {t(x.translationKey)}
                                </TabItemButtonStyled>
                            ))}
                        </TabsBoxStyled>
                        {currentTab === "overview" ? <OverviewTab /> : <></>}
                        {currentTab === "tutors" ? <TutorsTab /> : <></>}
                        {currentTab === "students" ? <StudentsTab /> : <></>}
                        {currentTab === "reviews" ? <ReviewsTab /> : <></>}
                        {currentTab === "videoQuality" ? <VideoQualityTab /> : <></>}
                        {currentTab === "attendenceMonitoring" ? <AttendeceMonitoringTab /> : <></>}
                        {currentTab === "transactions" ? <TransactionsTab /> : <></>}
                        {currentTab === "claims" ? <ReservationIssuesTab /> : <></>}
                        {currentTab === "withdrawals" ? <WithdrawlsTab /> : <></>}
                    </Box>
                </ContainerStyled>
            </BackgroundBoxStyled>
        </React.Fragment>
    );
};

export default withAppClientSideProvider(AdminDashboard);

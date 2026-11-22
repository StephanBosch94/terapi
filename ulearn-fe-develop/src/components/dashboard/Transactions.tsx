import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import ErrorLarge from "components/core/error/ErrorLarge";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { TertiaryButton } from "components/styles/TertiaryButton";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import H5Styled from "components/styles/typography/H5Styled";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { TransactionFilterResponse, selectTransactionFilter, selectTransactionFilterStatus, setTransactionFilter } from "core/redux/transactionSlice";
import { getPriceTranslation } from "core/services/Utils/walletUtil";
import moment from "moment";
import { useContext, useEffect, useRef, useState } from "react";
import DownloadIcon from "assets/download.svg";
import OutgoingIcon from "assets/arrow_bend_left.svg";
import IncomingIcon from "assets/arrow_bend_right.svg";
import EmptyTransactionsIcon from "assets/empty_transactions.svg";
import PStyled from "components/styles/typography/PStyled";
import H4Styled from "components/styles/typography/H4Styled";
import { selectShortUserInfo } from "core/redux/userSlice";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import TableHead from "@mui/material/TableHead";
import UserBoxCell from "./UserBoxCell";
import Link from "next/link";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import AppTransactionService from "core/services/AppTransactionService";
import { selectUserRoleId } from "core/redux/authSlice";
import { SecondaryButtonRectangle } from "components/styles/SecondaryButtonRectangle";
import ThreeDotsMenu from "components/menu/ThreeDotsMenu";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import MenuLinkItem from "components/menu/MenuLinkItem";
import { UIContext } from "components/core/UIContextProvider";
import { useAppDataContext } from "components/core/providers/AppProvider";
import CircularProgress from "@mui/material/CircularProgress";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { apiHasMoreItems } from "core/services/Utils/apiUtil";
import { isTimeToFeatchMore } from "core/services/Utils/infiniteScrollUtil";

const TransactionBoxStyled = styled(Box)({
    width: "100%",
    borderRadius: "4px",
    display: "flex",
    height: "75vh",
    gap: "24px",
    border: `1px solid ${COLORS.BLACK_10}`,
    flexDirection: "row",
    padding: "0px 16px",
    overflow: "auto",
    "::-webkit-scrollbar": {
        display: "none"
    }
}) as typeof Box;

const OtherTransactionBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "row",
    gap: "16px",
    alignItems: "center",
}) as typeof Box;

const ActionBoxStyled = styled(Box)({
    width: "100%",
    display: "flex",
    justifyContent: "end",
    minWidth: "max-content",
    textDecoration: "none",
    "a": {
        textDecoration: "none"
    }
}) as typeof Box;

const TertiaryButtonStyled = styled(TertiaryButton)({
    display: "flex",
    gap: "8px",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY
    }
}) as typeof TertiaryButton;

const TransactionTypeBoxStyled = styled(Box)({
    backgroundColor: COLORS.PRIMARY_10,
    padding: "12px",
    borderRadius: "50%"
}) as typeof Box;

const IconStyledBoxPrimaryBig = styled(Box)({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.5)"
    }
}) as typeof Box;

const TableBodyStyled = styled(TableBody)({
    padding: "0",
    border: "none",
    overflowY: "auto",
}) as typeof TableBody;

const EmptyTransactionsHeader = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "column noWrap"
}) as typeof Box;

const EmptyTransationsContentStyled = styled(Box)({
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

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
    },
    // hide last border
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}));

const Body1BlackStyled = styled(Body1Styled)({
    color: COLORS.BLACK
}) as typeof Body1Styled;

const TransactionHeaderBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    marginBottom: "16px",
    justifyContent: "space-between",
    "a": {
        textDecoration: "none",
    }
}) as typeof Box;

const TransactionHeaderButtonsBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    gap: "4px"
}) as typeof Box;

const DownloadPrimaryButtonStyled = styled(PrimaryButtonRectangle)({
    display: "flex",
    gap: "8px",
    alignItems: "center",
    padding: "8px 16px",
    svg: {
        color: COLORS.WHITE
    }
}) as typeof PrimaryButtonRectangle;

const DownloadSecondaryButtonStyled = styled(SecondaryButtonRectangle)({
    display: "flex",
    gap: "8px",
    alignItems: "center",
    padding: "8px 16px",
    svg: {
        color: COLORS.PRIMARY
    }
}) as typeof SecondaryButtonRectangle;

const IconCell = ({ amount, typeKey }: { amount: number | undefined, typeKey: string | null | undefined }) => {
    const { t } = useTranslation();
    return (
        <OtherTransactionBoxStyled>
            <TransactionTypeBoxStyled>
                <IconStyledBoxPrimaryBig>
                    {amount && amount > 0 ? <IncomingIcon /> : <OutgoingIcon />}
                </IconStyledBoxPrimaryBig>
            </TransactionTypeBoxStyled>
            <H5Styled>{t(typeKey ?? "")}</H5Styled>
        </OtherTransactionBoxStyled>
    )
}

const DownloadButton = ({ receiptUrl }: { receiptUrl: string | null | undefined }) => {
    const { t } = useTranslation();
    return (
        <ActionBoxStyled>
            <Link href={receiptUrl ?? ""}>
                <TertiaryButtonStyled><DownloadIcon />{t(TRANSLATION_KEYS.DASHBOARD.DOWNLOAD)}</TertiaryButtonStyled>
            </Link>
        </ActionBoxStyled>
    )
}

const TransactionDate = ({ transaction }: { transaction: NonNullable<TransactionFilterResponse['data'][0]> }) => {

    return transaction.reservation ? (
        <>{moment(transaction.reservation?.startTime).format("DD.MM.yyyy HH:mm")} - {moment(transaction.reservation?.endTime).format("HH:mm")}</>
    ) : (
        <>{moment(transaction?.created).format("DD.MM.YYYY HH:mm")}</>
    );
}


const InfinityLoadSkeleton = () => {

    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", height: "30px", width: "100%", flex: "0 1 auto" }}>
            <CircularProgress size={20} />
        </Box>
    )
}

const TransactionsLoaded = ({ transactions, fetchNext }: { transactions: TransactionFilterResponse, fetchNext: (newSkip: number) => Promise<any> }) => {
    const { t } = useTranslation();
    const { locale } = useAppDataContext();
    const [skip, setSkip] = useState(0);
    const tableRef = useRef<HTMLTableSectionElement>(null);
    const isLoadingRef = useRef(false);
    const hasMoreItems = apiHasMoreItems(transactions.size!, skip);
    const status = useAppSelector(selectTransactionFilterStatus);

    const handleScroll = () => {
        if (isLoadingRef.current) return;
        if (!hasMoreItems) return;

        if (isTimeToFeatchMore(tableRef)) {
            handleFetchNext();
        }
    }

    const handleFetchNext = async () => {
        isLoadingRef.current = true;
        const newSkip = skip + COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW;
        setSkip(newSkip);
        await fetchNext(newSkip);
        isLoadingRef.current = false;
    }

    return (
        <TransactionBoxStyled>
            <TableContainer component={Paper} sx={{ border: "none", padding: "0", boxShadow: "none", overflow: "auto" }} ref={tableRef} onScroll={handleScroll}>
                <Table stickyHeader sx={{ minWidth: 650, padding: "0", border: "none", overflow: "auto", gap: "0 !important" }} >
                    <TableHead>
                        <TableRow>
                            <StyledTableCell><H5Styled>{t(TRANSLATION_KEYS.TRANSACTIONS_TABLE_COLUMNS.TYPE)}</H5Styled></StyledTableCell>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.TRANSACTIONS_TABLE_COLUMNS.AMMOUNT)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.TRANSACTIONS_TABLE_COLUMNS.LESSON_DATE)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding><H5Styled>{t(TRANSLATION_KEYS.TRANSACTIONS_TABLE_COLUMNS.STATUS)}</H5Styled></StyledTableCellWithPadding>
                            <StyledTableCellWithPadding />
                        </TableRow>
                    </TableHead>

                    <TableBodyStyled>
                        {transactions.data.map((row) => (
                            <StyledTableRow
                                key={row.transactionId!}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <StyledTableCell scope="row">
                                    {row.opponent
                                        ? (<UserBoxCell user={row.opponent} typeKey={row.transactionType?.nameKey} />)
                                        : (<IconCell amount={row.amount} typeKey={row.transactionType?.nameKey} />)}
                                </StyledTableCell>
                                <StyledTableCellWithPadding align="left">
                                    <H5Styled>
                                        {getPriceTranslation(t, row.amount, row.currency?.nameKey, locale, true)}
                                    </H5Styled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>
                                        <TransactionDate transaction={row} />
                                    </Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCellWithPadding align="left">
                                    <Body1BlackStyled>
                                        {t(row?.transactionState?.nameKey ?? "")}
                                    </Body1BlackStyled>
                                </StyledTableCellWithPadding>
                                <StyledTableCell align="left"><DownloadButton receiptUrl={row.receiptUrl} /></StyledTableCell>
                            </StyledTableRow>
                        ))}
                    </TableBodyStyled>
                </Table>
                {status === LoadingStatusEnum.Loading ? <InfinityLoadSkeleton /> : <></>}
            </TableContainer>
        </TransactionBoxStyled>
    )
}

const TrasactionsLoading = () => (
    <TransactionBoxStyled sx={{ alignItems: "center", justifyContent: "center", minHeight: "366px" }}>
        <CircularProgressCustom />
    </TransactionBoxStyled>
)

const TransactionsError = () => (
    <TransactionBoxStyled sx={{ alignItems: "center", justifyContent: "center", minHeight: "366px" }}>
        <ErrorLarge />
    </TransactionBoxStyled>
)

const EmptyTransactions = () => {
    const userInfo = useAppSelector(selectShortUserInfo);
    const { t } = useTranslation();

    return (
        <EmptyTransationsContentStyled>
            <EmptyTransactionsHeader>
                <H4Styled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.EMPTY_TRANSACTIONS.TITLE)}</H4Styled>
                {userInfo?.userRoleId === UserRoleEnum.Student ? (
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.EMPTY_TRANSACTIONS.STUDENT_DESCRIPTION)}</PStyled>) :
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.EMPTY_TRANSACTIONS.TUTOR_DESCRIPTION)}</PStyled>}
            </EmptyTransactionsHeader>
            <EmptyTransactionsIcon />
        </EmptyTransationsContentStyled>
    )
}

const TransactionsHeaderButtonsDesktop = () => {
    const { t } = useTranslation();
    const userInfo = useAppSelector(selectUserRoleId);

    return (
        <TransactionHeaderButtonsBoxStyled>
            {userInfo !== UserRoleEnum.Student ? (
                <Link href={AppTransactionService.getTaxExportPath()}>
                    <DownloadSecondaryButtonStyled>
                        <DownloadIcon />
                        {t(TRANSLATION_KEYS.DASHBOARD.DOWNLOAD_TAX_RETURN)}
                    </DownloadSecondaryButtonStyled>
                </Link>
            ) : <></>}
            <Link href={AppTransactionService.getExportAllPath()}>
                <DownloadPrimaryButtonStyled>
                    <DownloadIcon />
                    {t(TRANSLATION_KEYS.DASHBOARD.DOWNLOAD_ALL)}
                </DownloadPrimaryButtonStyled>
            </Link>
        </TransactionHeaderButtonsBoxStyled>
    )
}

const TransactionsHeaderButtonsMobile = () => {
    const { t } = useTranslation();
    const userRoleId = useAppSelector(selectUserRoleId);

    return (
        <ThreeDotsMenu
            innerMenu={
                <MenuBoxStyled>
                    <MenuLinkItem href={AppTransactionService.getExportAllPath()} text={t(TRANSLATION_KEYS.DASHBOARD.DOWNLOAD_ALL)} icon={<DownloadIcon />} />
                    {userRoleId !== UserRoleEnum.Student ? (<MenuLinkItem href={AppTransactionService.getTaxExportPath()} text={t(TRANSLATION_KEYS.DASHBOARD.DOWNLOAD_TAX_RETURN)} icon={<DownloadIcon />} />) : <></>}
                </MenuBoxStyled>
            }
            autoClose={true}
        />
    )
}

const TransactionsHeader = () => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    return (
        <TransactionHeaderBoxStyled>
            <H4Styled>{t(TRANSLATION_KEYS.DASHBOARD.TRANSACTIONS)}</H4Styled>
            {screenType === "desktop" ? <TransactionsHeaderButtonsDesktop /> : <></>}
            {screenType === "mobile" ? <TransactionsHeaderButtonsMobile /> : <></>}
        </TransactionHeaderBoxStyled >
    )
}

const Transactions = () => {
    const transactions = useAppSelector(selectTransactionFilter);
    const transactionsStatus = useAppSelector(selectTransactionFilterStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        fetchNext(0, AppendTypeEnum.None);
    }, []);

    const fetchNext = (newSkip: number, appendType: AppendTypeEnum = AppendTypeEnum.End): Promise<any> => {
        return dispatch(setTransactionFilter({
            appendType: appendType,
            request: {
                skip: newSkip,
                take: COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW
            }
        }));
    }

    return (
        <Box>
            <TransactionsHeader />
            <Box sx={{ display: "flex", overflow: "auto", width: "100%", ">div": { width: "100%", overflow: "auto" } }}>
                {transactionsStatus === LoadingStatusEnum.Error ? <TransactionsError /> : <></>}
                {!transactions ? <TrasactionsLoading /> : <></>}
                {transactions && transactions.size ? <TransactionsLoaded transactions={transactions} fetchNext={fetchNext} /> : <></>}
                {transactions && !transactions.size ? <EmptyTransactions /> : <></>}
            </Box>
        </Box>
    )
}

export default Transactions;
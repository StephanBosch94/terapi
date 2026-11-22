import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/system/Box";
import Tooltip from "components/styles/Tooltip";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";
import InfoIcon from "assets/info.svg";
import { useTranslation } from "components/core/TranslationProvider";
import { PrimaryButtonCompacted } from "components/styles/PrimaryButtonCompacted";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import {
    GetUserWalletStatisticsResponse,
    selectUserWalletStatistics,
    selectUserWalletStatisticsStatus,
    setUserWalletStatistics
} from "core/redux/userWalletSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import { useEffect } from "react";
import { selectUserRoleId } from "core/redux/authSlice";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import DepositMoneyModalProvider, { useDepositMoneyModalContext } from "components/modals/DepositMoneyModalProvider";
import WithdrawMoneyModalProvider, { useWithdrawMoneyModalCotext } from "components/modals/WithdrawMoneyProvider";
import { SecondaryButtonCompacted } from "components/styles/SecondaryButtonCompacted";
import StatisticsColumnBoxStyled from "components/styles/boxes/StatisticsColumnBoxStyled";
import { selectShortUserInfo } from "core/redux/userSlice";
import { usePriceLocalization } from "core/hooks/priceHook";

const WalletContentBox = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "16px",
    display: "flex",
    flexDirection: "column",
    gap: "12px",
    borderRadius: "4px",
    minHeight: "160px",
    height: "100%"
})) as typeof Box;

const WalletColumnBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: { xs: "column", md: "row" },
    gap: { xs: "24px", md: "40px" }
})) as typeof Box;

const TitleBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "4px"
}) as typeof Box;

const InfoIconStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.BLACK_60,
        ":hover": {
            color: COLORS.BLACK,
            cursor: "pointer"
        }
    }
}) as typeof Box;

const PrimaryButtonStyled = styled(PrimaryButtonCompacted)(() => sx({
    width: "fit-content",
    marginTop: { xs: "24px", md: "32px" }
})) as typeof PrimaryButtonCompacted;


const SecondaryButtonStyled = styled(SecondaryButtonCompacted)(() => sx({
    width: "fit-content",
    marginTop: { xs: "24px", md: "32px" }
})) as typeof SecondaryButtonCompacted;

const WalletError = () => {

    return (
        <WalletContentBox>
            <ErrorLarge />
        </WalletContentBox>
    )
}

const WalletLoading = () => {

    return (
        <WalletContentBox sx={{ alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom />
        </WalletContentBox>
    )
}

const StatisticsBox = ({ amount, titleKey, tooltipKey }: { amount: number, titleKey: string, tooltipKey: string }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();

    return (
        <StatisticsColumnBoxStyled>
            <TitleBoxStyled>
                <Body1Styled>{t(titleKey)}</Body1Styled>
                <Tooltip text={t(tooltipKey)}>
                    <InfoIconStyled><InfoIcon /></InfoIconStyled>
                </Tooltip>
            </TitleBoxStyled>
            <H3Styled>
                {localize({ price: amount, round: false })}
            </H3Styled>
        </StatisticsColumnBoxStyled>
    )
}

const IncomeWallet = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { t } = useTranslation();
    const { setOpen } = useWithdrawMoneyModalCotext();

    return (
        <WalletContentBox>
            <WalletColumnBoxStyled>
                <StatisticsBox
                    amount={userWalletStatistics.amount ?? 0}
                    titleKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.AVAILABLE_BALANCE}
                    tooltipKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.AVAILABLE_BALANCE_TOOLTIP}
                />

                <StatisticsBox
                    amount={userWalletStatistics.scheduledAmount ?? 0}
                    titleKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.SCHEDULED_BALANCE}
                    tooltipKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.SCHEDULED_BALANCE_TOOLTIP}
                />
            </WalletColumnBoxStyled>
            <PrimaryButtonStyled onClick={setOpen}>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WITHDRAW_MONEY)}</PrimaryButtonStyled>
        </WalletContentBox>
    )
}

const IncomeWalletWithContexts = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {

    return (
        <WithdrawMoneyModalProvider
            bankAccountNumber={userWalletStatistics.bankAccountNumber}
            bankCode={userWalletStatistics.bankCode}
            balance={userWalletStatistics.amount}
        >
            <IncomeWallet userWalletStatistics={userWalletStatistics} />
        </WithdrawMoneyModalProvider>
    )
}

const RegularWallet = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const { t } = useTranslation();
    const { setOpen: setDepositMoneyModalOpen } = useDepositMoneyModalContext();
    const { setOpen: setWithdrawModelModalOpen } = useWithdrawMoneyModalCotext();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <WalletContentBox>
            <WalletColumnBoxStyled>
                <StatisticsBox
                    amount={userWalletStatistics.amount ?? 0}
                    titleKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.AVAILABLE_BALANCE}
                    tooltipKey={TRANSLATION_KEYS.TUTOR_DASHBOARD.AVAILABLE_BALANCE_TOOLTIP}
                />
            </WalletColumnBoxStyled>
            {!shortUserInfo?.inviterId ? (
                <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
                    <PrimaryButtonStyled onClick={setDepositMoneyModalOpen}>{t(TRANSLATION_KEYS.STUDENT_DASHBOARD.DEPOSIT_MONEY)}</PrimaryButtonStyled>
                    <SecondaryButtonStyled onClick={setWithdrawModelModalOpen}>{t(TRANSLATION_KEYS.TUTOR_DASHBOARD.WITHDRAW_MONEY)}</SecondaryButtonStyled>
                </Box>
            ) : <></>}
        </WalletContentBox >
    )
}

const RegularWalletWithContext = ({ userWalletStatistics }: { userWalletStatistics: GetUserWalletStatisticsResponse }) => {

    return (
        <DepositMoneyModalProvider>
            <WithdrawMoneyModalProvider
                bankAccountNumber={userWalletStatistics.bankAccountNumber}
                bankCode={userWalletStatistics.bankCode}
                balance={userWalletStatistics.amount}
            >
                <RegularWallet userWalletStatistics={userWalletStatistics} />
            </WithdrawMoneyModalProvider>
        </DepositMoneyModalProvider>
    )
}

const Wallet = ({ userRole, userWalletStatistics }: { userRole: UserRoleEnum, userWalletStatistics: GetUserWalletStatisticsResponse }) => {
    const incomeUserRoleIds = [UserRoleEnum.Admin, UserRoleEnum.Tutor];

    return incomeUserRoleIds.includes(userRole)
        ? <IncomeWalletWithContexts userWalletStatistics={userWalletStatistics} />
        : <RegularWalletWithContext userWalletStatistics={userWalletStatistics} />
}

const WalletViews = () => {
    const dispatch = useAppDispatch();
    const userWalletStatistics = useAppSelector(selectUserWalletStatistics);
    const userWalletStatisticsStatus = useAppSelector(selectUserWalletStatisticsStatus);
    const userRole = useAppSelector(selectUserRoleId);

    useEffect(() => {
        dispatch(setUserWalletStatistics());
    }, []);

    return (
        <>
            {userWalletStatisticsStatus === LoadingStatusEnum.Error ? <WalletError /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Loading ? <WalletLoading /> : <></>}
            {userWalletStatisticsStatus === LoadingStatusEnum.Loaded && userWalletStatistics && userRole ? (
                <Wallet userRole={userRole} userWalletStatistics={userWalletStatistics} />
            ) : <></>}
        </>
    )
}

export default WalletViews;
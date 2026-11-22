import Box from "@mui/material/Box";
import { useTranslation } from "./core/TranslationProvider";
import { PrimaryButton } from "./styles/PrimaryButton";
import PStyled from "./styles/typography/PStyled";
import CheckIcon from "assets/check-circle-fill.svg";
import { COLORS } from "./styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import CustomBoxWithoutColorStyled from "./styles/boxes/CustomBoxWithoutColorStyled";
import H3Styled from "./styles/typography/H3Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import router from "next/router";
import routes from "routes/routes";
import PartyIcon from "assets/emoji/party_popper.svg";


const PaymentBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    gap: "24px",
    justifyContent: "center",
    flexFlow: "column noWrap",
})) as typeof Box;

const ContentBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "column noWrap",
})) as typeof Box;

const HeaderBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "row"
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY,
        width: "60px",
        height: "60px"
    }
})) as typeof Box;


const SuccessPaymentBoxStyled = () => {
    const { t } = useTranslation();

    const handleClick = () => {
        router.push(routes.studentDashboard.path);
    };

    return (
        <CustomBoxWithoutColorStyled>
            <PaymentBoxStyled>
                <IconBoxStyled><CheckIcon /></IconBoxStyled>
                <ContentBoxStyled>
                    <HeaderBoxStyled>
                        <PartyIcon />
                        <H3Styled>{t(TRANSLATION_KEYS.PAYMENT.SUCCESS_TITLE)}</H3Styled>
                        <PartyIcon />
                    </HeaderBoxStyled>
                    <PStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.PAYMENT.SUCCESS_DESCRIPTION)}</PStyled>
                </ContentBoxStyled>
                <PrimaryButton onClick={handleClick}>{t(TRANSLATION_KEYS.PAYMENT.SUCCESS_BUTTON)}</PrimaryButton>
            </PaymentBoxStyled>
        </CustomBoxWithoutColorStyled>

    )
};
export default SuccessPaymentBoxStyled;
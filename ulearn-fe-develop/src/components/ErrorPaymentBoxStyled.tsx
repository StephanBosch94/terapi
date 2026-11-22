import Box from "@mui/material/Box";
import { useTranslation } from "./core/TranslationProvider";
import { PrimaryButton } from "./styles/PrimaryButton";
import PStyled from "./styles/typography/PStyled";
import ErrorIcon from "assets/x-circle-fill.svg";
import { COLORS } from "./styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import CustomBoxWithoutColorStyled from "./styles/boxes/CustomBoxWithoutColorStyled";
import H3Styled from "./styles/typography/H3Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import router from "next/router";
import routes from "routes/routes";


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

const IconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.RED,
        width: "60px",
        height: "60px"
    }
})) as typeof Box;


const ErrorPaymentBoxStyled = () => {
    const { t } = useTranslation();

    const handleClick = () => {
        // TODO send user one step before payment
    };

    return (
        <CustomBoxWithoutColorStyled>
            <PaymentBoxStyled>
                <IconBoxStyled><ErrorIcon /></IconBoxStyled>
                <ContentBoxStyled>
                    <H3Styled>{t(TRANSLATION_KEYS.PAYMENT.ERROR_TITLE)}</H3Styled>
                    <PStyled>{t(TRANSLATION_KEYS.PAYMENT.ERROR_DESCRIPTION)}</PStyled>
                </ContentBoxStyled>
                <PrimaryButton onClick={handleClick}>{t(TRANSLATION_KEYS.PAYMENT.ERROR_BUTTON)}</PrimaryButton>
            </PaymentBoxStyled>
        </CustomBoxWithoutColorStyled>

    )
};
export default ErrorPaymentBoxStyled;
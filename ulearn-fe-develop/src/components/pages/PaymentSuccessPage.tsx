import Box from "@mui/system/Box";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import FunIcon from "assets/emoji/fun.svg";
import CheckCircleIcon from "assets/check-circle.svg";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H2StyledComponent from "components/styles/typography/H2Styled";
import { useTranslation } from "components/core/TranslationProvider";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import Link from "next/link";
import routes from "routes/routes";
import ConfettiBox from "components/styles/boxes/ConfettiBox";
import { useContext, useEffect, useState } from "react";
import { UIContext } from "components/core/UIContextProvider";

const CheckIconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    color: COLORS.PRIMARY,
    paddingBottom: "16px",
    svg: {
        transform: "scale(4.5)",
    }
})) as typeof Box;

const DescriptionBoxStyled = styled(Box)(() => ({
    padding: "0px 16px",
    display: "inline-flex",
    gap: "8px",
    alignItems: "center",
    textAlign: "center"
})) as typeof Box;

const DescriptionTypographyStyled = styled(PStyled)(() => ({
    color: COLORS.BLACK
})) as typeof PStyled;

type PageType = "successfull_reservation_payment" | "successfull_money_refill";

type PageStateType = {
    type: PageType,
    titleKey: string,
    descriptionKey: string,
    buttonKey: string
}

const pageStates: PageStateType[] = [
    {
        type: "successfull_reservation_payment",
        titleKey: TRANSLATION_KEYS.SUCCESSFULL_RESERVATION_PAYMENT.TITLE,
        descriptionKey: TRANSLATION_KEYS.SUCCESSFULL_RESERVATION_PAYMENT.DESCRIPTION,
        buttonKey: TRANSLATION_KEYS.SUCCESSFULL_RESERVATION_PAYMENT.BUTTON
    },
    {
        type: "successfull_money_refill",
        titleKey: TRANSLATION_KEYS.SUCCESSFULL_MONEY_REFILL.TITLE,
        descriptionKey: TRANSLATION_KEYS.SUCCESSFULL_MONEY_REFILL.DESCRIPTION,
        buttonKey: TRANSLATION_KEYS.SUCCESSFULL_MONEY_REFILL.BUTTON
    }
]

const PaymentSuccessPage = ({ type }: { type: PageType }) => {
    const [showConfetti, setShowConfetti] = useState(true);
    const state = pageStates.find(x => x.type === type) ?? pageStates[0];
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    useEffect(() => {
        setTimeout(() => {
            setShowConfetti(false);
        }, 3000);
    }, []);

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", overflow: "auto", height: "100%" }}>
            {showConfetti ? <ConfettiBox gravity={0.4} /> : <></>}
            <Box sx={{ display: "flex", flex: "0 1 auto", width: "100%" }}>
                <Navbar source={NavbarSourceEnum.studentPage} />
            </Box>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", flex: "1 1 auto", justifyContent: "center", alignItems: "center", gap: "24px" }}>
                <CheckIconBoxStyled><CheckCircleIcon /></CheckIconBoxStyled>
                <Box sx={{ display: "flex", flexFlow: "column noWrap", justifyContent: "center", alignItems: "center", gap: "16px" }}>
                    <H2StyledComponent>{t(state.titleKey)}</H2StyledComponent>
                    <DescriptionBoxStyled>
                        {screenType === "desktop" ? <FunIcon /> : <></>}
                        <DescriptionTypographyStyled>{t(state.descriptionKey)}</DescriptionTypographyStyled>
                        {screenType === "desktop" ? <FunIcon /> : <></>}
                    </DescriptionBoxStyled>
                </Box>
                <Link href={routes.home.path}><PrimaryButton>{t(state.buttonKey)}</PrimaryButton></Link>
            </Box>
        </Box>
    )
}

export default PaymentSuccessPage;
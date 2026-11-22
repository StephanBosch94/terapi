import CookiesIcon from "assets/emoji/cookies.svg";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useEffect, useState } from "react";
import { PrimaryButtonRectangle } from "../styles/PrimaryButtonRectangle";
import { SecondaryButtonRectangle } from "../styles/SecondaryButtonRectangle";
import { COLORS } from "../styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import { getStorage } from "core/services/Core/StorageService";
import { cacheConstants } from "core/constants/cacheConstants";
import Typography from "@mui/material/Typography";
import { ContainerStyled } from "components/styles/ContainerStyled";

const IconBoxStyled = styled(Box)(() => ({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    width: "58px",
    height: "58px",
    svg: {
        transform: "scale(3)"
    }
})) as typeof Box;

const ModalBoxStyled = styled(Box)(() => sx({
    position: "fixed",
    zIndex: "1200",
    bottom: '0%',
    left: '0%',
    width: "100%",
    backgroundColor: COLORS.PRIMARY,
    boxShadow: 24,
})) as typeof Box;

const ContentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    flexFlow: { xs: "column ", md: "row" },
    justifyContent: "center",
    gap: { xs: "16px", md: "64px" },
    padding: { xs: "24px", md: "40px" }
})) as typeof Box;

const ButtonBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "16px",
    flexFlow: "column",
    minWidth: "200px",
    width: "100%"
})) as typeof Box;

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
    flexFlow: "column noWrap",
    textAlign: { xs: "center", md: "left" }
})) as typeof Box;

const DescriptionTextStyled = styled(Body1Styled)(() => sx({
    fontSize: {xs: "0.75rem", md: "0.875rem"},
    color: COLORS.WHITE,
    a: {
        color: COLORS.WHITE
    }
})) as typeof Body1Styled;

const PrimaryButtonRectangleStyled = styled(PrimaryButtonRectangle)(() => sx({
    border: `1px solid ${COLORS.WHITE}`
})) as typeof PrimaryButtonRectangle;

type CookiesType = "all" | "essential_only";

const CookiesModal = () => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);

    useEffect(() => {
        checkCookies();
    }, []);

    const checkCookies = async () => {
        const storage = await getStorage();
        const cookiesValue = await storage.getCommonValue<string>(cacheConstants.cookies);
        if (!cookiesValue) {
            setOpen(true);
        }
    }

    const handleClick = async (type: CookiesType) => {
        const storage = await getStorage();
        await storage.setCommonValue(cacheConstants.cookies, type);
        setOpen(false);
    }

    return (
        <>
            {open ? (
                <ModalBoxStyled>
                    <ContainerStyled>
                        <ContentBoxStyled>
                            <IconBoxStyled><CookiesIcon /></IconBoxStyled>
                            <HeadingBoxStyled>
                                <Typography sx={{ color: COLORS.WHITE, fontWeight: "600", fontSize: { xs: "1rem", md: "1.25rem" } }}>{t(TRANSLATION_KEYS.COOKIES.TITLE)}</Typography>
                                <DescriptionTextStyled sx={{ color: COLORS.WHITE }}
                                    dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.COOKIES.DESCRIPTION) }}
                                />
                            </HeadingBoxStyled>
                            <ButtonBoxStyled>
                                <SecondaryButtonRectangle onClick={() => handleClick("all")}>{t(TRANSLATION_KEYS.COOKIES.ACCEPT_ALL)}</SecondaryButtonRectangle>
                                <PrimaryButtonRectangleStyled onClick={() => handleClick("essential_only")}>{t(TRANSLATION_KEYS.COOKIES.JUST_ESSENTIAL)}</PrimaryButtonRectangleStyled>
                            </ButtonBoxStyled>
                        </ContentBoxStyled>
                    </ContainerStyled>
                </ModalBoxStyled>
            ) : <></>}
        </>
    )
}

export default CookiesModal;
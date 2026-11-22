import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import Link from "next/link";
import AppLogo from "assets/logo.svg";
import React from "react";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import FacebookIcon from "assets/facebook.svg";
import InstagramIcon from "assets/instagram.svg";
import HeartIcon from "assets/heart.svg";
import Body1Styled from "components/styles/typography/Body1Styled";
import { useAppDataContext } from "components/core/providers/AppProvider";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Divider from "@mui/material/Divider";
import routes, { staticRoutes } from "routes/routes";
import PStyled from "components/styles/typography/PStyled";
import { ContainerStyled } from "components/styles/ContainerStyled";
import TrustPilotWidget from "components/styles/TrustPilotWidget";

const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: { xs: "60px 0px 20px 0px", md: "60px 0px" }
})) as typeof Box;

const GroupTitleStyled = styled(PStyled)(() => sx({
    marginBottom: "16px",
    fontWeight: 600,
    color: COLORS.BLACK
})) as typeof PStyled;

const LinkTextStyled = styled(Body1Styled)(() => sx({
    marginBottom: "16px",
    textDecoration: "none !important",
    color: `${COLORS.BLACK} !important`,
    ":hover": {
        cursor: "pointer",
        textDecoration: "underline !important",
    }
})) as typeof Body1Styled;


const IconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    padding: "4px",
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    svg: {
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const StyledGrid = styled(Grid)(() => sx({
    display: "flex",
    flexDirection: "rows"
})) as typeof Grid;

const SocialMediaBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "24px",
    height: "auto",
    paddingLeft: "4px",
    justifyContent: {xs:"center", md:"end"},
    minWidth: "220px",
})) as typeof Box;

const CompanyInfoBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column",
    gap: "16px"
})) as typeof Box;

const SocialMediaIconStyled = styled(Box)(() => sx({
    transform: "scale(1.5)",
    color: COLORS.PRIMARY,
    ":hover": {
        cursor: "pointer",
        color: COLORS.PRIMARY_HOVER,
    }
})) as typeof Box;

const Body1StyledBlack = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK,
})) as typeof Body1Styled;

const Footer = ({ bgColor }: { bgColor: string }) => {
    const { t } = useTranslation();
    const TextDecorationStyled = { textDecoration: "none", ":hover": { color: COLORS.BLACK, textDecoration: "underline" }};
    const { subjects } = useAppDataContext()
    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <ContainerStyled>
                <StyledGrid container columns={12} spacing={4}>
                    <Grid item xs={6} md={3}>
                        <Box sx={{ display: "inline-flex", flexDirection: "column", justifyContent: "space-between", height: "100%", paddingBottom: "12px" }}>
                            <Box>
                                <Box sx={{ marginBottom: "6px", paddingTop: "4px" }}>
                                    <Link href={routes.home.path} aria-label="Yaspeak"><AppLogo /></Link>
                                </Box>
                                <CompanyInfoBoxStyled>
                                    <Body1StyledBlack>{t(TRANSLATION_KEYS.FOOTER.COMPANY)}</Body1StyledBlack>
                                    <Body1StyledBlack>{t(TRANSLATION_KEYS.FOOTER.IC)}</Body1StyledBlack>
                                    <Body1StyledBlack>{t(TRANSLATION_KEYS.FOOTER.CONTACT_EMAIL)}</Body1StyledBlack>
                                    <Body1StyledBlack>{t(TRANSLATION_KEYS.FOOTER.ADDRESS)}</Body1StyledBlack>
                                </CompanyInfoBoxStyled>

                            </Box>
                        </Box>
                    </Grid>
                    <Grid item xs={6} md={3}>
                        <GroupTitleStyled>{t(TRANSLATION_KEYS.FOOTER.LANGUAGE_TEACHERS)}</GroupTitleStyled>
                        {subjects.map((subject, idx) =>
                            <Link key={subject.subjectId!} href={`${routes.search.path}?subjectId=${subject.subjectId}`} style={TextDecorationStyled}>
                                <LinkTextStyled>{t(subject.footerSubjectTeachersPageLinkKey ?? "")}</LinkTextStyled>
                            </Link>)}
                    </Grid>
                    <Grid item xs={6} md={3}>
                        <GroupTitleStyled>{t(TRANSLATION_KEYS.FOOTER.LEARN_A_LANGUAGE)}</GroupTitleStyled>
                        {subjects.map((subject, idx) =>
                            <Link key={subject.subjectId!} href={routes.subjectLandingPage.path(subject.enumKey ?? "")} style={TextDecorationStyled}>
                                <LinkTextStyled>{t(subject.footerSubjectLandingPageLinkKey ?? "")}</LinkTextStyled>
                            </Link>)}
                    </Grid>
                    <Grid item xs={6} md={3}>
                        <Box sx={{ display: "flex", justifyContent: "space-between", flexDirection: "column", height: "100%" }}>
                            <Box>
                                <GroupTitleStyled>{t(TRANSLATION_KEYS.FOOTER.OTHERS)}</GroupTitleStyled>
                                <Link href={staticRoutes.termsOfUse} style={TextDecorationStyled}><LinkTextStyled>{t(TRANSLATION_KEYS.FOOTER.TERMS_OF_USE)}</LinkTextStyled></Link>
                                <Link href={staticRoutes.gdpr} style={TextDecorationStyled}><LinkTextStyled>{t(TRANSLATION_KEYS.FOOTER.PRIVACY_POLICY)}</LinkTextStyled></Link>
                                <Link href={staticRoutes.cookies} style={TextDecorationStyled}><LinkTextStyled>{t(TRANSLATION_KEYS.FOOTER.COOKIES)}</LinkTextStyled></Link>
                                <Link href={t(TRANSLATION_KEYS.FOOTER.HUBSPOT_FORM)} style={TextDecorationStyled}>
                                    <LinkTextStyled>{t(TRANSLATION_KEYS.FOOTER.REPORT_PROBLEM)}</LinkTextStyled>
                                </Link>
                            </Box>
                        </Box>
                    </Grid>
                </StyledGrid>
                <Divider sx={{ margin: { xs: "20px 0px", md: "60px 0px", borderColor: COLORS.BLACK_10 } }} />
                <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", flexFlow: { xs: "column noWrap", md: "row noWrap", gap: "24px" } }}>
                    <TrustPilotWidget />
                    <SocialMediaBoxStyled>
                        <Link href={staticRoutes.facebook} target="_blank" title="Yaspeak Facebook">
                            <SocialMediaIconStyled><FacebookIcon /></SocialMediaIconStyled>
                        </Link>
                        {/* <SocialMediaIconStyled><TwitterIcon /></SocialMediaIconStyled>
                        <SocialMediaIconStyled><YoutubeIcon /></SocialMediaIconStyled> */}
                        <Link href={staticRoutes.instagram} target="_blank" title="Yaspeak Instagram">
                            <SocialMediaIconStyled><InstagramIcon /></SocialMediaIconStyled>
                        </Link>
                    </SocialMediaBoxStyled>
                </Box>
            </ContainerStyled>
        </BackgroundBoxStyled>
    )
}
export default Footer;
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useRef, useContext, useEffect, useState, useMemo } from "react";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import Image from 'next/image'
import AuthModalProvider from "components/modals/AuthModalProvider";
import routes from "routes/routes";
import { motion, useScroll, useTransform } from "framer-motion";
import { UIContext } from "components/core/UIContextProvider";
import { useRouter } from "next/router";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import ArrowIcon from "assets/arrow-right.svg";
import Typography from "@mui/material/Typography";
import { ContainerStyled } from "components/styles/ContainerStyled";
import Link from "next/link";

const Spacing = { xs: "16px", md: "60px" }
const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: { xs: "32px 0px", md: "80px 0px" }
})) as typeof Box;

const TopHeroSectionStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: { xs: "left", md: "center" },
    height: "100%",
    gap: { xs: "16px", md: "24px" },
    paddingRight: { xs: "0px", md: "40px" }
})) as typeof Box;

const AnimationContainerBoxStyled = styled(Typography)(() => sx({
    fontWeight: "800 !important",
    fontSize: { xs: "2rem", md: "3rem" },
    textAlign: { xs: "left", md: "center" },
    lineHeight: { xs: "3.2rem", md: "4.2rem" },
    transition: "all 1s ease",
    "> *": {
        width: "fit-content",
    },
    ".box": {
        overflow: "hidden",
        height: { xs: "2.5rem", md: "3.5rem" },
    },
    "b": {
        transition: "all 1s ease",
        borderRight: `.12em solid ${COLORS.PRIMARY}`, /* The typwriter cursor */
        whiteSpace: "nowrap", /* Keeps the content on a single line */
        color: COLORS.PRIMARY,
        fontWeight: "800 !important",
        fontSize: { xs: "2rem", md: "3rem" },
        "> .underline": {
            color: COLORS.BACKGROUND,
        },
    },
    ".selected": {
        backgroundColor: COLORS.PRIMARY_10
    }
})) as typeof Typography;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    animation: "fade-up 0.5s",
    display: "flex",
    gap: "24px",
    flexDirection: "column",
})) as typeof ContainerStyled;

const ImageSectionGridStyled = styled(Grid)(() => sx({
    display: { xs: "inline-flex", md: "inline-flex" },
    alignItems: "flex-start",
    gap: { xs: "16px", md: "40px" },
    width: "100%",
})) as typeof Grid;

const motionBoxStyles = {
    flex: "1 1 auto",
    display: "flex"
}

const MotionBoxStyled = styled(Box)({ ...motionBoxStyles });

const texts = [
    TRANSLATION_KEYS.STUDENT_HERO.ANIMATION_TEXT_ONE,
    TRANSLATION_KEYS.STUDENT_HERO.ANIMATION_TEXT_TWO,
    TRANSLATION_KEYS.STUDENT_HERO.ANIMATION_TEXT_THREE,
    TRANSLATION_KEYS.STUDENT_HERO.ANIMATION_TEXT_FOUR
];

const imageDimensions = {
    width: 258,
    height: 520
}

const imageDimensionsMobile = {
    width: 103,
    height: 177
}

type AnimationStateType = "typing" | "deletion";
const AnimationSettings = {
    typingSpeed: 200,
    deletingSpeed: 1000
}

const AnimatedText = () => {
    const { t } = useTranslation();
    const [index, setIndex] = useState(0);
    const [text, setText] = useState(t(texts[0]));
    const [state, setState] = useState<AnimationStateType>("typing")
    const { locale } = useRouter();
    const translations = useMemo(() => texts.map(x => t(x)), [locale]);
    const { screenType } = useContext(UIContext);
    const titleKey = screenType === "desktop" ? TRANSLATION_KEYS.STUDENT_HERO.TITLE : TRANSLATION_KEYS.STUDENT_HERO.MOBILE_TITLE;

    useEffect(() => {
        switch (state) {
            case "typing": {
                if (text.length === translations[index].length) {
                    setState("deletion");
                    return;
                }
                const timeout = setTimeout(() => {
                    setText(translations[index].slice(0, text.length + 1))
                }, AnimationSettings.typingSpeed);

                return () => clearTimeout(timeout);
            }
            case "deletion": {
                const timeout = setTimeout(() => {
                    setText("");
                    setIndex(prev => prev < translations.length - 1 ? prev + 1 : 0);
                    setState("typing");
                }, AnimationSettings.deletingSpeed);

                return () => clearTimeout(timeout);
            }
            default:
                return;
        }
    }, [index, text, state, translations]);

    return (
        <AnimationContainerBoxStyled variant="h1" dangerouslySetInnerHTML={{ __html: (t(titleKey, { language: `<b class="${state === "deletion" ? "selected" : ""}">${text}</b>` })) }} />)
}

const StudentHeroHeader = () => {
    const { t } = useTranslation();

    return (
        <Grid item xs={12} md={12} padding="0" >
            <TopHeroSectionStyled>
                <AnimatedText />
                <PStyled sx={{ textAlign: { xs: "left", md: "center" }, maxWidth: "920px", lineHeight: "2rem" }}>{t(TRANSLATION_KEYS.STUDENT_HERO.DESCRIPTION)}</PStyled>
                <Link href={routes.search.path} style={{ textDecoration: "none" }}><PrimaryButton>{t(TRANSLATION_KEYS.STUDENT_HERO.GET_STARTED)} <ArrowIcon /></PrimaryButton></Link>
            </TopHeroSectionStyled>
        </Grid>
    )
}

const StudentHeroImage = ({ src, blurDataUrl, height, width }: { src: string, blurDataUrl: string, height: number, width: number }) => {
    return (
        <Image src={src} placeholder="blur" blurDataURL={blurDataUrl} height={height} width={width} quality={75} priority={true} alt="User's image" style={{ height: "auto", width: "100%" }} />
    )
}

const MobileStudentHero = () => {

    return (
        <BackgroundBoxStyled >
            <ContainerCustomStyled>
                <Grid container padding="0" columns={12} sx={{ display: "flex", flexDirection: { xs: "column", md: "row" }, justifyContent: "center", gap: Spacing }}>
                    <StudentHeroHeader />
                    {/* <ImageSectionGridStyled item xs={12} md={12} >
                        <StudentHeroImage src="/images/hero_yellow_mobile.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.YELLOW} height={imageDimensionsMobile.height} width={imageDimensionsMobile.width} />
                        <StudentHeroImage src="/images/hero_blue_mobile.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.BLUE} height={imageDimensionsMobile.height} width={imageDimensionsMobile.width} />
                        <StudentHeroImage src="/images/hero_green_mobile.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.GREEN} height={imageDimensionsMobile.height} width={imageDimensionsMobile.width} />
                    </ImageSectionGridStyled> */}
                </Grid>
            </ContainerCustomStyled>
        </BackgroundBoxStyled>

    )
}

const DesktopStudentHero = () => {
    const backgroundRef = useRef(null);
    const { scrollYProgress } = useScroll({
        target: backgroundRef,
        offset: ["end end", "end start"]
    });

    const marginIncrease = useTransform(scrollYProgress, [0.3, 0.7], [0, 180]);

    const dimensions = imageDimensions;

    return (
        <BackgroundBoxStyled ref={backgroundRef}>
            <ContainerCustomStyled>
                <Grid container padding="0" columns={12} sx={{ display: "flex", flexDirection: { xs: "column", md: "row" }, justifyContent: "center", gap: Spacing }}>
                    <StudentHeroHeader />
                    <ImageSectionGridStyled item xs={12} md={12} >
                        <MotionBoxStyled sx={{ height: dimensions.height, width: dimensions.height, display: { xs: "none", md: "flex" } }}>
                            <StudentHeroImage src="/images/hero_yellow.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.YELLOW} height={imageDimensions.height} width={imageDimensions.width} />
                        </MotionBoxStyled>
                        <motion.div style={{ ...motionBoxStyles, height: dimensions.height, width: dimensions.height, marginTop: marginIncrease }}>
                            <StudentHeroImage src="/images/hero_blue.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.BLUE} height={imageDimensions.height} width={imageDimensions.width} />
                        </motion.div>
                        <MotionBoxStyled sx={{ height: dimensions.height, width: dimensions.height }}>
                            <StudentHeroImage src="/images/hero_green.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.GREEN} height={imageDimensions.height} width={imageDimensions.width} />
                        </MotionBoxStyled>
                        <motion.div style={{ ...motionBoxStyles, height: dimensions.height, width: dimensions.height, marginTop: marginIncrease }}>
                            <StudentHeroImage src="/images/hero_red.png" blurDataUrl={LOADING_IMAGES.STUDENT_HERO.RED} height={imageDimensions.height} width={imageDimensions.width} />
                        </motion.div>
                    </ImageSectionGridStyled>
                </Grid>
            </ContainerCustomStyled>
        </BackgroundBoxStyled>
    )
}

const StudentHero = () => {
    const { screenType } = useContext(UIContext);

    return (
        <>
            {screenType === "desktop" ? <DesktopStudentHero /> : <></>}
            {screenType === "mobile" ? <MobileStudentHero /> : <></>}
        </>
    )
}

const StudentHeroWithProviders = () => (
    <AuthModalProvider>
        <StudentHero />
    </AuthModalProvider>
)

export default StudentHeroWithProviders;
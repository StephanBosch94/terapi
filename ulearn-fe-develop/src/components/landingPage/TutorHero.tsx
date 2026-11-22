import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React, { useCallback, useState } from "react";
import H1Styled from "components/styles/typography/H1Styled";
import PStyled from "components/styles/typography/PStyled";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import usePrevious from "core/services/hooks/usePrevious";
import Image from "next/image";
import Box from "@mui/material/Box";
import Slider from "@mui/material/Slider";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import ArrowIcon from "assets/arrow-right.svg";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { usePriceLocalization } from "core/hooks/priceHook";
import { useAppDataContext } from "components/core/providers/AppProvider";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: { xs: "32px 0px", md: "80px 0px" }
})) as typeof Box;

const LeftHeroSectionStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    height: "100%",
    flex: "1 1 auto",
    width: { xs: "100%", md: "auto" },
    gap: { xs: "16px", md: "24px" },
    paddingRight: { xs: "0px", md: "40px" }
})) as typeof Box;

const PStyledText = styled(PStyled)(() => sx({
    color: COLORS.BLACK
})) as typeof PStyled;

const DescriptionBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "4px",
    "b": {
        fontWeight: 600,
        color: COLORS.BLACK
    }
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    animation: "fade-up 0.5s",
    display: "flex",
    gap: "48px",
    alignItems: { xs: "flex-start", lg: "center" },
    flexDirection: { xs: "column", lg: "row" },
})) as typeof ContainerStyled;

const SliderStyled = styled(Slider)(() => sx({
    color: COLORS.PRIMARY,
    width: { xs: "100%", md: "424px" },
    marginTop: "8px",
    '& .MuiSlider-thumb': {
        height: "32px",
        width: "32px",
        backgroundColor: COLORS.WHITE,
        border: `1px solid ${COLORS.PRIMARY}`,
        boxShadow: "2px 3px 6px rgba(0, 0, 0, 0.15), -1px -1px 6px rgba(0, 0, 0, 0.08)",
        '&:focus, &:hover, &.Mui-active': {
            boxShadow: "none"
        }
    },
    '& .MuiSlider-track': {
        border: 'none',
        height: "4px"
    },
    '& .MuiSlider-rail': {
        opacity: 1,
        backgroundColor: COLORS.PRIMARY_10,
        height: "4px",
    },
})) as typeof Slider;

const AnimatedAmountBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    color: COLORS.PRIMARY,
    fontSize: { xs: "2rem", md: "4rem" },
    lineHeight: { xs: "2rem", md: "4rem" },
    fontWeight: "800 !important",
    ".numbers": {
        display: "flex",
        overflow: "hidden",
        height: { xs: "2rem", md: "4rem" },
    },
    ".numbers span": {
        zIndex: 1,
        display: "flex",
        flexFlow: "column noWrap",
        height: "100%",
        transition: "all 2s ease",
        transform: "translateY(0)",
    },
    ".digit": {
        textAlign: "right",
    }
})) as typeof Box;

const ImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    position: "relative",
    flex: "1 1 auto",
    width: { xs: "100%", lg: "auto" },
    img: {
        minWidth: { xs: "auto", lg: "552px" },
        maxWidth: { xs: "auto", lg: "552px" },
        width: "100%",
        height: "auto"
    }
})) as typeof Box;

const createArray = (val: number) => {
    return val.toString().split('').map(x => parseInt(x));
}

const createNumberArrays = (prevValue: number, newValue: number) => {
    const prevArray = createArray(prevValue);
    const newArray = createArray(newValue);
    const diffArray = Array(Math.abs(prevArray.length - newArray.length)).fill(0);

    return {
        prevArray: prevArray.length < newArray.length ? prevArray.concat(diffArray) : prevArray,
        newArray: newArray
    }
}

function animateNumber(number: number, oldNumber: number, element: HTMLElement): void {
    const { prevArray, newArray } = createNumberArrays(oldNumber, number);

    const html = createNumberHTML(newArray, prevArray);
    const humanReadableHtml = getHumanReadableArray(html);
    element.innerHTML = '';
    element.insertAdjacentHTML("beforeend", humanReadableHtml.join(""));
    const ticks = [...element.querySelectorAll("span[data-value]")] as HTMLElement[];

    setTimeout(() => {
        for (let tick of ticks.filter(x => x.getAttribute("data-value"))) {
            const dist = parseInt(tick.getAttribute("data-value")!) - 1;
            tick.style.transform = `translateY(-${dist * 100}%)`;
        }
    }, 0);
}

const getHumanReadableArray = (html: string[]) => {
    const commaSpan = '<span>,</span>';
    if (html.length <= 3) {
        return html;
    }
    if (html.length === 4) {
        return [html[0], commaSpan, ...html.slice(1, html.length)];
    }
    if (html.length === 5) {
        return [...html.slice(0, 2), commaSpan, ...html.slice(2, html.length)];
    }
    return html;
}

const createNumberHTML = (newNumbers: Array<number | string>, prevNumbers: Array<number | string>) => {
    return newNumbers
        .map((newNumber, i) => calcDeltaBetweenNumbers(Number(prevNumbers[i]), Number(newNumber)))
        .map((deltas) => (`<span data-value="${deltas.length}">${deltas.map(x => `<span class="digit">${x}</span>`).join('')}</span>`));
}

const calcDeltaBetweenNumbers = (prevNumber: number, newNumber: number) => {
    const alteredNew = newNumber < prevNumber ? (newNumber + 10) : newNumber;
    return Array(alteredNew - prevNumber + 1)
        .fill(0)
        .map((_, idx) => (prevNumber + idx) % 10);
}

const getApproximateAmount = (numberOfLesssons: number) => numberOfLesssons * COMMON_CONSTANTS.LP_CALCULATOR.DEFAULT_HOUR_RATE * (1 - COMMON_CONSTANTS.LP_CALCULATOR.COMMISSION_RATE);

const AnimatedAmountBox = ({ prevNumberOfLessons, numberOfLessons }: { prevNumberOfLessons: number | undefined, numberOfLessons: number }) => {
    const { convertToUserCurrency } = usePriceLocalization();
    const { currentCurrency } = useAppDataContext();
    const previousPrice = Math.round(convertToUserCurrency({ price: getApproximateAmount(prevNumberOfLessons ?? 0) }));
    const currentPrice = Math.round(convertToUserCurrency({ price: getApproximateAmount(numberOfLessons ?? 0) }));

    const refCallback = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            animateNumber(currentPrice, previousPrice, node);
        }
    }, [currentPrice, previousPrice]);

    return (
        <AnimatedAmountBoxStyled className="number-box">
            <Box sx={{ display: "inline-flex", alignItems: "center" }}>
                <div ref={refCallback} className="numbers" />
                <div>&#8239;{currentCurrency?.symbol ?? ""}</div>
            </Box>
        </AnimatedAmountBoxStyled>
    )
}

const TutorHero = () => {
    const { t } = useTranslation();
    const { setOpen } = useAuthModalContext();
    const { localize } = usePriceLocalization();
    const [numberOfLessons, setNumberOfLessons] = useState(COMMON_CONSTANTS.LP_CALCULATOR.DEFAULT_NUMBER_OF_HOURS);
    const prevNumberOfLessons = usePrevious(numberOfLessons);

    const handleNumberOfLessonsChange = (event: Event, value: number | Array<number>, activeThumb: number) => {
        setNumberOfLessons(value as number);
    }

    return (
        <React.Fragment>
            <BackgroundBoxStyled>
                <ContainerCustomStyled>
                    <LeftHeroSectionStyled>
                        <H1Styled>{t(TRANSLATION_KEYS.TUTOR_LANDING_PAGE.TITLE)}</H1Styled>
                        <AnimatedAmountBox prevNumberOfLessons={prevNumberOfLessons} numberOfLessons={numberOfLessons} />
                        <DescriptionBoxStyled>
                            <PStyledText dangerouslySetInnerHTML={{ __html: (t(TRANSLATION_KEYS.TUTOR_LANDING_PAGE.DESCRIPTION, { numberOfLessons, price: localize({ price: COMMON_CONSTANTS.LP_CALCULATOR.DEFAULT_HOUR_RATE }) })) }} />
                        </DescriptionBoxStyled>
                        <SliderStyled
                            value={numberOfLessons}
                            min={COMMON_CONSTANTS.LP_CALCULATOR.MIN_NUMBER_OF_HOURS}
                            max={COMMON_CONSTANTS.LP_CALCULATOR.MAX_NUMBER_OF_HOURS}
                            onChange={handleNumberOfLessonsChange}
                        />
                        <PStyledText>{t(TRANSLATION_KEYS.TUTOR_LANDING_PAGE.SECOND_DESCRIPTION)} </PStyledText>
                        <Box sx={{ display: "block" }}>
                            <PrimaryButton onClick={() => setOpen(UserRoleEnum.Tutor, "registration")} sx={{ marginTop: "24px" }}>{t(TRANSLATION_KEYS.TUTOR_HERO_ITEM.START_TEACHING)} <ArrowIcon /></PrimaryButton>
                        </Box>
                    </LeftHeroSectionStyled>
                    <ImageBoxStyled>
                        <Image src="/images/tutor_hero.png" placeholder="blur" blurDataURL={LOADING_IMAGES.TUTOR_HERO.MAIN} width={440} height={424} quality={75} priority={true} alt="Start teaching and earn money on your schedule" />
                    </ImageBoxStyled>
                </ContainerCustomStyled>
            </BackgroundBoxStyled>
        </React.Fragment>
    )
}

const TutorHeroWithProviders = () => {
    return (
        <AuthModalProvider>
            <TutorHero />
        </AuthModalProvider>
    )
}

export default TutorHeroWithProviders;
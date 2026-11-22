import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import React, { useEffect, useRef } from "react";
import Link from "next/link";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppDispatch } from "core/redux/reduxHooks";
import ErrorLarge from "components/core/error/ErrorLarge";
import { ScrollBoxStyled } from "components/styles/boxes/ScrollBoxStyled";
import H2Styled from "components/styles/typography/H2Styled";
import LeftArrowIcon from "assets/carret-left.svg";
import RightArrowIcon from "assets/carret-right.svg";
import H4Styled from "components/styles/typography/H4Styled";
import Box from "@mui/material/Box";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";
import InjectIcon, { IconCategoryType } from "components/core/icons/InjectIcon";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import H5Styled from "components/styles/typography/H5Styled";

type ItemTextSizeType = "small" | "regular";
type DataType = { id: number, enumKey: string, nameKey: string, iconName: string, iconCategory: IconCategoryType, href: string }
export type BrowserPropsType = { bgColor: string, data: DataType[] | undefined, titleKey?: string, linkType?: "link" | "registration", itemTextSize?: ItemTextSizeType }

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.BACKGROUND,
    padding: { xs: "60px 0px", md: "120px 0px" }
})) as typeof Box;

const ItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    justifyContent: "center",
    border: `1px solid ${COLORS.BLACK_10}`,
    backgroundColor: COLORS.WHITE,
    borderRadius: "4px",
    height: "300px",
    width: "280px",
    gap: "12px",
    padding: "12px",
    marginRight: "8px",
    textAlign: "center",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5,
        cursor: "pointer",
        svg: {
            transform: "scale(5)"
        }
    }
})) as typeof Box;

const ItemIconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: { xs: "scale(4)", md: "scale(4.5)" },
        transition: "all 0.3s ease-out"
    }
})) as typeof Box;

const ItemBoxInnerContainer = styled(Box)(() => sx({
    paddingTop: "24px",
    display: "flex",
    justifyContent: "space-between",
    gap: "8px",
    "a": {
        textDecoration: "none",
        color: "inherit"
    }
})) as typeof Box;

const ArrowLeftBoxStyled = styled(Box)(() => sx({
    display: { xs: "none", md: "flex" },
    padding: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    position: "absolute",
    backgroundColor: COLORS.WHITE,
    borderRadius: "50%",
    zIndex: 100,
    top: "calc(50% + 12px)",
    left: "-16px",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5,
        border: `1px solid ${COLORS.PRIMARY}`,
        cursor: "pointer"
    }
})) as typeof Box;

const ArrowRightBoxStyled = styled(Box)(() => sx({
    display: { xs: "none", md: "flex" },
    padding: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    position: "absolute",
    backgroundColor: COLORS.WHITE,
    borderRadius: "50%",
    zIndex: 100,
    top: "calc(50% + 12px)",
    right: "-16px",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_5,
        border: `1px solid ${COLORS.PRIMARY}`,
        cursor: "pointer"
    }
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY
    }
}))


const ItemBox = ({ iconName, nameKey, iconCategory, itemTextSize }: { iconName: string, nameKey: string, iconCategory: IconCategoryType, itemTextSize: ItemTextSizeType }) => {
    const { t } = useTranslation();

    return (
        <>
            <ItemIconBoxStyled>
                <InjectIcon category={iconCategory} iconName={iconName ?? ""} size={72} />
            </ItemIconBoxStyled>
            {itemTextSize === "regular" ? <H4Styled>{t(nameKey ?? "")}</H4Styled> : <></>}
            {itemTextSize === "small" ? <H5Styled>{t(nameKey ?? "")}</H5Styled> : <></>}
        </>
    )
}

const OptionalLink = ({ children, href }: { children: any, href: string | null | undefined }) => {
    return href && href.length ? <Link href={href}>{children}</Link> : <Box>{children}</Box>
}

const SubjectsBrowser = ({
    bgColor,
    data,
    titleKey = TRANSLATION_KEYS.SUBJECTS.SECTION_TITLE,
    linkType = "link",
    itemTextSize = "regular"
}: BrowserPropsType) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const scrollableBoxRef = useRef<HTMLElement>(null);
    const scrollableItemBoxRef = useRef<HTMLElement>(null);
    const { setOpen } = useAuthModalContext();

    const handleClickRight = () => {
        if (!scrollableBoxRef.current || !scrollableItemBoxRef.current) return;
        var itemWidth = scrollableItemBoxRef.current.clientWidth;

        scrollableBoxRef.current.scrollBy({
            left: itemWidth,
            behavior: "smooth"
        })
    }

    const handleClickLeft = () => {
        if (!scrollableBoxRef.current || !scrollableItemBoxRef.current) return;
        var itemWidth = scrollableItemBoxRef.current.clientWidth;

        scrollableBoxRef.current.scrollBy({
            left: -itemWidth,
            behavior: "smooth"
        })
    }

    const handleLanguageBoxClick = () => {
        if (linkType !== "registration") return;
        setOpen(UserRoleEnum.Tutor, "registration");
    }

    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <AnimatedContainer >
                <Box sx={{ position: "relative" }}>
                    <H2Styled>{t(titleKey)}</H2Styled>
                    <ArrowLeftBoxStyled onClick={handleClickLeft}>
                        <IconBoxStyled><LeftArrowIcon /></IconBoxStyled>
                    </ArrowLeftBoxStyled>
                    <ScrollBoxStyled ref={scrollableBoxRef}>
                        <ItemBoxInnerContainer>
                            {!data ? <ErrorLarge /> : <></>}
                            {data ? data.map((x, idx) =>
                                <OptionalLink key={x.id!} href={linkType === "link" ? x.href : ""}>
                                    <ItemBoxStyled ref={idx === 0 ? scrollableItemBoxRef : null} onClick={handleLanguageBoxClick}>
                                        <ItemBox iconName={x.iconName} nameKey={x.nameKey} iconCategory={x.iconCategory} itemTextSize={itemTextSize} />
                                    </ItemBoxStyled>
                                </OptionalLink>
                            ) : <></>}
                        </ItemBoxInnerContainer>
                    </ScrollBoxStyled>
                    <ArrowRightBoxStyled onClick={handleClickRight}>
                        <IconBoxStyled><RightArrowIcon /></IconBoxStyled>
                    </ArrowRightBoxStyled>
                </Box>
            </AnimatedContainer >
        </BackgroundBoxStyled>
    )
}


const BrowserWithContexts = (props: BrowserPropsType) => (
    <AuthModalProvider>
        <SubjectsBrowser {...props} />
    </AuthModalProvider>
)

export default BrowserWithContexts;
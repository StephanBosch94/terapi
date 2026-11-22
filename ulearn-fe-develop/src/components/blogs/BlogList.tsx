import React from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { experimental_sx as sx } from "@mui/system";
import { useTranslation } from "components/core/TranslationProvider";
import H5Styled from "components/styles/typography/H5Styled";
import H2Styled from "components/styles/typography/H2Styled";
import { TertiaryButton } from "components/styles/TertiaryButton";
import ArrowRightIcon from "assets/arrow-right.svg";
import Body1Styled from "components/styles/typography/Body1Styled";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Image from "next/image";
import { ContainerStyled } from "components/styles/ContainerStyled";

const StyledImage = styled(Box)(() => sx({
    img: {
        borderRadius: "4px",
        width: "100%",
        height: "auto"
    }
})) as typeof Box;

const StyledImageSmall = styled(Box)(() => sx({
    img: {
        borderRadius: "4px",
        width: "auto",
        height: "140px"
    }
})) as typeof Box;

const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: "60px 0px"
})) as typeof Box;

const BlogSmallItemsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center",
    gap: "16px"
})) as typeof Box;

const BlogItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: "left",
    justifyContent: "center",
})) as typeof Box;

const TertiaryButtonStyled = styled(TertiaryButton)(() => ({
    display: "flex",
    gap: "4px",
    backgroundColor: "transparent",
    padding: "0px",
    ":hover": {
        backgroundColor: "transparent",
        textDecoration: "underline"
    }
})) as typeof TertiaryButton;

const BLOG_POSTS = [
    {
        title: "Lessons and insights from 8 years of Pixelgrade",
        description: "Thanks to our completely open file format, your documents are future-proofed — no matter what. They’re your files and ",
        image: "/images/blog_1.png",
        width: 440,
        height: 262
    },
    {
        title: "Starting your traveling blog with Vasco",
        description: "Thanks to our completely open file format",
        image: "/images/blog_2.png",
        width: 100,
        height: 100
    },
    {
        title: "Why choose a theme that looks good with WooCommerce",
        description: "Thanks to our completely open file format.",
        image: "/images/blog_3.png",
        width: 100,
        height: 100
    },
    {
        title: "How to write content about your photographs",
        description: "Thanks to our completely open file format.",
        image: "/images/blog_4.png",
        width: 100,
        height: 100
    }
]


const BlogList = ({ bgColor }: { bgColor: string }) => {
    const { t } = useTranslation();

    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <ContainerStyled>
                <H2Styled>{t(TRANSLATION_KEYS.BLOG.TITLE)}</H2Styled>
                <Grid container columns={12} spacing={4} sx={{ display: "flex", padding: "40px 0px", justifyContent: "center", overflowX: "auto" }}>
                    <Grid item xs={12} md={6}>
                        <BlogItemBoxStyled>
                            <StyledImage>
                                <Image src={BLOG_POSTS[0].image} alt="Blogpost image" />
                            </StyledImage   >
                            <Box>
                                <H5Styled sx={{ paddingTop: "24px" }}>{t(BLOG_POSTS[0].title)}</H5Styled>
                                <Body1Styled sx={{ margin: "16px 0px" }}>{t(BLOG_POSTS[0].description)}</Body1Styled>
                                <TertiaryButtonStyled>{t(TRANSLATION_KEYS.BLOG.LEARN_MORE)} <ArrowRightIcon /></TertiaryButtonStyled>
                            </Box>
                        </BlogItemBoxStyled>
                    </Grid>
                    <Grid item xs={12} md={6}>
                        <Box sx={{ display: "flex", gap: "16px", flexDirection: "column", alignItems: "start", paddingLeft: { xs: "0px", md: "40px" } }}>
                            <BlogSmallItemsBoxStyled>
                                <StyledImageSmall>
                                    <Image src={BLOG_POSTS[1].image} alt="Blogpost image" />
                                </StyledImageSmall>
                                <Box>
                                    <H5Styled sx={{ paddingTop: "4px" }}>{t(BLOG_POSTS[1].title)}</H5Styled>
                                    <Body1Styled sx={{ margin: "8px 0px" }}>{t(BLOG_POSTS[1].description)}</Body1Styled>
                                    <TertiaryButtonStyled>{t(TRANSLATION_KEYS.BLOG.LEARN_MORE)} <ArrowRightIcon /></TertiaryButtonStyled>
                                </Box>
                            </BlogSmallItemsBoxStyled>
                            <BlogSmallItemsBoxStyled>
                                <StyledImageSmall>
                                    <Image src={BLOG_POSTS[2].image} alt="Blogpost image" />
                                </StyledImageSmall>
                                <Box>
                                    <H5Styled sx={{ paddingTop: "4px" }}>{t(BLOG_POSTS[2].title)}</H5Styled>
                                    <Body1Styled sx={{ margin: "8px 0px" }}>{t(BLOG_POSTS[2].description)}</Body1Styled>
                                    <TertiaryButtonStyled>{t(TRANSLATION_KEYS.BLOG.LEARN_MORE)} <ArrowRightIcon /></TertiaryButtonStyled>
                                </Box>
                            </BlogSmallItemsBoxStyled>
                            <BlogSmallItemsBoxStyled>
                                <StyledImageSmall>
                                    <Image src={BLOG_POSTS[3].image} alt="Blogpost image" />
                                </StyledImageSmall>
                                <Box>
                                    <H5Styled sx={{ paddingTop: "4px" }}>{t(BLOG_POSTS[3].title)}</H5Styled>
                                    <Body1Styled sx={{ margin: "8px 0px" }}>{t(BLOG_POSTS[3].description)}</Body1Styled>
                                    <TertiaryButtonStyled>{t(TRANSLATION_KEYS.BLOG.LEARN_MORE)} <ArrowRightIcon /></TertiaryButtonStyled>
                                </Box>
                            </BlogSmallItemsBoxStyled>
                        </Box>
                    </Grid>
                </Grid>
            </ContainerStyled>
        </BackgroundBoxStyled>
    )
}

export default BlogList;

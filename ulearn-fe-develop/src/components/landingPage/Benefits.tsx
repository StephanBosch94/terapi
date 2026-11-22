import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import React from "react";
import H2Styled from "components/styles/typography/H2Styled";
import { useTranslation } from "components/core/TranslationProvider";
import { BenefitType, BenefitHeadingType } from "core/services/Types/AppTypes";
import H5Styled from "components/styles/typography/H5Styled";
import Body1Styled from "components/styles/typography/Body1Styled";
import PStyled from "components/styles/typography/PStyled";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Image from "next/image";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: "60px 0px"
})) as typeof Box;

const CardBoxStyled = styled(Box)(() => sx({
    display: "inline-block",
    backgroundColor: COLORS.WHITE,
    padding: "8px 9px 4px 9px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    margin: "16px 0px"
})) as typeof Box;

const BenefitContentBox = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "4px"
})) as typeof Box;

const BenefitTitleBox = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    gap: "16px"
})) as typeof Box;


const Benefits = ({ benefits, bgColor, heading }: { benefits: BenefitType[], bgColor: string, heading: BenefitHeadingType }) => {
    const { t } = useTranslation();
    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <AnimatedContainer>
                <Grid container columns={12} spacing={4}>
                    <Grid item xs={12}>
                        <BenefitTitleBox>
                            <H2Styled>{t(heading.titleKey)}</H2Styled>
                            <PStyled>{t(heading.descriptionKey)}</PStyled>
                        </BenefitTitleBox>
                    </Grid>
                    <Grid item xs={12}>
                        <Grid container spacing={4}>
                            {benefits.map((benefit, idx) =>
                                <Grid item xs={12} md={6} lg={3} key={idx}>
                                    <CardBoxStyled>
                                        <Image src={benefit.imgPath} height={28} width={28} alt={benefit.altText} quality={75}/>
                                    </CardBoxStyled>
                                    <BenefitContentBox>
                                        <H5Styled>{t(benefit.nameKey)}</H5Styled>
                                        <Body1Styled sx={{}}>{t(benefit.descriptionKey)}</Body1Styled>
                                    </BenefitContentBox>
                                </Grid>
                            )}
                        </Grid>
                    </Grid>
                </Grid>
            </AnimatedContainer>
        </BackgroundBoxStyled>
    )
}

export default Benefits;


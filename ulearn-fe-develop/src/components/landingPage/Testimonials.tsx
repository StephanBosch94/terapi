import React from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import PStyled from "components/styles/typography/PStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import H5Styled from "components/styles/typography/H5Styled";
import Image from 'next/image';
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";


const BackgroundBoxStyled = styled(Box)(() => sx({
    padding: "60px 0px"
})) as typeof Box;

const AvatarStyled = styled(Box)(() => sx({
    img: {
        width: "48px",
        height: "48px",
        borderRadius: "50%",
    }
})) as typeof Box;

type TestemonialDataType = {
    descriptionKey: string,
    personNameKey: string,
    personDescriptionKey: string,
    imagePath: string,
    imageBlurredPath: string
};

const Testemonials = ({ testemonials, bgColor }: { testemonials: TestemonialDataType, bgColor: string }) => {
    const { t } = useTranslation();
    return (
        <React.Fragment>
            <BackgroundBoxStyled sx={{ display: "flex", backgroundColor: bgColor, justifyContent:"center" }}>
                <AnimatedContainer>
                    <Grid container columns={12} spacing={4}>
                        <Grid item xs={12} md={6}>
                            <Box sx={{ display: "flex", justifyContent: "center", width: "100%", img: { width: "80%", height: "auto" } }}>
                                <Image src={testemonials.imagePath} alt="" height={541} width={448} quality={75} priority={true} placeholder="blur" blurDataURL={LOADING_IMAGES.TESTEMONIALS.MAIN} />
                            </Box>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Box sx={{ height: "100%", display: "flex", alignContent: "center", justifyContent: "center", flexWrap: "wrap" }}>
                                <PStyled sx={{ display: "flex", textAlign: "center" }}>{t(testemonials.descriptionKey)}</PStyled>
                                <Box sx={{ display: "flex", gap: "16px", flexDirection: "row", marginTop: "16px", alignItems: "center" }}>
                                    <AvatarStyled>
                                        <Image src={testemonials.imagePath} height={48} width={48} placeholder="blur" blurDataURL={LOADING_IMAGES.TESTEMONIALS.MAIN} alt={t(testemonials.personNameKey)} priority={false} />
                                    </AvatarStyled>
                                    <Box>
                                        <H5Styled>{t(testemonials.personNameKey)}</H5Styled>
                                        <Body1Styled>{t(testemonials.personDescriptionKey)}</Body1Styled>
                                    </Box>
                                </Box>
                            </Box>
                        </Grid>
                    </Grid>
                </AnimatedContainer>
            </BackgroundBoxStyled>
        </React.Fragment>)
}
export default Testemonials;
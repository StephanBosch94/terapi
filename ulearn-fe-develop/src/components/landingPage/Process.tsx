import { useTranslation } from "components/core/TranslationProvider";
import React from "react";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import PStyled from "components/styles/typography/PStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import H2Styled from "components/styles/typography/H2Styled";
import H5Styled from "components/styles/typography/H5Styled";
import { ProcessHeadingType, ProcessType } from "core/services/Types/AppTypes";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { AnimatedContainer } from "components/styles/AnimatedContainerStyled";

const BackgroundBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    padding: "60px 0px"
})) as typeof Box;

const PROCESS_NUMBER_BOX_WIDTH = 48;
const PROCESS_NUMBER_BOX_HEIGHT = 48;
const LINE_HEIGHT = 100;
const PADDING = 60;
const PROCESS_NUMBER_BOX_WIDTH_MOBILE = 40;
const PROCESS_NUMBER_BOX_HEIGHT_MOBILE = 40;

const MobileBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: { xs: "block", md: "none" }
})) as typeof Box;

const MobileNumberBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    width: "100%",
    justifyContent: "space-between",
    alignItems: "center"
})) as typeof Box;

const ProcessNumberBoxStyled = styled(Box)(() => sx({
    display: "flex",
    backgroundColor: COLORS.WHITE,
    width: PROCESS_NUMBER_BOX_WIDTH,
    height: PROCESS_NUMBER_BOX_HEIGHT,
    border: `1px solid ${COLORS.PRIMARY}`,
    borderRadius: "50%",
    padding: "8px",
    color: COLORS.BLACK,
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const ProcessNumberBoxMobileStyled = styled(Box)(() => sx({
    display: "flex",
    backgroundColor: COLORS.WHITE,
    width: "24px",
    height: "24px",
    border: `1px solid ${COLORS.PRIMARY}`,
    borderRadius: "50%",
    padding: "8px",
    color: COLORS.BLACK,
    alignItems: "center",
    justifyContent: "center",
})) as typeof Box;

const ProcessDescriptionBoxStyled = styled(Box)(() => sx({
    display: "flex",
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    fontSize: "0.875rem",
    width: { xs: "auto", md: "400px" },
    padding: "0px 16px",
    borderRadius: "8px",
    alignItems: "center",
    textAlign: { xs: "center", md: "left" },
    marginLeft: { sx: "0px", md: "40px" },
})) as typeof Box;

const LeftInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center",
    gap: "16px",
    ".active": {
        backgroundColor: COLORS.PRIMARY,
        cursor: "pointer",
        color: COLORS.WHITE,

    }
})) as typeof Box;

const LeftInnerBoxMobileStyled = styled(Box)(() => sx({
    justifyContent: "space-between",
    ".active": {
        backgroundColor: COLORS.PRIMARY,
        cursor: "pointer",
        color: COLORS.WHITE,
        width: PROCESS_NUMBER_BOX_WIDTH_MOBILE,
        height: PROCESS_NUMBER_BOX_HEIGHT_MOBILE,
    }
})) as typeof Box;

const LeftGridItemStyled = styled(Grid)(() => sx({
    display: "flex",
    flexDirection: "row",
    justifyContent: "right",
    gap: "16px"
})) as typeof Grid;

const ConnectionLineContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "right",
    marginRight: `${(PROCESS_NUMBER_BOX_WIDTH / 2) - 1}px`,
})) as typeof Box;

const ConnectionLineBoxStyled = styled(Box)(() => sx({
    width: "1px",
    height: `${LINE_HEIGHT}px`,
    backgroundColor: COLORS.BLACK_10,
})) as typeof Box;

const ConnectionLineMobileBoxStyled = styled(Box)(() => sx({
    width: "100%",
    border: `1px dashed ${COLORS.BLACK_10}`,
    strokeDasharray: 10,
    margin: "0px 16px"
})) as typeof Box;

const ProcessNumberTypography = styled(Typography)(() => sx({
    fontWeight: 400,
    fontSize: "1rem"
})) as typeof Typography;

const ProcessNumberMobileTypography = styled(Typography)(() => sx({
    fontWeight: 400,
    fontSize: "1rem"
})) as typeof Typography;

const ProcessMobile = ({ processes, activeStep, setActiveStep }: { processes: ProcessType[], activeStep: number, setActiveStep: (x: number) => void }) => {
    const { t } = useTranslation();
    return (
        <MobileBoxStyled>
            <MobileNumberBoxStyled>
                {
                    processes.map((keyPair, idx) => (
                        <React.Fragment key={idx} >
                            <LeftInnerBoxMobileStyled>
                                <ProcessNumberBoxMobileStyled className={(activeStep === idx) ? "active" : ""} onClick={() => { setActiveStep(idx) }}>
                                    {activeStep === idx ? <ProcessNumberMobileTypography>{idx + 1}</ProcessNumberMobileTypography> : <></>}
                                </ProcessNumberBoxMobileStyled>
                            </LeftInnerBoxMobileStyled>
                            {idx !== processes.length - 1 && <ConnectionLineMobileBoxStyled />}
                        </React.Fragment>
                    ))
                }
            </MobileNumberBoxStyled>
            <H5Styled sx={{ marginTop: "32px", display: "flex", justifyContent: "center" }}>{t(processes[activeStep].nameKey)}</H5Styled>
        </MobileBoxStyled>
    )
}
const ProcessDesktop = ({ processes, activeStep, setActiveStep }: { processes: ProcessType[], activeStep: number, setActiveStep: (x: number) => void }) => {
    const { t } = useTranslation();
    return (
        <Grid item xs={6} sx={{ display: { xs: "none", md: "flex" } }}>
            <Grid container sx={{ padding: "60px 0px" }}>
                {
                    processes.map((keyPair, idx) => (
                        <React.Fragment key={idx}>
                            <LeftGridItemStyled item xs={12}>
                                <LeftInnerBoxStyled>
                                    <H5Styled>{t(keyPair.nameKey)}</H5Styled>
                                    <ProcessNumberBoxStyled className={(activeStep === idx) ? "active" : ""} onMouseEnter={() => { setActiveStep(idx) }}>
                                        <ProcessNumberTypography>{idx + 1}</ProcessNumberTypography>
                                    </ProcessNumberBoxStyled>
                                </LeftInnerBoxStyled>
                            </LeftGridItemStyled>
                            {
                                idx !== processes.length - 1 &&
                                (
                                    <Grid item xs={12}>
                                        <ConnectionLineContainerBoxStyled>
                                            <ConnectionLineBoxStyled />
                                        </ConnectionLineContainerBoxStyled>
                                    </Grid>
                                )
                            }

                        </React.Fragment>
                    ))
                }
            </Grid>
        </Grid>
    )
}

const Process = ({ processes, headings, bgColor }: { processes: ProcessType[], headings: ProcessHeadingType, bgColor: string }) => {
    const [activeStep, setActiveStep] = React.useState(0);
    const textBlockRef = React.useRef<HTMLDivElement>(null);
    const { t } = useTranslation();

    const getMarginTop = () => {
        const marginTop = ((LINE_HEIGHT + PROCESS_NUMBER_BOX_HEIGHT) * activeStep) + PADDING;
        return marginTop;
    }

    return (
        <BackgroundBoxStyled sx={{ backgroundColor: bgColor }}>
            <AnimatedContainer>
                    <Box sx={{ display: "flex", alignItems: "center", flexDirection: "column", gap: "16px", marginBottom: "40px" }}>
                        <H2Styled sx={{ textAlign: "center" }}>{t(headings.titleKey)}</H2Styled>
                        <PStyled sx={{ textAlign: "center", maxWidth: "920px" }}>{t(headings.descriptionKey)}</PStyled>
                    </Box>
                    <Grid container columns={12}>
                        <ProcessMobile activeStep={activeStep} processes={processes} setActiveStep={setActiveStep} />
                        <ProcessDesktop key={activeStep} processes={processes} activeStep={activeStep} setActiveStep={setActiveStep} />
                        <Grid item xs={12} md={6}>
                            <Grid container>
                                <Grid item xs={12}>
                                    <ProcessDescriptionBoxStyled sx={{ marginTop: { xs: "16px", md: `${getMarginTop()}px` } }}>
                                        <p ref={textBlockRef}>{t(processes[activeStep].descriptionKey)}</p>
                                    </ProcessDescriptionBoxStyled>
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
            </AnimatedContainer>
        </BackgroundBoxStyled >
    )
}

export default Process;
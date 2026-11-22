import LinearProgress from "@mui/material/LinearProgress";
import { styled } from "@mui/material/styles";
import { COLORS } from "../constants/ColorsConstatnts";

const LinearProgressStyled = styled(LinearProgress)({
    position: "fixed",
    top: 0,
    left: 0,
    width: "100%",
    zIndex: "1001",
    backgroundColor: COLORS.PRIMARY
}) as typeof LinearProgress;

export default LinearProgressStyled;
import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const CustomBoxWithoutColorStyled = styled(Box)(() => sx({
    minWidth: { xs: "calc(100% - 32px)", md: "auto" },
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: 'translate(-50%, -50%)',
    padding: { xs: "20px 20px", md: "40px 40px" },
    textAlign: "center",
    borderRadius: "4px",
    backgroundColor: COLORS.WHITE,
    a: {
        textDecoration: "underline",
        color: COLORS.PRIMARY,
    }
})) as typeof Box;

export default CustomBoxWithoutColorStyled;

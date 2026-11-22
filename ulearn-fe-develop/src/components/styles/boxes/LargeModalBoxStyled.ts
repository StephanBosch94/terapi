import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const LargeModalBoxStyled = styled(Box)(() => sx({
    maxWidth: { xs: "calc(100% - 16px)", md: "auto" },
    minWidth: { xs: "calc(100% - 24px)", md: "auto" },
    position: "absolute",
    gap: "40px",
    top: "50%",
    left: "50%",
    transform: 'translate(-50%, -50%)',
    padding: { xs: "24px", md: "40px" },
    boxShadow: "0px 4px 4px rgba(0, 0, 0, 0.1)",
    textAlign: "center",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    flexFlow: "row noWrap",
    borderRadius: "4px",
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`,
    a: {
        textDecoration: "underline",
        color: COLORS.PRIMARY,
    }
})) as typeof Box;

export default LargeModalBoxStyled;

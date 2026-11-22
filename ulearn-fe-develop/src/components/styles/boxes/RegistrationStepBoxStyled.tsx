
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const RegistrationStepBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    flexFlow: "column noWrap",
    width: { xs: "auto", md: "640px" },
    overflow: "auto",
    padding: { xs: "0px 16px", md: "0px 8px 0 0" },
    gap: "24px",
})) as typeof Box;

export default RegistrationStepBoxStyled;
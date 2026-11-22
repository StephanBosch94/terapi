import Box from "@mui/material/Box"
import { styled, experimental_sx as sx } from "@mui/material/styles";

const TutorItemBoxWithStatusStyled = styled(Box)(() => sx({
    position: "relative",
    width: "fit-content",
    height: "auto",
    ".online": {
        position: "absolute",
        top: { xs: "0px", md: "8px" },
        right: { xs: "4px", md: "8px" }
    },
    img: {
        width: { xs: "72px", md: "220px" },
        height: { xs: "72px", md: "220px" },
        objectFit: "cover",
        objectPosition: "50% 50%"
    }
})) as typeof Box;


export default TutorItemBoxWithStatusStyled;
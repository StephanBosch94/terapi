import Box from "@mui/material/Box"
import { COLORS } from "../constants/ColorsConstatnts"


const OnlineIconBoxStyled = () => {

    return (
        <Box sx={{ width: "16px", height: "16px", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <Box sx={{ width: "8px", height: "8px", backgroundColor: COLORS.GREEN, borderRadius: "50%" }} />
        </Box>
    )
}

export default OnlineIconBoxStyled;
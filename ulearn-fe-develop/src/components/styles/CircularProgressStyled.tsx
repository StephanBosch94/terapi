import CircularProgress from "@mui/material/CircularProgress";
import { COLORS } from "./constants/ColorsConstatnts";

const CircularProgressCustom = ({size = 20, color = COLORS.PRIMARY}: {size?: number | undefined, color?: string | undefined}) => {
    return (
        <CircularProgress size={size} style={{ color: color }} />
    )
}

export default CircularProgressCustom;
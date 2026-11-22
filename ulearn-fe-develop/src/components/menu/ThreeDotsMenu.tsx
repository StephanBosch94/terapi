import { IconButton } from "components/styles/IconButton";
import Menu, { MenuPropsType } from "./Menu";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import DotsIcon from "assets/3_dots.svg";

const IconDotsStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.5)",
    }
}) as typeof Box;


const ThreeDotsMenu = (props: Omit<MenuPropsType, "content">) => (
    <Menu
        content={
            <IconButton>
                <IconDotsStyled><DotsIcon /></IconDotsStyled>
            </IconButton>
        }
        {...props}
    />
)

export default ThreeDotsMenu;
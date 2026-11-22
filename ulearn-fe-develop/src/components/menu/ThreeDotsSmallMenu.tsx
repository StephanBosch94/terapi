import Menu, { MenuPropsType } from "./Menu";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import DotsIcon from "assets/3_dots.svg";

const EventDotsIconBox = styled(Box)({
    border: `1px solid ${COLORS.PRIMARY}`,
    padding: "4px",
    borderRadius: "4px",
    alignItems: "center",
    display: "flex",
    color: COLORS.PRIMARY,
    svg: {
        transform: "scale(1.25)"
    }
}) as typeof Box;


const ThreeDotsMenu = (props: Omit<MenuPropsType, "content">) => (
    <Menu
        content={
            <EventDotsIconBox>
                <DotsIcon />
            </EventDotsIconBox>
        }
        {...props}
    />
)

export default ThreeDotsMenu;
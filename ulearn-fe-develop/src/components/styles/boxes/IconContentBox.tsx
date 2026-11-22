import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

const OuterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    gap: "8px",
    alignItems: "center",
})) as typeof Box;

const IconBoxStyled = styled(Box)(() => sx({
    width: "16px",
    display: "flex",
    alignItems: { xs: "flex-start", md: "center" },
    color: COLORS.BLACK,
})) as typeof Box;

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexWrap: "wrap",
    gap: "8px",
    width: "100%",
    color: COLORS.BLACK_60
})) as typeof Box;

const IconContentBox = ({ icon, children }: { icon: JSX.Element, children: any }) => {

    return (
        <OuterBoxStyled>
            <IconBoxStyled>{icon}</IconBoxStyled>
            <InnerBoxStyled>
                {children}
            </InnerBoxStyled>
        </OuterBoxStyled>
    )
}

export default IconContentBox;
import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import KeyboardArrowDownIcon from 'assets/caret-down.svg';
import KeyboardArrowUpIcon from 'assets/caret-up.svg';

const SvgBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    "svg": {
        transform: "scale(1)"
    }
})) as typeof Box;

const MunuInputIcon = ({ isOpen }: { isOpen: boolean }) =>
    isOpen
        ? <SvgBoxStyled><KeyboardArrowUpIcon /></SvgBoxStyled >
        : <SvgBoxStyled><KeyboardArrowDownIcon /></SvgBoxStyled>

export default MunuInputIcon;
import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";

const SubjectLevelBoxStyled = styled(Box)(() => sx({
    padding: "0 8px"
})) as typeof Box;


const Badge = ({ bgColor, children }: { bgColor: string, children: JSX.Element }) => {

    return (
        <SubjectLevelBoxStyled sx={{ backgroundColor: bgColor }}>
            {children}
        </SubjectLevelBoxStyled>
    )
}

export default Badge;
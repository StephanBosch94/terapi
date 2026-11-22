import InputAdornment from "@mui/material/InputAdornment";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";

const InputAdornmentStyled = styled(InputAdornment)({
    color: COLORS.BLACK_10
}) as typeof InputAdornment;

const InputCurrncyAdornment = () => {
    const { currentCurrency } = useAppDataContext();

    return (
        <InputAdornmentStyled position="end">
            <CaptionTextStyled>
                {currentCurrency.symbol ?? ""}
            </CaptionTextStyled>
        </InputAdornmentStyled>
    )
}

export default InputCurrncyAdornment;
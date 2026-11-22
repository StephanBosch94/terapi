import { styled, experimental_sx as sx } from "@mui/material/styles";
import { PrimaryButton } from "components/styles/PrimaryButton";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import Typography from "@mui/material/Typography";
import ArrowIcon from "assets/arrow-right.svg";
import { UIContext } from "components/core/UIContextProvider";
import { useContext } from "react";

const ContinueButtonStyled = styled(PrimaryButton)(() => sx({
    display: "inline-flex",
    gap: { xs: "4px", md: "6px" },
    alignContent: "center",
    borderRadius: "4px",
    padding: { xs: "8px 16px", md: "12px 24px" }
})) as typeof PrimaryButton;

const ContinueButtonTypography = styled(Typography)({
    fontSize: "0.875rem",
    fontWeight: "500"
}) as typeof Typography;

const ReservationContinueButton = ({ onClickHandler }: { onClickHandler: () => void }) => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    return (
        <ContinueButtonStyled onClick={onClickHandler}>
            <ContinueButtonTypography>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.CONTINUE_RESERVATION)}</ContinueButtonTypography>
            {screenType === "desktop" ? <ArrowIcon /> : <></>}
        </ContinueButtonStyled>
    )
}

export default ReservationContinueButton;
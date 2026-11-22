import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";

const MandatoryBoxStyled = styled(Box)({
    display: "flex",
    gap: "4px"
}) as typeof Box;

const AttributeLabelStyled = styled(Body1Styled)({
    fontWeight: "600",
    color: COLORS.BLACK
});

const MandatoryLabelStyled = styled(Body1Styled)({
    fontWeight: "600",
    color: COLORS.RED
});

const InputFieldLabel = ({ titleKey, isMandatory = false }: { titleKey: string, isMandatory?: boolean }) => {
    const { t } = useTranslation();

    return (
        <MandatoryBoxStyled sx={{ width: "auto" }}>
            <AttributeLabelStyled>{t(titleKey)}</AttributeLabelStyled>
            <MandatoryLabelStyled>*</MandatoryLabelStyled>
        </MandatoryBoxStyled>
    )
}

export default InputFieldLabel;
import { Box, styled } from "@mui/material";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Body1Styled from "components/styles/typography/Body1Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import { useTranslation } from "components/core/TranslationProvider";
import SpecializationIcon from "assets/specialization.svg";
import { FloatableBoxStyled } from "components/styles/boxes/FloatableBoxStyled";
import { TutorItemRowBoxStyled } from "./styles/TutorItemRowBoxStyled";
import InjectIcon from "components/core/icons/InjectIcon";

const SpecializationItemBoxStyled = styled(Box)({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px",
    marginRight: "8px"
}) as typeof Box;

const SpecializationList = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();

    return (
        <>
            {user.specializations?.map((x, idx) => (
                <SpecializationItemBoxStyled key={x.specializationId!}>
                    <InjectIcon category="specializations" iconName={x.iconName ?? ""} />
                    <Body1Styled sx={{ color: COLORS.BLACK }}>{t(x.nameKey ?? "")}</Body1Styled>
                </SpecializationItemBoxStyled>
            ))}
        </>
    )
}

const Specializations = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();
    return (
        <TutorItemRowBoxStyled>
            <FloatableBoxStyled>
                <SpecializationIcon />
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.SPECIALIZATION)}</Body1Styled>
            </FloatableBoxStyled>
            <SpecializationList user={user} />
        </TutorItemRowBoxStyled>
    )
}

export default Specializations;
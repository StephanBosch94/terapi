import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { selectDuration, setDuration } from "core/redux/availabilityCalendarSlice";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useTranslation } from "components/core/TranslationProvider";
import { useMemo } from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

const ClassDurationBaseBox = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    width: "100%",
    padding: "4px 8px",
    borderRadius: "4px",
    minWidth: "70px"
}) as typeof Box;

const ClassDurationActiveBoxStyled = styled(ClassDurationBaseBox)({
    display: "inline-flex",
    justifyContent: "center",
    width: "100%",
    padding: "4px 8px",
    borderRadius: "4px",
    backgroundColor: COLORS.PRIMARY,
    color: COLORS.WHITE,
    minWidth: "70px"
}) as typeof Box;

const ClassDurationInnerBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    width: "100%",
    padding: "4px 8px",
    borderRadius: "4px",
    minWidth: "70px",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5
    }
}) as typeof Box;

const ClassDurationTypograpgyStyled = styled(Typography)({
    fontSize: "0.875rem",
    lineHeight: "20px"
}) as typeof Typography;

const OuterBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    padding: "8px",
    gap: "4px",
    borderRadius: "4px",
    backgroundColor: COLORS.WHITE,
    border: `1px solid ${COLORS.BLACK_10}`
}) as typeof Box;

export const getDurations = (): { duration: ClassDurationEnum, translationKey: string }[] => {
    return [
        {
            duration: ClassDurationEnum.Full,
            translationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.DURATION_50_MIN
        },
        {
            duration: ClassDurationEnum.Half,
            translationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.DURATION_25_MIN
        }
    ]
}

const TutorClassDurationSmall = () => {
    const durations = useMemo(() => getDurations(), []);
    const dispatch = useAppDispatch();
    const duration = useAppSelector(selectDuration);

    const handleSetDuration = (duration: ClassDurationEnum) => {
        dispatch(setDuration(duration));
    }

    const { t } = useTranslation();
    return (
        <OuterBoxStyled>
            {
                durations.map((x, idx) => duration === x.duration ? (
                    <ClassDurationActiveBoxStyled key={idx}>
                        <ClassDurationTypograpgyStyled>{t(x.translationKey)}</ClassDurationTypograpgyStyled>
                    </ClassDurationActiveBoxStyled>
                ) : (
                    <ClassDurationInnerBoxStyled key={idx} onClick={() => handleSetDuration(x.duration)}>
                        <ClassDurationTypograpgyStyled>{t(x.translationKey)}</ClassDurationTypograpgyStyled>
                    </ClassDurationInnerBoxStyled>
                ))
            }
        </OuterBoxStyled>
    )
}

export default TutorClassDurationSmall;
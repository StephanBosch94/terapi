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

const ClassDurationInnerBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    width: "100%",
    padding: "4px 0 4px 0",
    color: COLORS.BLACK,
    borderRadius: "4px",
}) as typeof Box;

const ClassDurationTypograpgyStyled = styled(Typography)({
    fontSize: "0.875rem",
    lineHeight: "20px"
}) as typeof Typography;

const RightPanelDurationBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    width: "100%",
    padding: "8px",
    gap: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    ".active": {
        backgroundColor: COLORS.PRIMARY,
        color: COLORS.WHITE
    },
    ".passive:hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_10,
    }
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

const TutorClassDuration = () => {
    const dispatch = useAppDispatch();
    const duration = useAppSelector(selectDuration);
    const durations = useMemo(() => getDurations(), [duration]);

    const handleSetDuration = (duration: ClassDurationEnum) => {
        dispatch(setDuration(duration));
    }

    const { t } = useTranslation();
    return (
        <RightPanelDurationBoxStyled>
            {
                durations.map((x, idx) =>
                    <ClassDurationInnerBoxStyled
                        key={idx}
                        className={duration === x.duration ? "active" : "passive"}
                        onClick={() => handleSetDuration(x.duration)}
                    >
                        <ClassDurationTypograpgyStyled>{t(x.translationKey)}</ClassDurationTypograpgyStyled>
                    </ClassDurationInnerBoxStyled>
                )
            }
        </RightPanelDurationBoxStyled>
    )
}

export default TutorClassDuration;
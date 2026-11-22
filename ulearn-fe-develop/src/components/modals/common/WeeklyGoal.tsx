import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import Menu from "components/menu/Menu";
import MenuItemsList from "components/menu/MenuItemList";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import MunuInputIcon from "components/styles/icons/MenuInputIcon";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { CodetableWeeklyGoalReponse } from "core/redux/codetableSlice";
import { useState } from "react";

const CaptionBlackStyled = styled(CaptionTextStyled)(() => sx({
    color: COLORS.BLACK
})) as typeof CaptionTextStyled;

const TextFieldBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    width: "100%",
    flexFlow: "column noWrap",
    gap: "4px"
})) as typeof Box;

const InputBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    justifyContent: "space-between",
    width: "100%",
    background: COLORS.WHITE,
    border: `1px solid ${(COLORS.BLACK_10)}`,
    borderRadius: "4px",
    boxSizing: "border-box",
    padding: "10px 14px",
    fontWeight: 400,
    color: COLORS.BLACK_60,
    ":hover": {
        borderColor: COLORS.BLACK_40,
        cursor: "pointer"
    },
    ".active": {
        color: COLORS.BLACK
    }
})) as typeof Box;

const WeeklyGoalInputField = ({ weeklyGoals, name, prevValue }: { weeklyGoals: CodetableWeeklyGoalReponse, name?: string, prevValue?: number | null | undefined }) => {
    const [weeklyGoalId, setWeeklyGoalId] = useState<number>(prevValue ?? weeklyGoals[0].weeklyGoalId ?? 1);
    const [open, setOpen] = useState(false);
    const items = weeklyGoals.map(x => ({ id: x.weeklyGoalId, nameKey: x.nameKey }));
    const selectedGoal = weeklyGoals.find(x => x.weeklyGoalId === weeklyGoalId);
    const { t } = useTranslation();
    
    const handleSelectWeeklyGoalId = (newWeeklyGoalId: number) => {
        setWeeklyGoalId(newWeeklyGoalId);
    }

    return (
        <TextFieldBoxStyled>
            <CaptionBlackStyled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.INVITE_PEOPLE_MODAL.EMPLOYEES_WEEKLY_GOAL)}</CaptionBlackStyled>
            <Menu
                sameWidths={true}
                content={
                    <InputBoxStyled>
                        <Box className={weeklyGoalId ? "active" : ""}>
                            {t(selectedGoal?.nameKey ?? "")}
                        </Box>
                        <MunuInputIcon isOpen={open} />
                        <input type="number" readOnly={true} name={name} value={weeklyGoalId ?? 0} style={{ display: "none" }} />
                    </InputBoxStyled>
                }
                innerMenu={(
                    <MenuItemsList items={items} onClick={handleSelectWeeklyGoalId} size="large" />
                )}
                onClick={(newVal: boolean) => setOpen(newVal)}
                autoClose={true}
            />
        </TextFieldBoxStyled>
    )
}

export default WeeklyGoalInputField;
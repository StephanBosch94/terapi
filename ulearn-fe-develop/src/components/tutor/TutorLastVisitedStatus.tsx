import Box from "@mui/material/Box"
import { useTranslation } from "components/core/TranslationProvider";
import Tooltip from "components/styles/Tooltip"
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import moment from "moment";
import { styled, experimental_sx as sx } from "@mui/material/styles";

const OuterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "16px",
    height: "16px",
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: COLORS.WHITE,
    borderRadius: "50%",
    boxShadow: '0px 5px 4px rgba(0, 0, 0, 0.1)',
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "10px",
    height: "10px",
    borderRadius: "50%"
})) as typeof Box;

const TutorLastVisitedStatus = ({ isOnline, lastSeen }: { isOnline: boolean | undefined, lastSeen: string | undefined | null }) => {
    const { t } = useTranslation();
    
    return (
        <Box className="online">
            <Tooltip text={isOnline ? t(TRANSLATION_KEYS.TUTOR_ITEM.ONLINE) : t(TRANSLATION_KEYS.TUTOR_ITEM.VISITED, { date: moment(lastSeen).fromNow() })} >
                <OuterBoxStyled>
                    <InnerBoxStyled sx={{ backgroundColor: isOnline ? COLORS.GREEN : COLORS.BLACK_40 }} />
                </OuterBoxStyled>
            </Tooltip >
        </Box>
    )
}

export default TutorLastVisitedStatus;
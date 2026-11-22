import { Box, experimental_sx as sx, styled } from "@mui/system";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import { getUserName, getShortName } from "core/services/Utils/userUtils";
import routes from "routes/routes";
import { useContext } from "react";
import { UIContext } from "components/core/UIContextProvider";
import { useTranslation } from "components/core/TranslationProvider";
import FlagIcon from "components/core/FlagIcon";
import Link from "next/link";
import TutorItemVerified from "./TutorItemVerified";

const UserTitleBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    alignItems: "center",
    gap: "8px",
    color: COLORS.BLACK,
    fontSize: { xs: "1.2rem", md: "1.25rem" },
    fontWeight: 700,
    "a": {
        textDecoration: "none", color: COLORS.BLACK
    },
    "a:hover": {
        textDecoration: "underline !important",
        cursor: "pointer !important"
    }
})) as typeof Box;



const TutorItemHeaderDesktop = ({ user }: { user: (AppUserOverviewVM) }) => {
    return (
        <Box sx={{ display: "flex", width: "100%", justifyContent: "space-between", alignItems: "center" }}>
            <UserTitleBoxStyled>
                <Link href={routes.tutorIdPage.path(user.userId!)}>{getUserName(user)}</Link>
                <FlagIcon countryCode={user.country?.countryCode} />
            </UserTitleBoxStyled>
            {user.certificationsVerified ? <TutorItemVerified /> : <></>}
        </Box>
    )
}

const TutorItemHeaderMobile = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();

    return (
        <UserTitleBoxStyled>
            {getShortName(user)}
            <FlagIcon countryCode={user.country?.countryCode} />
        </UserTitleBoxStyled>
    )
}

const TutorItemHeader = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { screenType } = useContext(UIContext);

    return (
        <>
            {screenType === "desktop" ? <TutorItemHeaderDesktop user={user} /> : <></>}
            {screenType === "mobile" ? <TutorItemHeaderMobile user={user} /> : <></>}
        </>
    )
}

export default TutorItemHeader;
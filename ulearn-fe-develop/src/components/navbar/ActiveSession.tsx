import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppReservationVM } from "core/services/Types/AppTypes";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { getUserName } from "core/services/Utils/userUtils";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { useTranslation } from "components/core/TranslationProvider";
import routes from "routes/routes";
import { SecondaryButton } from "components/styles/SecondaryButton";
import Body1Styled from "components/styles/typography/Body1Styled";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Link from "next/link";

const HOUR_FORMAT = "HH:mm";

const AvatarStyled = styled(Avatar)(() => sx({
    width: "40px",
    height: "40px",
    border: `1px solid ${COLORS.WHITE}`
})) as typeof Avatar

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "12px",
    height: "100%"
})) as typeof Box;

const BoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "space-between",
    padding: "16px",
    width: "100%",
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "4px",
})) as typeof Box;

const getActivityTranslationKey = (reservation: AppReservationVM, time: moment.Moment) => {
    const result = time.isAfter(moment(reservation.startTime))
        ? TRANSLATION_KEYS.NAVBAR.RUNNING_ACTIVITY
        : TRANSLATION_KEYS.NAVBAR.SCHEDULED_ACTIVITY

    return result;
}

const SecondaryButtonStyled = styled(SecondaryButton)(() => sx({
    padding: "8px 16px",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.WHITE
    }
})) as typeof SecondaryButton;

const getAcivityTranslationPlaceholderValues = (reservation: AppReservationVM, time: moment.Moment) => {
    const startTime = moment(reservation.startTime);

    return {
        user: getUserName(reservation.opponent),
        from: moment(reservation.startTime).format(HOUR_FORMAT),
        to: moment(reservation.endTime).format(HOUR_FORMAT),
    }
}

const ActiveSession = ({ runningActivity }: { runningActivity: AppReservationVM }) => {
    const { t } = useTranslation();
    const [time, setTime] = useState(moment());

    useEffect(() => {
        // Infinity dependency loop
        const timer = setTimeout(() => {
            setTime(moment())
        }, 1000);

        return () => clearTimeout(timer);
    }, [time]);

    return (
        <BoxStyled>
            <InnerBoxStyled>
                <AvatarStyled src={getProfilePhotoThumbnailUrl(runningActivity.opponent)} />
                <Body1Styled sx={{ color: COLORS.WHITE }}>
                    {t(getActivityTranslationKey(runningActivity, time), getAcivityTranslationPlaceholderValues(runningActivity, time))}
                </Body1Styled>
            </InnerBoxStyled>
            <InnerBoxStyled>
                <Link href={routes.meetingPage.path(runningActivity.room?.roomKey ?? "")} style={{ textDecoration: "none" }}><SecondaryButtonStyled>{t(TRANSLATION_KEYS.NAVBAR.JOIN_ACTIVITY)}</SecondaryButtonStyled></Link>
            </InnerBoxStyled>
        </BoxStyled>

    )
}

export default ActiveSession;
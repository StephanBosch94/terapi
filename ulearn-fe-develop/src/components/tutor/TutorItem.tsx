import { useTranslation } from "components/core/TranslationProvider";
import { Box, experimental_sx as sx, styled } from "@mui/system";
import React, { useContext } from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppUserOverviewVM } from "core/services/Types/AppTypes";
import PStyled from "components/styles/typography/PStyled";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { getUserRating, getUserName, getShortenDescription } from "core/services/Utils/userUtils";
import TutorSubjectBox from "./TutorSubjectBox";
import StarIcon from "assets/star-filled.svg";
import Body1Styled from "components/styles/typography/Body1Styled";
import { UIContext } from "components/core/UIContextProvider";
import TutorItemSpecializaitons from "components/tutor/TutorItemSpecializations";
import TutorItemHeader from "./TutorItemHeader";
import TutorItemBoxWithStatusStyled from "./styles/TutorItemBoxWithStatusStyled";
import Image from "next/image";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import Grid from "@mui/material/Grid";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import routes from "routes/routes";
import Link from "next/link";
import { PrimaryButton } from "components/styles/PrimaryButton";
import H5Styled from "components/styles/typography/H5Styled";
import TutorLanguagesBox from "./TutorLanguagesBox";
import TutorLastVisitedStatus from "./TutorLastVisitedStatus";
import { usePriceLocalization } from "core/hooks/priceHook";

const ItemBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    width: "100%",
    padding: "16px",
    display: "flex",
    gap: "24px",
    borderRadius: "8px",
    "a": {
        textDecoration: "none", color: COLORS.BLACK
    },
    "a:hover": {
        cursor: "pointer !important"
    }
})) as typeof Box;

const UserRatingStyled = styled(PStyled)(() => sx({
    fontWeight: 600,
    color: COLORS.BLACK
})) as typeof PStyled;

const ProfilePhotoGridStyled = styled(Grid)(() => sx({
    position: "relative",
    "img": {
        width: "202px",
        height: "auto",
        borderRadius: "8px",
        border: `1px solid ${COLORS.BLACK_10}`
    }
})) as typeof Grid;


const MobileHeaderDescriptionGridStyled = styled(Grid)(() => sx({
    display: "flex",
    alignContent: "space-between",
    justifyContent: "start",
    paddingLeft: "16px",
    width: "100%",
    flexFlow: "column wrap",
    gap: "4px"
})) as typeof Grid;

const UserPriceBoxStyled = styled(Box)(() => sx({
    color: COLORS.BLACK,
    fontSize: { xs: "1rem", md: "1.5rem" },
    fontWeight: 700
})) as typeof Box;

const IconStyled = styled(Box)(() => sx({
    color: COLORS.PRIMARY,
    display: "flex",
    alignItems: "center",
})) as typeof Box;

const AvatarStyled = styled(Box)(() => sx({
    img: {
        width: "80px",
        height: "80px",
        borderRadius: "50%"
    }
})) as typeof Box;

const DesktopRightSectionBox = styled(Box)(() => sx({
    borderLeft: `1px solid ${COLORS.BLACK_10}`,
    padding: "4px 8px 0px 24px",
    minWidth: "240px",
    display: "flex",
    flexFlow: "column",
    alignItems: "start",
    justifyContent: "space-between"
})) as typeof Box;

const RatingBoxStyledBox = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    alignItems: "center",
    gap: "4px"
})) as typeof Box;

const DescriptionBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
    flexFlow: "column noWrap"
})) as typeof Box;

const PricingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    alignItems: "flex-start",
    gap: "4px"
})) as typeof Box;


const TutorDescription = ({ user, maxLength = 300 }: { user: (AppUserOverviewVM), maxLength?: number }) => {

    return (
        <Body1Styled sx={{ color: COLORS.BLACK }}>
            {getShortenDescription(user, maxLength)}
        </Body1Styled>
    )
}

const TutorHourRate = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { t } = useTranslation();
    const { localizeTrial } = usePriceLocalization();

    return (
        <Box sx={{ display: "flex", flexFlow: "row wrap", alignItems: "center", gap: "4px" }}>
            <H5Styled sx={{ color: COLORS.BLACK }}>{localizeTrial({ price: user.hourRate })}</H5Styled>
            <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_ITEM.TRIAL_LESSON).toLowerCase()}</Body1Styled>
        </Box>
    )
}

const ReservationDesktopButton = ({ user }: { user: AppUserOverviewVM }) => {
    const { t } = useTranslation();

    return (
        <Link href={routes.tutorIdPage.path(user.userId!)} style={{ width: "100%" }}>
            <PrimaryButton sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.RESERVATION)}</PrimaryButton>
        </Link>
    )
}

const ReservationMobileButton = ({ user }: { user: AppUserOverviewVM }) => {
    const { t } = useTranslation();

    return (
        <Link href={routes.tutorIdPage.path(user.userId!)} style={{ width: "100%" }}>
            <PrimaryButtonRectangle sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.RESERVATION)}</PrimaryButtonRectangle>
        </Link>
    )
}

const TutorItemDesktop = ({ user }: { user: AppUserOverviewVM }) => {
    const { t } = useTranslation();
    const { localizeTrial } = usePriceLocalization();

    return (
        <ItemBoxStyled>
            <ProfilePhotoGridStyled >
                <TutorItemBoxWithStatusStyled>
                    <Image src={getProfilePhotoUrl(user)} alt={getUserName(user)} placeholder="blur" blurDataURL={LOADING_IMAGES.GRAY.RECTANGLE} width={220} height={180} />
                    {/* <TutorLastVisitedStatus isOnline={user.statistics?.isOnline} lastSeen={user.statistics?.lastVisited} /> */}
                </TutorItemBoxWithStatusStyled>
            </ProfilePhotoGridStyled>
            <Box sx={{ display: "flex", gap: "8px", flexFlow: "column noWrap" }}>
                <TutorItemHeader user={user} />
                <DescriptionBoxStyled>
                    <TutorSubjectBox user={user} />
                    <TutorLanguagesBox user={user} />
                    <TutorItemSpecializaitons user={user} />
                    <TutorDescription user={user} />
                </DescriptionBoxStyled>
            </Box>
            <DesktopRightSectionBox >
                <RatingBoxStyledBox>
                    <IconStyled><StarIcon /></IconStyled>
                    <UserRatingStyled>{getUserRating(user)}</UserRatingStyled>
                </RatingBoxStyledBox>
                <PricingBoxStyled>
                    <PStyled>{t(TRANSLATION_KEYS.TUTOR_ITEM.TRIAL_LESSON)}</PStyled>
                    <UserPriceBoxStyled>{localizeTrial({ price: user.hourRate })}</UserPriceBoxStyled>
                </PricingBoxStyled>
                <ReservationDesktopButton user={user} />
            </DesktopRightSectionBox>
        </ItemBoxStyled>
    )
}

const TutorItemMobile = ({ user }: { user: (AppUserOverviewVM) }) => {

    return (
        <ItemBoxStyled sx={{ display: "flex", flexDirection: "column", justifyContent: "space-between" }} >
            <Link href={routes.tutorIdPage.path(user.userId!)}>
                <Box sx={{ width: "100%", display: "flex", flexDirection: "row" }}>
                    <TutorItemBoxWithStatusStyled>
                        <AvatarStyled>
                            <Image src={getProfilePhotoUrl(user)} alt={getUserName(user)} height={100} width={100} placeholder="blur" blurDataURL={LOADING_IMAGES.GRAY.OVAL} />
                        </AvatarStyled>
                        {/* <TutorLastVisitedStatus isOnline={user.statistics?.isOnline} lastSeen={user.statistics?.lastVisited} /> */}
                    </TutorItemBoxWithStatusStyled>
                    <MobileHeaderDescriptionGridStyled>
                        <TutorItemHeader user={user} />
                        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
                            <TutorSubjectBox user={user} />
                            <TutorHourRate user={user} />
                        </Box>
                    </MobileHeaderDescriptionGridStyled>
                    <Box sx={{ display: "flex", gap: "4px", justifyContent: "end", alignItems: "start" }}>
                        <IconStyled sx={{ marginTop: "4px" }}><StarIcon /></IconStyled>
                        <UserRatingStyled>{getUserRating(user)}</UserRatingStyled>
                    </Box>
                </Box>
                <Box sx={{ padding: "12px 0px 16px 0px", overflowX: "hidden" }}>
                    <TutorDescription user={user} maxLength={250} />
                </Box>
                <ReservationMobileButton user={user} />
            </Link>
        </ItemBoxStyled>
    )
}

const TutorItem = ({ user }: { user: (AppUserOverviewVM) }) => {
    const { screenType } = useContext(UIContext);

    return (
        <>
            {screenType === "desktop" ? <TutorItemDesktop user={user} /> : <></>}
            {screenType === "mobile" ? <TutorItemMobile user={user} /> : <></>}
        </>
    )
}

export default TutorItem;
import { useTranslation } from "components/core/TranslationProvider";
import { Box, experimental_sx as sx, styled } from "@mui/system";
import React from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { AppUserCardOverviewVM } from "core/services/Types/AppTypes";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import routes from "routes/routes";
import { getUserRating, getUserName, getShortName } from "core/services/Utils/userUtils";
import StarIcon from "assets/star-filled.svg";
import TutorSubjectBox from "./TutorSubjectBox";
import FlagIcon from "components/core/FlagIcon";
import Link from "next/link";
import Image from "next/image";
import { LOADING_IMAGES } from "core/constants/loadingImagesConstants";
import Body1Styled from "components/styles/typography/Body1Styled";
import H5Styled from "components/styles/typography/H5Styled";
import PStyled from "components/styles/typography/PStyled";
import { usePriceLocalization } from "core/hooks/priceHook";

const ItemBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    display: "flex",
    flexDirection: "column",
    p: "16px",
    borderRadius: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5,
    },
    ".profile-image": {
        objectFit: "cover",
        objectPosition: "50% 50%"
    }
})) as typeof Box;

const UserTitleStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: 600
})) as typeof PStyled;

const UserRatingStyled = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: 600
})) as typeof Body1Styled;

const SubjectBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    gap: "4px",
    alignItems: "center",
    color: COLORS.BLACK
})) as typeof Box;

const UserPriceStyled = styled(H5Styled)(() => sx({
    color: COLORS.BLACK,
})) as typeof H5Styled;

const PriceBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "4px",
    paddingTop: "4px"
})) as typeof Box;

const HeadingBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    margin: "4px 0px",
    justifyContent: "space-between",
    alignItems: "center",
})) as typeof Box;

const IconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const ContentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
    flexFlow: "column wrap"
})) as typeof Box;

const TutorItemSmall = ({ user }: { user: (AppUserCardOverviewVM) }) => {
    const { t } = useTranslation();
    const { localizeTrial } = usePriceLocalization();

    return (
        <ItemBoxStyled>
            <Link href={routes.tutorIdPage.path(user.userId!)} style={{ textDecoration: "none", color: COLORS.BLACK }}>
                <Image className="profile-image" src={getProfilePhotoUrl(user)} width={230} height={230} style={{ borderRadius: "8px" }} placeholder="blur" alt={getUserName(user)} blurDataURL={LOADING_IMAGES.GRAY.RECTANGLE} />
                <ContentBoxStyled>
                    <HeadingBoxStyled>
                        <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
                            <UserTitleStyled>{getShortName(user)}</UserTitleStyled>
                            <FlagIcon countryCode={user.country?.countryCode} />
                        </Box>
                        <Box sx={{ display: "flex", gap: "4px", alignItems: "center" }}>
                            <IconStyled><StarIcon /></IconStyled>
                            <UserRatingStyled>{getUserRating(user)}</UserRatingStyled>
                        </Box>
                    </HeadingBoxStyled>
                    <SubjectBoxStyled>
                        <TutorSubjectBox user={user} />
                    </SubjectBoxStyled>
                    <PriceBoxStyled>
                        <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_ITEM.TRIAL_LESSON)}</Body1Styled>
                        <UserPriceStyled>{localizeTrial({ price: user.hourRate })}</UserPriceStyled>
                    </PriceBoxStyled>
                </ContentBoxStyled>
            </Link>
        </ItemBoxStyled>
    )
}

export default TutorItemSmall;
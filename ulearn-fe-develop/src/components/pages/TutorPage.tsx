import { styled, experimental_sx as sx } from "@mui/material/styles";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { AttributeTypeEnum } from "core/constants/enums/AttributeTypeEnum";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { getEmbededId, getYoutubeImageUrlFromQuery, getYoutubeVideoUrlFromQuery } from "core/services/Utils/youtubeUtil";
import React, { useEffect, useState, useContext, useMemo, useCallback } from "react";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import {
    getUserAttribute,
    getUserRating,
    getShortenDescription,
    getDescription,
    getUserName,
    getShortName
} from "core/services/Utils/userUtils";
import { UserRatingStyled } from "components/styles/user/UserRatingStyled";
import UserReview from "components/profile/reviews/Review";
import AvailabilityCalendar from "components/profile/calendar/AvailabilityCalendar";
import { useContactUserModalContext } from "components/modals/ContactUserModalProvider";
import ContactUserModalProvider from "components/modals/ContactUserModalProvider";
import { SecondaryButton } from "components/styles/SecondaryButton";
import { TextButtonStyled } from "components/styles/TextButton";
import Footer from "components/landingPage/Footer";
import Copyright from "components/landingPage/Copyright";
import H3Styled from "components/styles/typography/H3Styled";;
import PlayCircleIcon from "assets/outline-play-circle-outline.svg";
import { PublicTutorInfoResponse, TutorAdminInfoResponseBody, adminEditAccountAccount, selectShortUserInfo, selectTutorAdminInfo, selectTutorAdminInfoStatus, setTutorAdminInfo } from "core/redux/userSlice";
import { MobileBoxStyled } from "components/styles/boxes/MobileBoxStyled";
import TutorLanguagesBox from "components/tutor/TutorLanguagesBox";
import TutorSubjectBox from "components/tutor/TutorSubjectBox";
import { UIContext } from "components/core/UIContextProvider";
import { motion } from "framer-motion";
import ReservationContinueButton from "components/profile/ReservationContinueButton";
import ReservationModalProvider, { useReservationModalContext } from "components/modals/ReservationModalProvider";
import Body1Styled from "components/styles/typography/Body1Styled";
import H4Styled from "components/styles/typography/H4Styled";
import StarIcon from "assets/star-filled.svg";
import TutorItemVerified from "components/tutor/TutorItemVerified";
import FlagIcon from "components/core/FlagIcon";
import { NextSeo } from 'next-seo';
import { useTranslation } from "components/core/TranslationProvider";
import { withAppProvider } from "components/core/providers/AppProvider";
import { useRouter } from "next/router";
import { paths } from "apiTypes";
import { DefaultPagePropsType } from "core/services/Utils/ssrUtil";
import FileService from "core/services/FileService";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { FileTypeEnum } from "core/constants/enums/FileTypeEnum";
import moment from "moment";
import { TutorItemRowBoxStyled } from "components/tutor/styles/TutorItemRowBoxStyled";
import { FloatableBoxStyled } from "components/styles/boxes/FloatableBoxStyled";
import OnlineIconBoxStyled from "components/styles/boxes/OnlineIconBoxStyled";
import TutorItemBoxWithStatusStyled from "components/tutor/styles/TutorItemBoxWithStatusStyled";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import Image from "next/image";
import Link from "next/link";
import PStyled from "components/styles/typography/PStyled";
import { GetTutorPublicStatisticsResponse } from "core/redux/userStatisticsSlice";
import InjectIcon from "components/core/icons/InjectIcon";
import { BadgeTextBoxStyled } from "components/styles/boxes/BadgeTextBoxStyled";
import { getFullUrlWithLocale } from "core/services/Utils/urlUtil";
import { getSeoLanguageAlternatives } from "components/core/Seo";
import { getOffsetTopRecursively } from "core/services/Utils/htmlElementUtil";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import ErrorLarge from "components/core/error/ErrorLarge";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { isDispatchFulfilled } from "core/services/Utils/apiUtil";
import GoogleOneTap from "components/auth/GoogleOneTap";
import ChatIcon from "assets/chat.svg";
import SimilarTutorsOptional from "components/tutor/SimilarTutorsOptional";
import TutorPackageSelection from "components/profile/TutorPackageSelection";
import { PrimaryButtonCompacted } from "components/styles/PrimaryButtonCompacted";
import TextField from "@mui/material/TextField";
import { selectIsAuthorized } from "core/redux/authSlice";
import { createTutorProfileVisited } from "core/redux/userBehaviourSlice";
import { usePriceLocalization } from "core/hooks/priceHook";

const InnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    padding: { xs: "16px 0px", md: "16px" },
    gap: "16px",
    backgroundColor: COLORS.WHITE,
    borderRadius: "8px"
})) as typeof Box;

const IconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const RightBoxOuterStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "24px",
    position: "sticky",
    top: "16px"
}) as typeof Box;

const RightBoxStyled = styled(Box)({
    width: "100%",
    backgroundColor: COLORS.WHITE,
    borderRadius: "8px",
    border: `1px solid ${COLORS.BLACK_10}`
}) as typeof Box;

const DescriptionBoxStyled = styled(Box)({
    display: "flex",
    width: "100%",
    flexFlow: "column noWrap",
    gap: "16px",
    color: COLORS.BLACK,
    fontSize: "1rem",
    lineHeight: "1.1875rem",
    textAlign: "justify"
}) as typeof Box;

const RatingBoxStyled = styled(Box)({
    padding: "8px",
    flexFlow: "row noWrap",
    display: "flex",
    gap: "4px"
}) as typeof Box;

const SectionBoxStyled = styled(Box)({
    borderBottom: `1px solid ${COLORS.BLACK_10}`,
    paddingBottom: "12px"
}) as typeof Box;

const SectionTitleStyled = styled(PStyled)({
    fontWeight: 600,
    color: COLORS.BLACK
}) as typeof PStyled;

const SectionDescriptionTypographyStyled = styled(Body1Styled)({
    whiteSpace: "pre-line",
    color: COLORS.BLACK
}) as typeof Body1Styled;

const RightAreaYoutubeBox = styled(Box)({
    width: "100%",
    aspectRatio: "16/9"
}) as typeof Box;

const RightAreaPriceTextStyled = styled(Typography)({
    color: COLORS.PRIMARY,
    fontSize: "1.5rem",
    fontWeight: "700",
    lineHeight: "1.8125rem",
    textAlign: "right"
}) as typeof Typography;

const AvatarStyled = styled(Avatar)({
    width: "72px",
    height: "72px"
}) as typeof Avatar;

const StickyButtonsBoxStyled = styled(Box)({
    position: "fixed",
    bottom: 0,
    padding: "24px",
    width: "100%",
    boxShadow: `0 4px 30px ${COLORS.BLACK_10}`,
    transition: "transform .15s cubic-bezier(.22,.61,.36,1) .1s",
    display: "flex",
    alignItems: "center",
    justifyContent: "space-between",
    zIndex: 100,
    backgroundColor: COLORS.WHITE,
}) as typeof Box;

const ProfilePhotoGridStyled = styled(Grid)({
    position: "relative",
    "span": {
        width: "100% !important",
    },
    "img": {
        objectFit: "cover",
        borderRadius: "8px",
        width: "100%",
        height: "auto"
    }
}) as typeof Grid;

const ReviewsBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    gap: "16px"
}) as typeof Box;

const MessageButtonBoxStyled = styled(Box)({
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "8px",
    padding: "8px",
    width: "100%"
}) as typeof Box;

const MessageButtonStyled = styled(SecondaryButton)({
    display: "inline-flex",
    gap: "8px",
    alignContent: "center",
    borderRadius: "4px",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_10,
        color: COLORS.PRIMARY
    }
}) as typeof SecondaryButton;

const VideoPreviewBoxStyled = styled(Box)(() => sx({
    position: "relative",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    color: COLORS.WHITE,
    width: "100%",
    height: "100%",
    backgroundRepeat: "no-repeat",
    backgroundPosition: "center",
    backgroundSize: "cover",
    borderRadius: { xs: "4px", md: "8px 8px 0 0" },
    svg: {
        transform: "scale(2.5)",
        filter: "drop-shadow(3px 5px 2px rgb(0 0 0 / 0.4))"
    },
    ":hover": {
        cursor: "pointer",
    },
    ":hover::before": {
        position: "absolute",
        content: '""',
        width: "100%",
        height: "100%",
    }
})) as typeof Box;

const SpecializationItemBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    padding: "4px 8px",
    borderRadius: "4px",
    a: {
        color: COLORS.PRIMARY
    }
}) as typeof Box;

const StatisticsItemBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    alignItems: "center",
}) as typeof Box;

const StatisticsReviewItemBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "row noWrap",
    alignItems: "center",
    gap: "4px"
}) as typeof Box;

const StatisticsBoxStyled = styled(Box)({
    padding: "16px 0px",
    display: "flex",
    justifyContent: "space-between"
}) as typeof Box;

const SpecializationBoxStyled = styled(Box)({
    padding: "16px 0px",
    display: "flex",
    flexFlow: "row wrap",
    gap: "8px"
}) as typeof Box;

const HeaderItemsStyled = styled(Box)({
    display: "flex",
    gap: "16px",
    flexFlow: "column noWrap"
}) as typeof Box;

const HeaderItemStyled = styled(Box)({
    display: "flex",
    gap: "18px",
    flexFlow: "column noWrap"
}) as typeof Box;

const HeaderMobileBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "row noWrap",
    alignItems: "center",
    gap: "16px"
}) as typeof Box;

const TextFieldStyled = styled(TextField)({
    width: "60px",
    border: `2px solid ${(COLORS.PRIMARY)}`,
    borderRadius: "4px",
    "input": { padding: "4px 8px" },
    "fieldset": { border: "none" }
}) as typeof TextField;


type ContentPageProps = DefaultPagePropsType & {
    userInfo: paths['/api/user']['get']['responses']['200']['content']['application/json'],
    statistics: paths['/api/user-statistics/tutor/{userId}']['get']['responses']['200']['content']['application/json']
}

const variants = {
    hidden: {
        opacity: 0,
        scale: 0,
        display: "none"
    },
    shown: {
        opacity: 1,
        scale: 1,
        display: "block",
    },
};

const inputFieldNames = {
    certificatesVerified: "certificatesVerified",
    excluded: "excluded",
    orderNumber: "orderNumber",
}

const VideoBox = ({ user }: { user: PublicTutorInfoResponse }) => {
    const [showVideoBox, setShowVideoBox] = React.useState(true);
    const [videoOn, setVideoOn] = React.useState(false);
    const [offset, setOffset] = useState(0);
    const [height, setHeight] = useState(0);
    const imageUrl = useMemo(() => getYoutubeImageUrlFromQuery(getEmbededId(getUserAttribute(user, AttributeTypeEnum.VideoUrl)?.value ?? "") ?? ""), []);
    const videoUrl = useMemo(() => getYoutubeVideoUrlFromQuery(getEmbededId(getUserAttribute(user, AttributeTypeEnum.VideoUrl)?.value ?? "") ?? ""), []);

    const currentRefCallback = useCallback((node: HTMLElement | null) => {

        if (node !== null) {
            const newOffset = getOffsetTopRecursively(node);
            setOffset(newOffset);
            setHeight(node.scrollHeight);
        }
    }, []);

    useEffect(() => {
        window.addEventListener('scroll', handleScroll);

        return () => {
            window.removeEventListener('scroll', handleScroll);
        };
    }, [offset]);

    const handleScroll = () => {
        const threshold = offset;
        const isShown = window.scrollY <= threshold;
        setShowVideoBox(isShown);
    };

    return (
        <Box ref={currentRefCallback}>
            {user.attributes?.some(x => x.attributeType?.attributeTypeId === AttributeTypeEnum.VideoUrl) ? (
                <RightAreaYoutubeBox component={motion.div} variants={variants} initial={"shown"} animate={showVideoBox ? "shown" : "hidden"}>
                    {
                        videoOn ? (
                            <iframe
                                style={{ "width": "100%", height: `${height}px`, borderRadius: "8px 8px 0 0" }}
                                src={videoUrl}
                                frameBorder="0"
                                allowFullScreen={true}
                                title="YouTube video player"
                            />
                        ) : (
                            <VideoPreviewBoxStyled sx={{ backgroundImage: `url(${imageUrl})` }} onClick={() => { setVideoOn(true) }}>
                                <PlayCircleIcon />
                            </VideoPreviewBoxStyled>
                        )
                    }
                </RightAreaYoutubeBox>
            ) : <></>}
        </Box>
    )
}

const MessageButton = () => {
    const { setOpen } = useContactUserModalContext();;
    const { t } = useTranslation();
    return (
        <MessageButtonBoxStyled>
            <MessageButtonStyled fullWidth onClick={setOpen}><ChatIcon />{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.WRITE_A_MESSAGE)}</MessageButtonStyled>
        </MessageButtonBoxStyled>
    )
}

const UserReviews = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();
    return (
        <ReviewsBoxStyled>
            <SectionBoxStyled><SectionTitleStyled>{`${t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.REVIEWS)} (${user.reviews?.length ?? 0})`}</SectionTitleStyled></SectionBoxStyled>
            {user.reviews ? user.reviews.map((review, idx) => (<UserReview key={review!.userReviewId!} userReview={review} />)) : <></>}
        </ReviewsBoxStyled>
    )
}

const Specializations = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();

    return (
        <>
            <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.SPECIALIAZATIONS)}</SectionTitleStyled></SectionBoxStyled>
            <SpecializationBoxStyled>
                {user.specializations?.map((x, idx) => (
                    <SpecializationItemBoxStyled key={x.specializationId!}>
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(x.nameKey ?? "")}</Body1Styled>
                        <InjectIcon category="specializations" iconName={x.iconName ?? ""} />
                    </SpecializationItemBoxStyled>
                ))}
            </SpecializationBoxStyled>
        </>
    )
}

const Interests = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();

    return (
        <>
            <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.INTERESTS)}</SectionTitleStyled></SectionBoxStyled>
            <SpecializationBoxStyled>
                {user.interests?.map((x, idx) => (
                    <SpecializationItemBoxStyled key={x.interestId!}>
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(x.nameKey ?? "")}</Body1Styled>
                        <InjectIcon category="interests" iconName={x.iconName ?? ""} />
                    </SpecializationItemBoxStyled>
                ))}
            </SpecializationBoxStyled>
        </>
    )
}

const Certificates = ({ user }: { user: PublicTutorInfoResponse }) => {
    const certificates = user.files?.filter(x => x.fileTypeId === FileTypeEnum.Certificate);
    const { t } = useTranslation();

    return (
        <>
            <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.CERTIFICATES)}</SectionTitleStyled></SectionBoxStyled>
            <SpecializationBoxStyled>
                {certificates ? certificates.map((x, idx) => (
                    <SpecializationItemBoxStyled key={idx}>
                        <Link href={FileService.getFileUrl(x.filePath ?? "")}>{x.fileName ?? ""}</Link>
                    </SpecializationItemBoxStyled>
                )) : <></>}
            </SpecializationBoxStyled>
        </>
    )
}

const LanguageLevel = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();

    return (
        <>
            <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.LANGUAGE_LEVEL_SPECIALIAZATIONS)}</SectionTitleStyled></SectionBoxStyled>
            <SpecializationBoxStyled>
                {user.subjectLevels?.map((x, idx) => (
                    <SpecializationItemBoxStyled key={x.subjectLevelId!}>
                        <InjectIcon category="subjectLevels" iconName={x.iconName ?? ""} />
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(x.tutorNameKey ?? "")}</Body1Styled>
                    </SpecializationItemBoxStyled>
                ))}
            </SpecializationBoxStyled>
        </>
    )
}

const Description = ({ user }: { user: PublicTutorInfoResponse }) => {
    const COUNT_OF_DISPLAYED_CHARACTERS_IN_DESCRIPTION = 400;
    const [showFullDescription, setShowFullDescription] = React.useState(false);
    const description = getDescription(user);
    const shortDescription = getShortenDescription(user, COUNT_OF_DISPLAYED_CHARACTERS_IN_DESCRIPTION);
    const hasMoreCharacters = shortDescription.length < (description?.length ?? 0)
    const { t } = useTranslation();

    const handleShowMoreClick = () => {
        setShowFullDescription(!showFullDescription);
    }

    return (
        <>
            <DescriptionBoxStyled>
                <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ABOUT_ME)}</SectionTitleStyled></SectionBoxStyled>
                <SectionDescriptionTypographyStyled>{!showFullDescription && hasMoreCharacters ? shortDescription : description}</SectionDescriptionTypographyStyled>
            </DescriptionBoxStyled>
            {hasMoreCharacters ? (
                <TextButtonStyled variant="text" onClick={handleShowMoreClick}> {
                    !showFullDescription
                        ? t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.SHOW_MORE_BUTTON)
                        : t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.SHOW_LESS_BUTTON)
                }
                </TextButtonStyled>
            ) : <></>}

        </>
    )
}

const Statistics = ({ user, statistics }: { user: PublicTutorInfoResponse, statistics: GetTutorPublicStatisticsResponse }) => {
    const { t } = useTranslation();

    return (
        <>
            <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.STATISTICS)}</SectionTitleStyled></SectionBoxStyled>
            <StatisticsBoxStyled>
                <StatisticsItemBoxStyled>
                    <H4Styled>{statistics?.numberOfLessons ?? 0}</H4Styled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.STATISTICS_LESSONS)}</Body1Styled>
                </StatisticsItemBoxStyled>
                <StatisticsItemBoxStyled>
                    <H4Styled>{statistics?.attendancePercentage ?? 0}%</H4Styled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.STATISTICS_ATTENDENCE)}</Body1Styled>
                </StatisticsItemBoxStyled>
                <StatisticsItemBoxStyled>
                    <H4Styled>{statistics?.numberOfStudents ?? 0}</H4Styled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.STATISTICS_STUDENTS)}</Body1Styled>
                </StatisticsItemBoxStyled>
                <StatisticsItemBoxStyled>
                    <StatisticsReviewItemBoxStyled>
                        <H4Styled>{getUserRating(user)}</H4Styled>
                        <IconStyled><StarIcon /></IconStyled>
                    </StatisticsReviewItemBoxStyled>
                    <Body1Styled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.STATISTICS_RATING)}</Body1Styled>
                </StatisticsItemBoxStyled>
            </StatisticsBoxStyled>
        </>
    )
}

const Online = () => {
    const { t } = useTranslation();
    return (
        <TutorItemRowBoxStyled>
            <FloatableBoxStyled>
                <OnlineIconBoxStyled />
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.ONLINE)}</Body1Styled>
            </FloatableBoxStyled>
        </TutorItemRowBoxStyled>
    )
}

const LastVisitedDate = ({ statistics }: { statistics: GetTutorPublicStatisticsResponse }) => {
    const { t } = useTranslation();
    const lastSeen = statistics.lastVisited;
    const lastSeenResolved = moment(lastSeen).add(3, 'days') > moment() ? moment(lastSeen) : moment().add(-3, 'days');

    return (
        <>
            {statistics?.isOnline ? <Online /> : <></>}
            {!statistics?.isOnline && statistics?.lastVisited ? <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_ITEM.VISITED, { date: lastSeenResolved.fromNow() })}</Body1Styled> : <></>}
        </>
    )
}

const TutorNameWithFlag = ({ user, shortenName = false }: { user: PublicTutorInfoResponse, shortenName?: boolean }) => {

    return (
        <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
            <H3Styled>{!shortenName ? getUserName(user) : getShortName(user)}</H3Styled>
            <FlagIcon countryCode={user.country?.countryCode} />
        </Box>
    )
}

const UserRating = ({ user }: { user: PublicTutorInfoResponse }) => {

    return (
        <Box sx={{ display: "flex", justifyContent: "end" }}>
            <RatingBoxStyled>
                <IconStyled><StarIcon /></IconStyled>
                <UserRatingStyled>{getUserRating(user)}</UserRatingStyled>
            </RatingBoxStyled>
        </Box>
    )
}

const AdminInfoRow = ({ titleKey, value }: { titleKey: string, value: string | null | undefined }) => {
    const { t } = useTranslation();
    return (
        <TutorItemRowBoxStyled sx={{ width: "100%" }}>
            <FloatableBoxStyled>
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(titleKey)}</Body1Styled>
            </FloatableBoxStyled>
            <BadgeTextBoxStyled>
                <Body1Styled sx={{ color: COLORS.BLACK }}>{value ?? ""}</Body1Styled>
            </BadgeTextBoxStyled>
        </TutorItemRowBoxStyled>
    )
}

const AdminBooleanRow = ({ titleKey, name, value }: { titleKey: string, name: string, value: boolean }) => {
    const { t } = useTranslation();
    const [currentValue, setCurrentValue] = useState(value);

    useEffect(() => {
        setCurrentValue(value);
    }, [value]);

    return (
        <TutorItemRowBoxStyled sx={{ width: "100%", gap: "0px" }}>
            <FloatableBoxStyled>
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(titleKey)}</Body1Styled>
            </FloatableBoxStyled>
            <MuiCheckboxStyled checked={currentValue} name={name} onClick={() => setCurrentValue(prev => !prev)} />
        </TutorItemRowBoxStyled>
    )
}

const AdminNumberInputRow = ({ titleKey, name, value }: { titleKey: string, name: string, value: number }) => {
    const { t } = useTranslation();

    return (
        <TutorItemRowBoxStyled sx={{ width: "100%", }}>
            <FloatableBoxStyled>
                <Body1Styled sx={{ color: COLORS.BLACK }}>{t(titleKey)}</Body1Styled>
            </FloatableBoxStyled>
            <TextFieldStyled type="number" name={name} defaultValue={String(value)} />
        </TutorItemRowBoxStyled>
    )
}

const AdminInfoLoaded = ({ user }: { user: PublicTutorInfoResponse & TutorAdminInfoResponseBody }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const { enqueueSnackbar } = useSnackbar();
    const router = useRouter();

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const formData = new FormData(event.currentTarget);
        const fieldValues = Object.fromEntries(formData.entries());
        const certificationsVerified = fieldValues[inputFieldNames.certificatesVerified] !== undefined;
        const excluded = fieldValues[inputFieldNames.excluded] !== undefined;
        const orderNumber = parseInt(fieldValues[inputFieldNames.orderNumber] as string);

        const status = await dispatch(adminEditAccountAccount({
            userId: user.userId!,
            certificationsVerified,
            excluded,
            orderNumber
        }))

        if (isDispatchFulfilled(status)) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.SUCCESSFULLY_UPDATED), { variant: "success" });
        }
        else {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.FAILED_TO_UPDATE), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
        }
        router.replace(router.asPath);
    }

    return (
        <>
            <DescriptionBoxStyled>
                <SectionBoxStyled><SectionTitleStyled>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.TITLE)}</SectionTitleStyled></SectionBoxStyled>
            </DescriptionBoxStyled>
            <form onSubmit={handleSubmit}>
                <SpecializationBoxStyled>
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.EMAIL} value={user.email} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.PHONE} value={user.phone} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.TIMEZONE} value={user.timezone} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.TEACHING_EXPERIENCE} value={t(user.teachingExperience?.nameKey ?? "")} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.ICO} value={user.ico} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.IS_VAT_PAYER} value={user.isVatPayer ? t(TRANSLATION_KEYS.COMMON.YES) : t(TRANSLATION_KEYS.COMMON.NO)} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.DIC} value={user.dic} />
                    <AdminInfoRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.PLACE_OF_BUSINESS} value={user.businessAddress} />
                    <AdminBooleanRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.CERTIFICATES_VERIFIED} value={user.certificationsVerified ?? false} name={inputFieldNames.certificatesVerified} />
                    <AdminBooleanRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.EXLUDED} value={user.excluded ?? false} name={inputFieldNames.excluded} />
                    <AdminNumberInputRow titleKey={TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.ADMIN_INFORMATION.ORDER_NUMBER} value={user.orderNumber ?? 0} name={inputFieldNames.orderNumber} />
                    <PrimaryButtonCompacted type="submit">{t(TRANSLATION_KEYS.COMMON.SAVE)}</PrimaryButtonCompacted >
                </SpecializationBoxStyled>
            </form>
        </>
    )
}

const AdminInfoLoading = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", width: "100%", height: "120px" }}>
        <CircularProgressCustom />
    </Box>
)

const AdminInfoError = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", width: "100%" }}>
        <ErrorLarge />
    </Box>
)

const AdminInfo = ({ user }: { user: PublicTutorInfoResponse }) => {
    const tutorAdminInfo = useAppSelector(selectTutorAdminInfo);
    const tutorAdminInfoStatus = useAppSelector(selectTutorAdminInfoStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setTutorAdminInfo({ userId: user.userId! }));
    }, [user]);

    return (
        <>
            {tutorAdminInfoStatus === LoadingStatusEnum.Error ? <AdminInfoError /> : <></>}
            {tutorAdminInfoStatus === LoadingStatusEnum.Loading ? <AdminInfoLoading /> : <></>}
            {tutorAdminInfoStatus === LoadingStatusEnum.Loaded && tutorAdminInfo ? <AdminInfoLoaded user={{ ...user, ...tutorAdminInfo }} /> : <></>}
        </>
    )
}

const PackagesDesktop = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { handleOpen } = useReservationModalContext();

    return (
        <>
            <TutorPackageSelection tutorInfo={{ hourRate: user.hourRate, currencyCode: user.currency?.enumKey, teachesLessonsForChildren: user.teachesLessonsForChildren }} variant="small" />
            <ReservationContinueButton onClickHandler={handleOpen} />
        </>
    )
}

const TutorPageDesktop = ({ user, statistics }: { user: PublicTutorInfoResponse, statistics: GetTutorPublicStatisticsResponse }) => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <>
            <ContainerStyled sx={{ padding: "16px 0px" }} >
                <Grid container columns={13} columnSpacing="24px">
                    <Grid item xs={9}>
                        <InnerBoxStyled sx={{ border: `1px solid ${COLORS.BLACK_10}` }}>
                            <Grid container columnSpacing="16px" rowSpacing="16px" columns={15}>
                                <ProfilePhotoGridStyled item xs={4}>
                                    <TutorItemBoxWithStatusStyled>
                                        <Image src={getProfilePhotoUrl(user)} placeholder="blur" blurDataURL="/images/gray_placeholder.png" width={189} height={189} alt="Profile photo" />
                                        {/* <TutorLastVisitedStatus isOnline={statistics?.isOnline} lastSeen={statistics?.lastVisited} /> */}
                                    </TutorItemBoxWithStatusStyled>
                                </ProfilePhotoGridStyled>
                                <Grid item xs={9}>
                                    <HeaderItemsStyled>
                                        <TutorNameWithFlag user={user} />
                                        <HeaderItemStyled>
                                            <TutorSubjectBox user={user} />
                                            <TutorLanguagesBox user={user} />
                                            {user.certificationsVerified ? <TutorItemVerified /> : <></>}
                                            <LastVisitedDate statistics={statistics} />
                                        </HeaderItemStyled>
                                    </HeaderItemsStyled>
                                </Grid>
                                <Grid item xs={2}>
                                    <UserRating user={user} />
                                </Grid>
                                {shortUserInfo?.userRoleId === UserRoleEnum.Admin ? (
                                    <Grid item xs={15}>
                                        <AdminInfo user={user} />
                                    </Grid>
                                ) : <></>}
                                <Grid item xs={15}>
                                    <Description user={user} />
                                </Grid>
                                <Grid item xs={15}>
                                    <Specializations user={user} />
                                </Grid>
                                <Grid item xs={15}>
                                    <Interests user={user} />
                                </Grid>
                                {shortUserInfo?.userRoleId === UserRoleEnum.Admin ? (
                                    <Grid item xs={15}>
                                        <Certificates user={user} />
                                    </Grid>
                                ) : <></>}
                                <Grid item xs={15}>
                                    <LanguageLevel user={user} />
                                </Grid>
                                {/* <Grid item xs={15}><Statistics user={user} statistics={statistics} /></Grid> */}
                                <Grid item xs={15}>
                                    <AvailabilityCalendar userId={user.userId!} displayMode="regular" />
                                </Grid>
                                {user?.reviews && user?.reviews?.length ? (
                                    <Grid item xs={15}>
                                        <UserReviews user={user} />
                                    </Grid>
                                ) : <></>}
                            </Grid>
                        </InnerBoxStyled>
                    </Grid>
                    <Grid item xs={4}>
                        <RightBoxOuterStyled>
                            <RightBoxStyled >
                                <VideoBox user={user} />
                                <InnerBoxStyled>
                                    <PackagesDesktop user={user} />
                                </InnerBoxStyled>
                            </RightBoxStyled>
                            <MessageButton />
                        </RightBoxOuterStyled>
                    </Grid>
                </Grid >
            </ContainerStyled>
            <SimilarTutorsOptional userId={user.userId!} />
        </>
    )
};

const StickyButtons = ({ user }: { user: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();
    const { setOpen } = useContactUserModalContext();
    const { handleOpen: handleReservationOpen } = useReservationModalContext();
    const { localizeTrial } = usePriceLocalization();

    return (
        <StickyButtonsBoxStyled>
            <RightAreaPriceTextStyled>{localizeTrial({ price: user.hourRate })}</RightAreaPriceTextStyled>
            <Box sx={{ display: "flex", gap: "8px" }}>
                <MessageButtonStyled sx={{ padding: "8px 16px" }} onClick={setOpen}>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.WRITE_A_MESSAGE)}</MessageButtonStyled>
                <ReservationContinueButton onClickHandler={handleReservationOpen} />
            </Box>
        </StickyButtonsBoxStyled>
    )
}

const TutorPageMobile = ({ user, statistics }: { user: PublicTutorInfoResponse, statistics: GetTutorPublicStatisticsResponse }) => {
    return (
        <MobileBoxStyled sx={{ position: "relative" }}>
            <ContainerStyled>
                <Grid container columns={15}>
                    <InnerBoxStyled>
                        <Grid container rowSpacing="16px" spacing={4} columns={15}>
                            <Grid item xs={13}>
                                <HeaderMobileBoxStyled>
                                    <TutorItemBoxWithStatusStyled>
                                        <AvatarStyled src={getProfilePhotoUrl(user)} alt="Tutor's profile photo" />
                                        {/* <TutorLastVisitedStatus isOnline={statistics?.isOnline} lastSeen={statistics?.lastVisited} /> */}
                                    </TutorItemBoxWithStatusStyled>
                                    <TutorNameWithFlag user={user} shortenName={true} />
                                </HeaderMobileBoxStyled>
                            </Grid>
                            <Grid item xs={2} sx={{ display: "flex", justifyContent: "end", alignItems: "center" }}>
                                <UserRating user={user} />
                            </Grid>
                            <Grid item xs={15} sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
                                <TutorSubjectBox user={user} />
                                <TutorLanguagesBox user={user} />
                                {user.certificationsVerified ? <TutorItemVerified /> : <></>}
                                <LastVisitedDate statistics={statistics} />
                            </Grid>
                            <Grid item xs={15}>
                                <Description user={user} />
                            </Grid>
                            <Grid item xs={15}>
                                <Specializations user={user} />
                            </Grid>
                            <Grid item xs={15}>
                                <Interests user={user} />
                            </Grid>
                            <Grid item xs={15}>
                                <LanguageLevel user={user} />
                            </Grid>
                            {/* <Grid item xs={15}>
                                <Statistics user={user} statistics={statistics} />
                            </Grid> */}
                            <Grid item xs={15}>
                                <VideoBox user={user} />
                            </Grid>
                            <Grid item xs={15}>
                                <AvailabilityCalendar userId={user.userId!} displayMode="regular" />
                            </Grid>
                            {user?.reviews && user?.reviews?.length ? (
                                <Grid item xs={15}>
                                    <UserReviews user={user} />
                                </Grid>
                            ) : <></>}
                        </Grid>
                    </InnerBoxStyled>
                </Grid>
            </ContainerStyled>
            <SimilarTutorsOptional userId={user.userId!} />
            <StickyButtons user={user} />
        </MobileBoxStyled>
    )
};

const TutorDescriptionPage = ({ user, statistics }: { user: PublicTutorInfoResponse, statistics: GetTutorPublicStatisticsResponse }) => {
    const { screenType } = useContext(UIContext);
    return (
        <ContactUserModalProvider userInfo={user} placeholderKey={TRANSLATION_KEYS.CONTACT_TUTOR_POPUP.CONTACT_TUTOR_DESCRIPTION}>
            <ReservationModalProvider tutorInfo={{
                userId: user.userId,
                hourRate: user.hourRate,
                currencyCode: user.currency?.enumKey,
                files: user.files,
                firstName: user.firstName,
                lastName: user.lastName,
                teachesLessonsForChildren: user.teachesLessonsForChildren
            }}>
                {screenType === "desktop" ? <TutorPageDesktop user={user} statistics={statistics} /> : <></>}
                {screenType === "mobile" ? <TutorPageMobile user={user} statistics={statistics} /> : <></>}
            </ReservationModalProvider>
        </ContactUserModalProvider>
    )
}


const Seo = ({ userInfo }: { userInfo: PublicTutorInfoResponse }) => {
    const { t } = useTranslation();
    const router = useRouter();
    const pageTitle = t(userInfo.subject?.tutorPublicPageTitleKey ?? "", { tutorName: getUserName(userInfo) });
    const pageDescription = t(userInfo.subject?.tutorPublicPageDescriptionKey ?? "", { tutorName: getUserName(userInfo) });
    const relativeUrl = router.pathname.replace('[...id]', `${userInfo.userId!}`);

    return (
        <NextSeo
            title={pageTitle}
            description={pageDescription}
            languageAlternates={getSeoLanguageAlternatives(router.locales, relativeUrl)}
            canonical={getFullUrlWithLocale(router.locale, relativeUrl)}
            openGraph={{
                url: getFullUrlWithLocale(router.locale, relativeUrl),
                title: pageTitle,
                description: pageDescription,
                locale: router.locale,
                type: "website",
                images: [{
                    url: getProfilePhotoUrl(userInfo),
                    width: 420,
                    height: 420,
                    alt: getUserName(userInfo)
                }]
            }}
        />
    )
}

const UserBehaviourLogger = ({ userInfo }: { userInfo: PublicTutorInfoResponse }) => {
    const isAuthorized = useAppSelector(selectIsAuthorized);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(createTutorProfileVisited({ userId: userInfo.userId! }));
    }, [isAuthorized]);

    return (
        <></>
    )
}

const TutorDescriptionPageFetch = ({ userInfo, statistics }: { userInfo: PublicTutorInfoResponse, statistics: GetTutorPublicStatisticsResponse }) => {

    return (
        <>
            <Seo userInfo={userInfo} />
            <GoogleOneTap role={UserRoleEnum.Student} autoRedirectToHomePage={false} />
            <UserBehaviourLogger userInfo={userInfo} />
            <Box sx={{ paddingBottom: { xs: "86px", md: "0px" } }}>
                <Navbar position="static" source={NavbarSourceEnum.studentPage} />
                <TutorDescriptionPage user={userInfo} statistics={statistics} />
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    );
}

export default withAppProvider<ContentPageProps>(TutorDescriptionPageFetch);
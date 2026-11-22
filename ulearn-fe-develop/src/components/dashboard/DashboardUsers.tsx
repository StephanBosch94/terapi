import { styled, experimental_sx as sx } from "@mui/material/styles";
import ContactUserModalProvider, { useContactUserModalContext } from "components/modals/ContactUserModalProvider";
import { SecondaryButtonCompacted } from "components/styles/SecondaryButtonCompacted";
import H5Styled from "components/styles/typography/H5Styled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { getUserName } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import ErrorLarge from "components/core/error/ErrorLarge";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import routes from "routes/routes";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { GetMyStudentsResponseBody, selectShortUserInfo } from "core/redux/userSlice";
import { useAppSelector } from "core/redux/reduxHooks";
import Avatar from "@mui/material/Avatar";
import Box from "@mui/material/Box";
import Link from "next/link";
import { PrimaryButtonCompacted } from "components/styles/PrimaryButtonCompacted";
import ReservationReviewModalProvider, { useUserReviewContext } from "components/modals/UserReviewModalProvider";
import Empty from "components/core/empty/Empty";
import StarsRating from "components/profile/reviews/StarsRating";
import MenuItem from "components/menu/MenuItem";
import Divider from '@mui/material/Divider';
import EditReviewIcon from "assets/edit.svg";
import { MenuBoxStyled } from "components/styles/boxes/MenuBoxStyled";
import React from "react";
import MessageIcon from "assets/chat.svg";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import ThreeDotsMenu from "components/menu/ThreeDotsMenu";


const AvatarStyled = styled(Avatar)(() => sx({
    width: "48px",
    height: "48px"
})) as typeof Avatar

const YourStudentsBoxStyled = styled(Box)(() => sx({
    width: "100%",
    borderRadius: "4px",
    display: "flex",
    border: `1px solid ${COLORS.BLACK_10}`,
    flexDirection: "column",
    padding: "16px",
    gap: { xs: "24px", md: "32px" }
})) as typeof Box;

const YourStudentListBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "column",
    borderRadius: "4px",
    backgroundColor: COLORS.WHITE,
})) as typeof Box;

const StudentItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    justifyContent: "space-between",
    gap: "8px",
    borderRadius: "4px",
    width: "100%",
})) as typeof Box;

const IconDotsStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY,
        transform: "scale(1.5)",
    }
})) as typeof Box;

const UserBoxStyled = styled(Box)(() => sx({
    display: "flex", alignItems: "center", gap: "16px",
    "a": {
        textDecoration: "none", color: COLORS.BLACK
    },
    "a:hover": {
        textDecoration: "underline",
        cursor: "pointer"
    }
})) as typeof Box;

type DashboardUsersDataType = {
    titleKey: string,
    descriptionKey: string,
    buttonTextKey: string,
    imagePath: string
};

const showReview = (userInfo: GetMyStudentsResponseBody["data"][0]) => {
    return userInfo.userRoleId == UserRoleEnum.Tutor && userInfo.numberOfCompletedLessons;
}

const UserReviewButton = ({ userInfo }: { userInfo: GetMyStudentsResponseBody["data"][0] }) => {
    const reviewButtonTitleKey = userInfo.userReview ? TRANSLATION_KEYS.DASHBOARD.EDIT_USER_REVIEW_BUTTON : TRANSLATION_KEYS.DASHBOARD.WRITE_USER_REVIEW_BUTTON;
    const { t } = useTranslation();
    const { setOpen: setUserReviewOpen } = useUserReviewContext();

    return showReview(userInfo) ? <SecondaryButtonCompacted onClick={() => setUserReviewOpen()}>{t(reviewButtonTitleKey)}</SecondaryButtonCompacted> : <></>;
}

const UserReviewMenuItem = ({ userInfo }: { userInfo: GetMyStudentsResponseBody["data"][0] }) => {
    const reviewButtonTitleKey = userInfo.userReview ? TRANSLATION_KEYS.DASHBOARD.EDIT_USER_REVIEW_BUTTON : TRANSLATION_KEYS.DASHBOARD.WRITE_USER_REVIEW_BUTTON;
    const { t } = useTranslation();
    const { setOpen: setUserReviewOpen } = useUserReviewContext();

    return showReview(userInfo) ? <Box><MenuItem onClick={() => setUserReviewOpen()} icon={<EditReviewIcon />} text={t(reviewButtonTitleKey)} /><Divider sx={{ color: COLORS.BLACK_5 }} /></Box> : <></>;
}

const UserReviewOptional = ({ userInfo }: { userInfo: GetMyStudentsResponseBody["data"][0] }) => {
    const { setOpen: setUserReviewOpen, selectedRating } = useUserReviewContext();

    const handleClick = (newRating: number) => {
        setUserReviewOpen(newRating);
    }

    return showReview(userInfo) ? (
        <StarsRating rating={selectedRating ?? 0} onClick={!userInfo.userReview ? handleClick : undefined} />
    ) : <></>
}

const UserListItem = ({ userInfo }: { userInfo: GetMyStudentsResponseBody["data"][0] }) => {
    const { t } = useTranslation();
    const { setOpen: setContactModalOpen } = useContactUserModalContext();
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return (
        <StudentItemBoxStyled >
            <UserBoxStyled>
                <AvatarStyled src={getProfilePhotoThumbnailUrl(userInfo)} />
                <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "4px" }}>
                    {shortUserInfo?.userRoleId === UserRoleEnum.Student ? (
                        <>
                            <Link href={routes.tutorIdPage.path(userInfo.userId!)}><H5Styled>{getUserName(userInfo)}</H5Styled></Link>
                            <UserReviewOptional userInfo={userInfo} />
                        </>
                    ) : <></>}
                    {shortUserInfo?.userRoleId === UserRoleEnum.Tutor ? (
                        <H5Styled>{getUserName(userInfo)}</H5Styled>
                    ) : <></>}
                </Box>
            </UserBoxStyled>
            <Box sx={{ display: { xs: "none", md: "flex" }, alignItems: "center", gap: "8px" }}>
                <UserReviewButton userInfo={userInfo} />
                <PrimaryButtonCompacted onClick={setContactModalOpen}>{t(TRANSLATION_KEYS.DASHBOARD.MESSAGE)}</PrimaryButtonCompacted>
            </Box>
            <Box sx={{ display: { xs: "flex", md: "none" } }}>
                <ThreeDotsMenu
                    innerMenu={
                        <MenuBoxStyled>
                            <UserReviewMenuItem userInfo={userInfo} />
                            <MenuItem onClick={setContactModalOpen} text={t(TRANSLATION_KEYS.DASHBOARD.MESSAGE)} icon={<MessageIcon />} />
                        </MenuBoxStyled>
                    }
                    autoClose={true}
                />
            </Box>
        </StudentItemBoxStyled >
    )
}

const UserListItemWithContext = ({ userInfo }: { userInfo: GetMyStudentsResponseBody["data"][0] }) => {
    return (
        <ContactUserModalProvider userInfo={userInfo}>
            <ReservationReviewModalProvider user={userInfo}>
                <UserListItem userInfo={userInfo} />
            </ReservationReviewModalProvider>
        </ContactUserModalProvider>
    )
}

const ListUsers = ({ users }: { users: GetMyStudentsResponseBody["data"] }) => {

    return (
        <>
            {users.map((x, idx) => (
                <YourStudentListBoxStyled key={x.userId!}>
                    <UserListItemWithContext key={x.userId!} userInfo={x} />
                </YourStudentListBoxStyled>
            ))}
        </>
    )
}

const DashboardUsersLoading = () => (
    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", minHeight: "280px" }}>
        <CircularProgressCustom />
    </Box>
)

const DashboardUsers = ({ users, usersStatus, content, onClick }: { users: GetMyStudentsResponseBody | undefined, usersStatus: LoadingStatusEnum | undefined, content: DashboardUsersDataType, onClick?: () => void }) => {

    return (
        <YourStudentsBoxStyled>
            {usersStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
            {usersStatus === LoadingStatusEnum.Loading ? <DashboardUsersLoading /> : <></>}
            {usersStatus === LoadingStatusEnum.Loaded && users && !users.size ? <Empty content={content} onClick={onClick} /> : <></>}
            {usersStatus === LoadingStatusEnum.Loaded && users && users.size ? <ListUsers users={users?.data} /> : <></>}
        </YourStudentsBoxStyled>
    )
}

export default DashboardUsers;
import Avatar from "@mui/material/Avatar";
import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import Body1Styled from "components/styles/typography/Body1Styled";
import H5Styled from "components/styles/typography/H5Styled";
import { UserWithFilesVM } from "core/services/Types/AppTypes";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { getUserNameOrEmail } from "core/services/Utils/userUtils";

const UserBoxStyled = styled(Box)({
    display: "flex",
    flexDirection: "row",
    alignItems: "center",
    gap: "16px",
}) as typeof Box;

const AvatarStyledSmall = styled(Avatar)({
    width: "40px",
    height: "40px"
}) as typeof Avatar

type UserType = {
    firstName?: string | null | undefined,
    lastName?: string | null | undefined,
    email?: string | null | undefined
} & UserWithFilesVM;


const UserBoxCell = ({ user, typeKey }: { user: UserType | undefined, typeKey?: string | null | undefined }) => {
    const { t } = useTranslation();

    return (
        <UserBoxStyled>
            <AvatarStyledSmall src={getProfilePhotoThumbnailUrl(user)} />
            <Box>
                <H5Styled>{getUserNameOrEmail(user)}</H5Styled>
                {typeKey ? <Body1Styled>{t(typeKey ?? "")}</Body1Styled> : <></>}
            </Box>
        </UserBoxStyled>
    )
}

export default UserBoxCell;
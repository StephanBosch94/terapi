import { styled, experimental_sx as sx } from "@mui/material/styles";
import { AppUserReview } from "core/services/Types/AppTypes";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { getUserName } from "core/services/Utils/userUtils";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import Moment from 'react-moment';
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import PStyled from "components/styles/typography/PStyled";
import StarsRating from "./StarsRating";

const UserNameStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK,
    fontWeight: 600,
})) as typeof Typography;

const CommentDescriptionStyled = styled(Body1Styled)(() => sx({
    color: COLORS.BLACK,
    textAlign: "justify",
})) as typeof Body1Styled;

const AvatarStyled = styled(Avatar)(() => sx({
    width: "48px",
    height: "48px"
})) as typeof Avatar;

const UserReview = ({ userReview }: { userReview: AppUserReview }) => {
    return (
        <Box>
            <Grid container>
                <Grid item xs={12}>
                    <Box sx={{ display: "flex", flexDirection: "row", gap: "16px" }}>
                        <AvatarStyled src={getProfilePhotoThumbnailUrl(userReview.reviewer)} />
                        <Box sx={{ display: "flex", flexDirection: "column", gap: "4px" }}>
                            <Box sx={{ display: "flex", gap: "8px", flexDirection: "row", alignItems: "center" }}>
                                <UserNameStyled>{getUserName(userReview.reviewer)}</UserNameStyled>
                                <CaptionTextStyled>
                                    <Moment fromNow>{userReview.created}</Moment>
                                </CaptionTextStyled>
                            </Box>
                            <StarsRating rating={userReview.rate ?? 0} />
                            <CommentDescriptionStyled>{userReview.comment ?? ""}</CommentDescriptionStyled>
                        </Box>
                    </Box>
                </Grid>
            </Grid>
        </Box>
    )
}

export default UserReview;
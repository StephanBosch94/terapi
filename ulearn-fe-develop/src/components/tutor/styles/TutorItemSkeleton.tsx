import Skeleton from "@mui/material/Skeleton";
import { experimental_sx as sx, styled } from "@mui/system";

const TutorItemSkeletonStyled = styled(Skeleton)(() => sx({
    borderRadius: "8px",
    width: "280px",
    height: "280px",
    display: "flex",
    flexShrink: "0"
})) as typeof Skeleton;

const TutorItemSkeleton = () => {

    return (
        <TutorItemSkeletonStyled variant="rectangular" animation="wave" />
    )
};

export default TutorItemSkeleton;
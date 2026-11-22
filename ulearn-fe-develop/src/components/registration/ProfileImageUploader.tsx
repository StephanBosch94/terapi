import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import ImageEditorProvider, { useImageEditModalContextType } from "components/modals/ImageEditorProvider";
import { PrimaryButtonCompacted } from "components/styles/PrimaryButtonCompacted";
import DropBoxProvider, { useDropBoxContext } from "components/styles/boxes/DropBoxProvider";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { UserInfoResponse, selectUploadProfilePhotoStatus, selectUserInfoErrors, selectUserInfoFiles, uploadProfilePhoto } from "core/redux/userSlice";
import { getFileUrl, getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { useSnackbar } from "notistack";
import { useState, useRef, useEffect } from "react";
import ImageIcon from "assets/image.svg";
import EditIcon from "assets/edit.svg";
import { IconButton } from "components/styles/IconButton";
import Box from "@mui/material/Box";
import Skeleton from "@mui/material/Skeleton";
import { FileTypeEnum } from "core/constants/enums/FileTypeEnum";

const UploadImageBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexDirection: "row",
    gap: "24px",
    alignItems: "center",
    width: "100%",
    borderRadius: "4px"
})) as typeof Box;

const ImageIconStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    svg: {
        transform: "scale(2.5)",
        color: COLORS.BLACK
    }
})) as typeof Box;

const EditIconBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    bottom: "8px",
    right: "8px",
    zIndex: 1,
    display: "inline-flex",
    gap: "4px",
    color: COLORS.WHITE
}));

const IconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    svg: {
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const DroppableBoxSkeleton = () => (<Skeleton variant="rectangular" animation="wave" width={imageDimensions.width} height={imageDimensions.height} />);

const imageDimensions = {
    width: "166px",
    height: "166px"
}

const DroppableBoxStyled = ({ imageUrl }: { imageUrl: string }) => {
    const { isDragActive } = useDropBoxContext();
    const { setOpen } = useImageEditModalContextType();
    const errors = useAppSelector(selectUserInfoErrors);
    const hasError = errors?.some(x => x === TRANSLATION_KEYS.ERRORS.MISSING_PROFILE_PHOTO);
    return (
        <>
            {!imageUrl || !imageUrl.length ? (
                <Box sx={{
                    padding: "75px",
                    border: hasError && !imageUrl ? `1px solid ${COLORS.RED}` : `1px dashed ${COLORS.BLACK_10}`,
                    backgroundColor: isDragActive ? COLORS.PRIMARY_10 : 'transparent'
                }}>
                    <ImageIconStyled><ImageIcon /></ImageIconStyled>
                </Box>
            ) : (
                <Box sx={{
                    width: imageDimensions.width,
                    height: imageDimensions.height,
                    position: "relative",
                    "img": { width: "100%", height: "100%", objectFit: "cover" },
                    opacity: isDragActive ? "0.5" : "1"
                }}>
                    <img src={imageUrl} alt="Profile image" style={{ borderRadius: "4px" }} />

                    <EditIconBoxStyled onClick={setOpen}>
                        <IconButton><IconBoxStyled><EditIcon /></IconBoxStyled></IconButton>
                    </EditIconBoxStyled>
                </Box>
            )}
        </>
    )
}

const ProfileImageUploader = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const userInfoFiles = useAppSelector(selectUserInfoFiles);
    const uploadProfilePhotoStatus = useAppSelector(selectUploadProfilePhotoStatus);
    const imageUrl = getFileUrl(userInfoFiles, FileTypeEnum.ProfilePhoto);
    
    const { enqueueSnackbar } = useSnackbar();
    const uploadInputRef = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        if (uploadProfilePhotoStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.FAILED_TO_UPLOAD_IMAGE), { variant: "error" });
        }
    }, [uploadProfilePhotoStatus]);

    const uploadImages = async (files: File[]) => {
        const file = files[0];
        await dispatch(uploadProfilePhoto(file));
    }

    const uploadImage = async (file: File) => uploadImages([file]);

    const handleUploadButtonClick = () => {
        if (!uploadInputRef.current) return;
        uploadInputRef.current.click()
    }

    const handleImageSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (!event.target || !event.target.files) return;
        uploadImages(Array.from(event.target.files));
    }

    return (
        <UploadImageBoxStyled>
            <DropBoxProvider multiple={false} onFilesDrop={uploadImages}>
                <ImageEditorProvider imageUrl={imageUrl} onSave={uploadImage}>
                    {uploadProfilePhotoStatus === LoadingStatusEnum.Loading ? <DroppableBoxSkeleton /> : <></>}
                    {uploadProfilePhotoStatus !== LoadingStatusEnum.Loading ? <DroppableBoxStyled imageUrl={imageUrl} /> : <></>}
                </ImageEditorProvider>
            </DropBoxProvider>
            <PrimaryButtonCompacted onClick={handleUploadButtonClick}>
                {!imageUrl.length ? t(TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_UPLOAD_PHOTO) : <></>}
                {imageUrl.length ? t(TRANSLATION_KEYS.TUTOR_SIGN_UP.ACCOUNT_UPLOAD_NEW_PHOTO) : <></>}
                <input ref={uploadInputRef} accept="image/jpg, image/jpeg, image/heif, image/heic, image/png" multiple={false} hidden type="file" onChange={handleImageSelect} />
            </PrimaryButtonCompacted>
        </UploadImageBoxStyled>
    )
}

export default ProfileImageUploader;
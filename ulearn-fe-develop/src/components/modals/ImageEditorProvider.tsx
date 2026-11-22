import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import ModalsHeader from "components/modals/common/ModalsHeader";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { SecondaryButtonRectangle } from "components/styles/SecondaryButtonRectangle";
import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import PStyled from "components/styles/typography/PStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate } from "core/redux/reduxHooks";
import { createContext, useState, useRef, useEffect } from "react";
import AvatarEditor from "react-avatar-editor";
import ArrowArcRight from "assets/arrow-arc-right.svg";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import Slider from "@mui/material/Slider";
import { ModalStyled } from "components/styles/ModalStyled";

const MuiSliderBox = styled(Box)({
    boxSizing: "content-box",
    padding: "0 10px 0 8px",
    display: "flex",
    alignItems: "center",
    flex: "1 1 auto"
}) as typeof Box;

const SliderStyled = styled(Slider)({
    color: COLORS.PRIMARY,
    width: "100%",
    padding: "0 !important",
    height: "20px",
    '& .MuiSlider-thumb': {
        height: "20px",
        width: "20px",
        backgroundColor: COLORS.WHITE,
        border: `1px solid ${COLORS.PRIMARY}`,
        '&:focus, &:hover, &.Mui-active': {
            boxShadow: "none"
        }
    },
    '& .MuiSlider-track': {
        border: 'none',
        height: "4px"
    },
    '& .MuiSlider-rail': {
        opacity: 1,
        backgroundColor: COLORS.BLACK_10,
        height: "4px",
    }
}) as typeof Slider;

type ImageEditModalContextType = {
    setOpen: () => void
} | undefined

const ImageEditModalContext = createContext<ImageEditModalContextType>(undefined);

const ImageEditorModalProvider = ({ imageUrl, children, onSave }: { imageUrl: string, children: any, onSave: (file: File) => void }) => {
    const [open, setOpen] = useState(false);
    const [scale, setScale] = useState(1.0);
    const [rotation, setRotation] = useState(0);
    const { t } = useTranslation();
    const editorRef = useRef<AvatarEditor>(null);

    // Reset values when new photo is updated
    useEffect(() => {
        setScale(1);
        setRotation(0);
    }, [imageUrl]);

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    const handleOpen = () => {
        setOpen(true);
    }

    const handleClose = () => {
        setOpen(false);
    }

    const handleScaleChange = (
        event: Event,
        newValue: number | number[],
        activeThumb: number,
    ) => {
        setScale(newValue as number)
    };

    const handleRotationClick = () => {
        setRotation((rotation + 90) % 360);
    }

    const handleSave = () => {
        if (editorRef.current) {
            const canvasScaled = editorRef.current.getImageScaledToCanvas();
            canvasScaled.toBlob((blob) => {
                if (blob) {
                    const file = new File([blob], 'canvas-image.jpg', { type: 'image/jpg' });
                    onSave(file);
                }
                setOpen(false);
            });
        }
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            handleSave();
        }
    }

    return (
        <ImageEditModalContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxSmallStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.TUTOR_SIGN_UP.IMAGE_EDITOR.TITLE)} handleClose={handleClose} />
                    <AvatarEditor
                        ref={editorRef}
                        image={imageUrl}
                        borderRadius={0}
                        border={0}
                        style={{ width: "100%", height: "100%" }}
                        scale={scale}
                        rotate={rotation}
                        crossOrigin="anonymous"
                    />
                    <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
                        <Box sx={{ display: "inline-flex", alignContent: "center", width: "100%", gap: "8px" }}>
                            <Box sx={{ display: "inline-flex", flex: "0 1 auto" }}>
                                <PStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.IMAGE_EDITOR.SCALE)}</PStyled>
                            </Box>
                            <MuiSliderBox >
                                <SliderStyled
                                    size="small"
                                    value={scale}
                                    min={1}
                                    max={5}
                                    step={0.1}
                                    onChange={handleScaleChange}
                                    aria-label="Small"
                                />
                            </MuiSliderBox>
                        </Box>
                        <Box sx={{ display: "inline-flex", alignItems: "center", width: "100%", gap: "8px" }}>
                            <PStyled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_SIGN_UP.IMAGE_EDITOR.ROTATE_BUTTON)}</PStyled>
                            <Box sx={{
                                display: "flex",
                                alignItems: "center",
                                padding: "4px",
                                borderRadius: "4px",
                                color: COLORS.PRIMARY,
                                border: `1px solid ${COLORS.PRIMARY}`,
                                background: COLORS.WHITE,
                                ":hover": {
                                    backgroundColor: COLORS.PRIMARY_10,
                                    cursor: "pointer"
                                }
                            }}
                                onClick={handleRotationClick}>
                                <ArrowArcRight />
                            </Box>
                        </Box>
                    </Box>
                    <Box sx={{ display: "flex", flexFlow: "row noWrap", gap: "4px", width: "100%" }}>
                        <SecondaryButtonRectangle onClick={handleClose} sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.COMMON.CANCEL)}</SecondaryButtonRectangle>
                        <PrimaryButtonRectangle onClick={handleSave} sx={{ width: "100%" }}>{t(TRANSLATION_KEYS.COMMON.SAVE)}</PrimaryButtonRectangle>
                    </Box>
                </ModalBoxSmallStyled>
            </ModalStyled>
        </ImageEditModalContext.Provider>
    );
}

export const useImageEditModalContextType = contextFactoryCreate('ImageEditModal', ImageEditModalContext);
export default ImageEditorModalProvider;
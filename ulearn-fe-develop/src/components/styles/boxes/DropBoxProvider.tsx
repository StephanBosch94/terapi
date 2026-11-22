import Box from "@mui/material/Box";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch } from "core/redux/reduxHooks";
import React, { useCallback, createContext, useState } from "react";
import { FileRejection, DropEvent, useDropzone } from "react-dropzone";
import { useSnackbar } from "notistack";

type DropBoxType = {
    isDragActive: boolean
} | undefined;
type FileFormatsType = ".jpg" | ".jpeg" | ".png" | ".heif" | ".heic" | ".pdf";
export const DropBoxContext = createContext<DropBoxType>(undefined);
export const useDropBoxContext = contextFactoryCreate('DropBox', DropBoxContext);

const DropBoxProvider = ({
    children,
    noClick = true,
    multiple = true,
    onFilesDrop = () => { },
    fileFormats = ['.jpg', '.jpeg', '.png', '.heif', '.heic']
}: {
    children: any,
    noClick?: boolean,
    multiple?: boolean,
    onFilesDrop?: (file: File[]) => void,
    fileFormats?: FileFormatsType[]
}) => {
    const { t } = useTranslation();
    const MAX_FILE_SIZE = 15; //MB

    const { enqueueSnackbar } = useSnackbar();

    const isValidFileExtension = (file: File) => {
        return fileFormats.some(x => file.name.includes(x));
    }

    const isValidFileSize = (file: File) => {
        return (file.size / 1024 / 1024) < MAX_FILE_SIZE;
    }

    const onDrop = useCallback((
        acceptedFiles: File[],
        fileRejections: FileRejection[],
        event: DropEvent
    ) => {
        if (!multiple && acceptedFiles.length > 1) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.INVALID_NUMBER_OF_FILES), { variant: "error" });
            return;
        }
        if (acceptedFiles.some(x => !isValidFileExtension(x))) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.INVALID_FILE_EXTENSION), { variant: "error" });
            return;
        }
        if (acceptedFiles.some(x => !isValidFileSize(x))) {
            enqueueSnackbar(t(TRANSLATION_KEYS.TUTOR_SIGN_UP.INVALID_FILE_SIZE), { variant: "error" });
            return;
        }
        onFilesDrop(acceptedFiles);
    }, [t]);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop, noClick, multiple });

    return (
        <DropBoxContext.Provider value={{
            isDragActive,

        }}>
            <Box {...getRootProps()}>
                <input {...getInputProps()} />
                {children}
            </Box>
        </DropBoxContext.Provider>
    )
}

export default DropBoxProvider;
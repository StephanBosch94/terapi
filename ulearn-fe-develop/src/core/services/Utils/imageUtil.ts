import { AppUserOverviewVM, UserWithFilesVM } from "../Types/AppTypes"
import { FileTypeEnum } from "core/constants/enums/FileTypeEnum";
import FileService from "core/services/FileService";

export const MemeTypes = {
  jpeg: "image/jpeg"
}

export const getFileUrl = (files: UserWithFilesVM['files'] | undefined, fileType: FileTypeEnum) => {
  const file = files?.find(x => x.fileTypeId === fileType);

  return file?.filePath ? FileService.getFileUrl(file?.filePath) : "";
}

export const getProfilePhotoUrl = (user: UserWithFilesVM | undefined) => {
  return getFileUrl(user?.files, FileTypeEnum.ProfilePhoto);
}

export const getProfilePhotoThumbnailUrl = (user: UserWithFilesVM | undefined) => {
  return getFileUrl(user?.files, FileTypeEnum.ProfilePhotoThumbnail);
}

export const getCertificateUrl = (user: UserWithFilesVM | undefined) => {
  return getFileUrl(user?.files, FileTypeEnum.Certificate);
}

export const readFileAsync = (fileData: any): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = () => {
      resolve(reader.result as string);
    }

    reader.onerror = reject;

    const fileDataBlob = new Blob([fileData]);
    reader.readAsDataURL(fileDataBlob);
  })
}

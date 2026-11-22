import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useTranslation } from "components/core/TranslationProvider";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ModalStyled } from "components/styles/ModalStyled";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { contextFactoryCreate, useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { createContext, useEffect, useRef, useState } from "react";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import { FilterEmployeesResponseBody, deleteEmployeeAccount, selectDeleteEmployeeAccountStatus, setEmployeesFilter } from "core/redux/userSlice";
import H5Styled from "components/styles/typography/H5Styled";
import Avatar from "@mui/material/Avatar";
import Body1Styled from "components/styles/typography/Body1Styled";
import { useSnackbar } from "notistack";
import { snackbarConstants } from "components/core/Snackbar";
import { setUserWalletStatistics } from "core/redux/userWalletSlice";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { getUserNameOrEmail } from "core/services/Utils/userUtils";
import ModalsHeader from "./common/ModalsHeader";

const PrimaryButtonActiveStyled = styled(PrimaryButtonRectangle)(() => sx({
    width: "100%",
    backgroundColor: COLORS.RED,
    ":hover": {
        backgroundColor: COLORS.RED_HOVER
    }
})) as typeof PrimaryButtonRectangle;

const AvatarStyledSmall = styled(Avatar)({
    width: "40px",
    height: "40px"
}) as typeof Avatar;

type DeleteStudentModalContextType = {
    setOpen: () => void
} | undefined;

const DeleteStudentEmployeeContext = createContext<DeleteStudentModalContextType>(undefined);

const DeleteEmployeeModalProvider = ({ children, user }: { children: any, user: FilterEmployeesResponseBody['data'][0] }) => {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const submitted = useRef(false);
    const { enqueueSnackbar } = useSnackbar();
    const deleteColleagueAcountStatus = useAppSelector(selectDeleteEmployeeAccountStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        document.addEventListener('keydown', handleKeydown);
        return () => {
            document.removeEventListener('keydown', handleKeydown);
        }
    });

    useEffect(() => {
        if (!submitted.current) return;
        if (deleteColleagueAcountStatus === LoadingStatusEnum.Loaded) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_MODAL.SUCCESS), { variant: "success" });
            submitted.current = false;
        }
        else if (deleteColleagueAcountStatus === LoadingStatusEnum.Error) {
            enqueueSnackbar(t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_MODAL.FAILED), { variant: "error", autoHideDuration: snackbarConstants.autohideErrorDuration });
            submitted.current = false;
        };
    }, [deleteColleagueAcountStatus]);

    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = () => {
        setOpen(true);
    }

    const handleClick = async () => {
        submitted.current = true;
        await dispatch(deleteEmployeeAccount({ userId: user.userId! }))
        await dispatch(setUserWalletStatistics())
        await dispatch(setEmployeesFilter({
            appendType: AppendTypeEnum.None,
            request: {
                skip: 0,
                take: 200
            }
        }))
        setOpen(false);
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (!open) return;
        if (e.key === 'Enter') {
            handleClick();
        }
    }

    return (
        <DeleteStudentEmployeeContext.Provider value={{
            setOpen: handleOpen
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_MODAL.TITLE)} handleClose={handleClose} />
                    <Box sx={{ display: "flex", justifyContent: "start", flexFlow: "row", gap: "12px", alignItems: "center" }}>
                        <AvatarStyledSmall src={getProfilePhotoThumbnailUrl(user)} />
                        <Box sx={{ display: "flex", justifyContent: "center", flexFlow: "column", gap: "4px" }}>
                            <H5Styled>{getUserNameOrEmail(user)}</H5Styled>
                            <Body1Styled dangerouslySetInnerHTML={{ __html: t(TRANSLATION_KEYS.COMPANY_DASHBOARD.AVAILABLE_BALANCE, { amount: `${user.wallet?.amount ?? 0} Kč` }) }} />
                        </Box>
                    </Box>
                    <Box sx={{ display: "flex", flexFlow: "column", gap: "8px" }}>
                        <H5Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_TITLE)}</H5Styled>
                        <Body1Styled>{t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_DESCRIPTION)}</Body1Styled>
                    </Box>
                    <PrimaryButtonActiveStyled onClick={handleClick}>
                        {deleteColleagueAcountStatus !== LoadingStatusEnum.Loading ? t(TRANSLATION_KEYS.COMPANY_DASHBOARD.DELETE_ACCOUNT_MODAL.BUTTON) : <CircularProgressCustom color={COLORS.WHITE} />}
                    </PrimaryButtonActiveStyled>
                </ModalBoxStyled>
            </ModalStyled>
        </DeleteStudentEmployeeContext.Provider >
    )
}

export const useDeleteStudentModalContext = contextFactoryCreate('DeleteEmployeeModal', DeleteStudentEmployeeContext);
export default DeleteEmployeeModalProvider;
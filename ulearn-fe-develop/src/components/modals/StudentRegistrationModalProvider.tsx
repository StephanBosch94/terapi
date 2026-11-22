import { ModalBoxSmallStyled } from "components/styles/boxes/ModalBoxSmallStyled";
import { contextFactoryCreate, useAppDispatch } from "core/redux/reduxHooks";
import { createContext, useState } from "react";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import StudentProfileSettings from "components/registration/StudentProfileSettings";
import { setMatchingTutors } from "core/redux/userRecommendationSlice";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { ModalStyled } from "components/styles/ModalStyled";
import ConfettiBox from "components/styles/boxes/ConfettiBox";

const ModalBoxStyled = styled(ModalBoxSmallStyled)({
    overflow: "auto",
}) as typeof Box;

type StudentRegistrationModalContextType = {
    setOpen: () => void,
    setClose: () => void
} | undefined;
const StudentRegistrationModalContext = createContext<StudentRegistrationModalContextType>(undefined);


const StudentRegistrationModalProvider = ({ children }: { children: JSX.Element }) => {
    const [open, setOpen] = useState(false);
    const [showConfetti, setShowConfetti] = useState(false);
    const dispatch = useAppDispatch();

    const handleOpen = () => {
        setOpen(true);
    }

    const handleClose = () => {
        setOpen(false);
    }

    const handleSubmitRegistration = () => {
        dispatch(setMatchingTutors({ take: COMMON_CONSTANTS.MAX_NUMBER_OF_MATCHING_TUTORS }));
        setOpen(false);
        setShowConfetti(true);
        setTimeout(() => {
            setShowConfetti(false);
        }, 6000);  
    }

    return (
        <StudentRegistrationModalContext.Provider value={{
            setOpen: handleOpen,
            setClose: handleSubmitRegistration
        }}>
            {children}
            <ModalStyled open={open}>
                <ModalBoxStyled>
                    <StudentProfileSettings pageType="registration" onFinalSubmit={handleSubmitRegistration} onClose={handleClose}/>
                </ModalBoxStyled>
            </ModalStyled>
            {showConfetti ? <ConfettiBox /> : <></>}
        </StudentRegistrationModalContext.Provider>
    )
}

export const withStudentRegistrationModalProvider = <P extends object>(Component: React.ComponentType<P>) => {
    const WithStudentRegistrationModalProvider = (props: any) => (
        <StudentRegistrationModalProvider>
            <Component {...props} />
        </StudentRegistrationModalProvider >
    );

    const displayName = Component.displayName || Component.name || 'Component';
    WithStudentRegistrationModalProvider.displayName = `withStudentRegistrationModalProvider(${displayName})`;

    return WithStudentRegistrationModalProvider;
};

export const useStudentRegistrationModal = contextFactoryCreate('StudentRegistrationModal', StudentRegistrationModalContext);
export default StudentRegistrationModalProvider;
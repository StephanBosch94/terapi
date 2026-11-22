import { contextFactoryCreate, useAppDispatch } from "core/redux/reduxHooks";
import { createContext, useEffect, useState } from "react";
import Box from "@mui/material/Box";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { ModalStyled } from "components/styles/ModalStyled";
import ConfettiBox  from "components/styles/boxes/ConfettiBox";
import CompanyProfileSettings from "components/registration/CompanyProfileSettings";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";

const ModalBoxStyledOverflow = styled(ModalBoxStyled)({
    overflow: "auto",
}) as typeof Box;

type CompanyRegistrationModalContextType = {
    setOpen: () => void,
    setClose: () => void
} | undefined;
const CompanyRegistrationModalContext = createContext<CompanyRegistrationModalContextType>(undefined);


const CompanyRegistrationModalProvider = ({ children }: { children: JSX.Element }) => {
    const [open, setOpen] = useState(false);
    const [showConfetti, setShowConfetti] = useState(false);

    const handleOpen = () => {
        setOpen(true);
    }

    const handleSubmitRegistration = () => {
        setOpen(false);
        setShowConfetti(true);
        setTimeout(() => {
            setShowConfetti(false);
        }, 6000);  
    }

    return (
        <CompanyRegistrationModalContext.Provider value={{
            setOpen: handleOpen,
            setClose: handleSubmitRegistration
        }}>
            {children}
            <ModalStyled open={open}>
                <ModalBoxStyledOverflow>
                    <CompanyProfileSettings pageType="registration" onFinalSubmit={handleSubmitRegistration} />
                </ModalBoxStyledOverflow>
            </ModalStyled>
            {showConfetti ? <ConfettiBox /> : <></>}
        </CompanyRegistrationModalContext.Provider>
    )
}

export const withCompanyRegistrationModalProvider = <P extends object>(Component: React.ComponentType<P>) => {
    const WithCompanyRegistrationModalProvider = (props: any) => (
        <CompanyRegistrationModalProvider>
            <Component {...props} />
        </CompanyRegistrationModalProvider >
    );

    const displayName = Component.displayName || Component.name || 'Component';
    WithCompanyRegistrationModalProvider.displayName = `withCompanyRegistrationModalProvider(${displayName})`;

    return WithCompanyRegistrationModalProvider;
};

export const useCompanyRegistrationModal = contextFactoryCreate('CompanyRegistrationModal', CompanyRegistrationModalContext);
export default CompanyRegistrationModalProvider;
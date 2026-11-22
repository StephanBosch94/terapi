import { contextFactoryCreate } from "core/redux/reduxHooks";
import { createContext, useContext } from "react";
import React from "react";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import { ModalStyled } from "components/styles/ModalStyled";
import Authentication from "components/auth/Authentication";
import routes from "routes/routes";
import LargeModalBoxStyled from "components/styles/boxes/LargeModalBoxStyled";
import { UIContext } from "components/core/UIContextProvider";

type PageType = "login" | "registration";
type AuthModalContextType = {
    setOpen: (role: UserRoleEnum, type: PageType) => void,
    setClose: () => void
} | undefined;

const AuthModalContext = createContext<AuthModalContextType>(undefined);

const AuthModalProvider = ({ children }: { children: any }) => {
    const [pageType, setPageType] = React.useState<PageType>("login");
    const [userRole, setUserRole] = React.useState<UserRoleEnum>(UserRoleEnum.Student);
    const [open, setOpen] = React.useState(false);
    const { screenType } = useContext(UIContext);
    const handleClose = () => {
        setOpen(false);
    }

    const handleOpen = (role: UserRoleEnum, type: PageType) => {
        setUserRole(role);
        setPageType(type);
        setOpen(true);
    }

    const handleAlternative = () => {
        setPageType(prev => prev === "login" ? "registration" : "login");
    }

    const handleSuccessExternal = () => {
        setOpen(false);
        location.replace(routes.home.path);
    }

    const handleSuccessInternal = () => {
        location.replace(routes.confirmEmail.path);
    }

    return (
        <AuthModalContext.Provider value={{
            setOpen: handleOpen,
            setClose: () => setOpen(false)
        }}>
            {children}
            <ModalStyled open={open} onClose={handleClose}>
                <LargeModalBoxStyled>
                    <Authentication
                        pageType={pageType}
                        userRole={userRole}
                        onAlternativeClick={handleAlternative}
                        onClose={handleClose}
                        hideImage={screenType === "mobile"}
                        onSuccessExternal={handleSuccessExternal}
                        onSucessInternal={handleSuccessInternal}
                    />
                </LargeModalBoxStyled>
            </ModalStyled>
        </AuthModalContext.Provider>
    );
};

export const useAuthModalContext = contextFactoryCreate('AuthModal', AuthModalContext);
export default AuthModalProvider;
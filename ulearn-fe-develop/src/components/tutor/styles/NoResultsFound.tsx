import React from "react";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import AuthModalProvider, { useAuthModalContext } from "components/modals/AuthModalProvider";
import Empty from "components/core/empty/Empty";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";

const EMPTY_CONTENT = {
    titleKey: TRANSLATION_KEYS.POPULAR_TUTORS.NO_RESULTS.TITLE,
    descriptionKey: TRANSLATION_KEYS.POPULAR_TUTORS.NO_RESULTS.DESCRIPTION,
    buttonTextKey: TRANSLATION_KEYS.POPULAR_TUTORS.NO_RESULTS.SIGN_UP_BUTTON,
    imagePath: "/images/not_found.png"
}

const NoResultsFound = () => {
    const { setOpen } = useAuthModalContext();

    return (
        <Empty content={EMPTY_CONTENT} onClick={() => setOpen(UserRoleEnum.Tutor, "registration")} />
    )
}

const NoResultsFoundWithContext = () => {

    return (
        <AuthModalProvider>
            <NoResultsFound />
        </AuthModalProvider>
    )
}

export default NoResultsFoundWithContext;
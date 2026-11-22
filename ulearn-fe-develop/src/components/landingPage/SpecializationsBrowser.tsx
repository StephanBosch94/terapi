import React from "react";
import routes from "routes/routes";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { IconCategoryType } from "components/core/icons/InjectIcon";
import Browser, { BrowserPropsType } from "./Browser";

const SpecializationsBrowser = (props: { subjectId: number, excludeSpecializationId?: number | undefined } & Omit<BrowserPropsType, "data">) => {
    const { specializations } = useAppDataContext();
    const filteredSpecializations = specializations
        ?.filter(x => x.subjectId === props.subjectId)
        ?.filter(x => x.specializationId !== props.excludeSpecializationId);
    const data = filteredSpecializations?.map(x => ({
        id: x.specializationId!,
        enumKey: x.enumKey ?? "",
        nameKey: x.nameKey ?? "",
        href: routes.specializationLandingPage.path(x.enumKey ?? ""),
        iconName: x.iconName ?? "",
        iconCategory: "specializations" as IconCategoryType,
    }));

    return (
        <Browser {...props} data={data} itemTextSize="small" />
    )
}

export default SpecializationsBrowser;
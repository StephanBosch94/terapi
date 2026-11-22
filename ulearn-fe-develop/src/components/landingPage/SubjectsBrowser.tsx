import React from "react";
import routes from "routes/routes";
import { CodetableSubjectsResponse } from "core/redux/codetableSlice";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { IconCategoryType } from "components/core/icons/InjectIcon";
import Browser, { BrowserPropsType } from "./Browser";

const getFilteredSubjects = (subjects: CodetableSubjectsResponse | undefined, excludeSubjectKey?: string | null) => {
    if (!excludeSubjectKey) return subjects;
    return subjects?.filter(x => x.enumKey !== excludeSubjectKey);
}

const SubjectsBrowser = (props: { excludeSubjectKey?: string | null } & Omit<BrowserPropsType, "data">) => {
    const { subjects } = useAppDataContext();
    const filteredSubjects = getFilteredSubjects(subjects, props.excludeSubjectKey);
    const data = filteredSubjects?.map(x => ({
        id: x.subjectId!,
        enumKey: x.enumKey ?? "",
        nameKey: x.nameKey ?? "",
        href: routes.subjectLandingPage.path(x.enumKey ?? ""),
        iconName: x.iconName ?? "",
        iconCategory: "emoji" as IconCategoryType,
    }));

    return (
        <Browser {...props} data={data} />
    )
}

export default SubjectsBrowser;
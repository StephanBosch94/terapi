export enum UserRoleEnum {
    Admin = 1,
    Tutor = 2,
    Student = 3,
    Company = 4
}

export const toUserRoleEnum = (roleEnumString: string): UserRoleEnum | undefined => {
    switch (roleEnumString?.toLocaleLowerCase()) {
        case "admin":
            return UserRoleEnum.Admin;
        case "tutor":
            return UserRoleEnum.Tutor;
        case "student":
            return UserRoleEnum.Student;
        case "company":
            return UserRoleEnum.Company;
        default:
            return undefined;
    }
}
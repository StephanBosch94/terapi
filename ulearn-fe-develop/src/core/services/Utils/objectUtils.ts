export const removeUndefinedProperties = <T extends {},>(props: T): T =>
    Object.fromEntries(
        Object.entries(props).filter(([, value]) => value !== undefined),
    ) as T;
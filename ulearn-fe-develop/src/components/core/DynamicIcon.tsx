import { useRef, useState, useEffect, ComponentType } from "react";

type OnCompletedCallbackType = (iconName: string, icon: ComponentType<any> | undefined) => void;
type OnErrorCallbackType = (error: Error) => void;

type OptionsType = {
    onCompleted?: OnCompletedCallbackType;
    onError?: OnErrorCallbackType;
};

export const useDynamicSVGImport = (iconName: string, options: OptionsType = {}) => {
    const ImportedIconRef = useRef<ComponentType<any> | undefined>();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | undefined>();

    const { onCompleted, onError } = options;
    useEffect(() => {
        setLoading(true);
        const importIcon = async () => {
            try {
                ImportedIconRef.current = (
                    await import(`assets/${iconName}.svg`)
                ).default;
                if (onCompleted) {
                    onCompleted(iconName, ImportedIconRef.current);
                }
            } catch (err) {
                if (onError) {
                    onError(err as Error);
                }
                setError(err as Error);
            } finally {
                setLoading(false);
            }
        };
        importIcon();
    }, [iconName, onCompleted, onError]);

    return { error, loading, SvgIcon: ImportedIconRef.current };
}

const DynamicIcon = ({ name, onCompleted, onError }: { name: string, onCompleted: OnCompletedCallbackType, onError: OnErrorCallbackType }) => {
    const { error, loading, SvgIcon } = useDynamicSVGImport(name, {
        onCompleted,
        onError
    });
    if (error) {
        return error.message;
    }
    if (loading) {
        return "Loading...";
    }
    if (SvgIcon) {
        return <SvgIcon />;
    }

    return null;
};

export default DynamicIcon;
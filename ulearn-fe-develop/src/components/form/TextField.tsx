import { useTranslation } from "components/core/TranslationProvider";
import { TextFieldStyled } from "components/styles/TextFieldStyled";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { useState, useEffect } from "react";

const TextField = ({ placeholderKey, defaultValue, type = "text", name, hasError = false, fullwidth = true }: { placeholderKey: string, defaultValue: string, type?: React.InputHTMLAttributes<unknown>['type'], name: string, hasError?: boolean, fullwidth?: boolean }) => {
    const { t } = useTranslation();
    const [value, setValue] = useState('');

    useEffect(() => {
        setValue(defaultValue);
    }, [defaultValue]);

    const handleSetValue = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setValue(e.target.value);
    }

    return (
        <TextFieldStyled
            fullWidth={fullwidth}
            placeholder={t(placeholderKey)}
            name={name}
            type={type}
            value={value}
            onChange={handleSetValue}
            sx={{
                border: hasError && !value.length ? `1px solid ${(COLORS.RED)}` : `1px solid ${(COLORS.BLACK_10)}`
            }}
        />
    )
}

export default TextField;
import { useTranslation } from "components/core/TranslationProvider";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import React from "react";
import { PrimaryButton } from "components/styles/PrimaryButton";
import H4Styled from "components/styles/typography/H4Styled";
import PStyled from "components/styles/typography/PStyled";
import { EmptyType } from "core/services/Types/AppTypes";
import Image from 'next/image';
import Box from "@mui/material/Box";

const EmptyBoxStyled = styled(Box)(() => sx({
    width: "100%",
    textAlign: "center",
    display: "flex",
    gap: "24px",
    alignItems: "center",
    justifyContent: "center",
    flexFlow: "column noWrap",
    padding: "40px 0px"
})) as typeof Box;

const TextBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "4px",
    alignItems: "center",
    justifyContent: "center",
    flexFlow: "column noWrap"
})) as typeof Box;

const Empty = ({ content, onClick}: { content: EmptyType, onClick?: () => void}) => {
    const { t } = useTranslation();

    return (
        <EmptyBoxStyled>
            <Image src={content.imagePath} height={64} width={120} alt="" />
            <TextBoxStyled>
                <H4Styled>{t(content.titleKey)}</H4Styled>
                <PStyled>{t(content.descriptionKey)}</PStyled>
            </TextBoxStyled>
            {onClick ? <Box><PrimaryButton onClick={onClick}>{t(content.buttonTextKey)}</PrimaryButton></Box> : <></>}
        </EmptyBoxStyled>
    )
}

export default Empty;
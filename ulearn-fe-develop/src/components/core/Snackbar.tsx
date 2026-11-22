import { CustomContentProps, SnackbarProvider } from 'notistack';
import { styled } from "@mui/material/styles";
import { COLORS } from 'components/styles/constants/ColorsConstatnts';
import CheckIcon from 'assets/check-circle-fill.svg';
import XIcon from 'assets/x-circle-fill.svg';
import H5Styled from "components/styles/typography/H5Styled";
import React from 'react';
import Box from '@mui/material/Box';

const MainBoxStyled = styled(Box)({
    minWidth: "400px",
    display: "inline-flex",
    alignItems: "center",
    justifyContent: "flex-start",
    padding: "16px",
    gap: "12px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    boxShadow: "4px 4px 4px rgba(0, 0, 0, 0.1)",
    backgroundColor: COLORS.WHITE,
    opacity: 1,
    zIndex: 100
}) as typeof Box;

const InnerBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    gap: "8px",
    alignItems: "center"
}) as typeof Box;

const CheckIconBoxStyled = styled(Box)({
    display: "flex",
    alignItems: "center",
    color: COLORS.GREEN,
    "svg": {
        transform: "scale(1.25)"
    }
}) as typeof Box;

const XIconBoxStyled = styled(Box)({
    color: COLORS.RED,
    "svg": {
        transform: "scale(1.25)"
    }
}) as typeof Box;

export const snackbarConstants = {
    autohideErrorDuration: 5000
}

const SnackbarError = React.forwardRef<React.RefAttributes<HTMLDivElement>, CustomContentProps>((props, ref) => {
    return (
        <MainBoxStyled ref={ref} style={props.style}>
            <XIconBoxStyled>
                <XIcon />
            </XIconBoxStyled>

            <InnerBoxStyled>
                <H5Styled>{props.message}</H5Styled>
            </InnerBoxStyled>
        </MainBoxStyled>
    )
})
SnackbarError.displayName = "Snackbar Error";

const SnackbarSuccess = React.forwardRef<React.RefAttributes<HTMLDivElement>, CustomContentProps>((props, ref) => {

    return (
        <MainBoxStyled ref={ref} style={props.style}>
            <CheckIconBoxStyled>
                <CheckIcon />
            </CheckIconBoxStyled>

            <InnerBoxStyled>
                <H5Styled>{props.message}</H5Styled>
            </InnerBoxStyled>
        </MainBoxStyled>
    )
})
SnackbarSuccess.displayName = "SnackbarSuccess";

const Snackbar = ({ children }: { children: any }) => {

    return (
        <SnackbarProvider
            Components={{
                success: SnackbarSuccess,
                error: SnackbarError
            }}
            anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'right',
            }}
            autoHideDuration={3000}
        >
            {children}
        </SnackbarProvider>
    )
}
export default Snackbar;
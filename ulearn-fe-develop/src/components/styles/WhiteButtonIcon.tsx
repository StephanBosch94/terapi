import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import React from "react";
import { COLORS } from "./constants/ColorsConstatnts";

export const WhiteIconButtonStyled = styled(Button)(() => sx({
  display: "flex",
  padding: "6px 12px",
  color: COLORS.BLACK,
  gap: "8px",
  backgroundColor: COLORS.WHITE,
  border: `1px solid ${COLORS.BLACK_10}`,
  borderRadius: "4px",
  fontWeight: "500",
  marginRight: "8px",
  width: { xs: "100%", md: "auto" },
  textDecoration: "none",
  fontSize: "1rem",
  alignItems: "center",
  ":hover": {
    backgroundColor: COLORS.PRIMARY_5,
    cursor: "pointer"
  }
})) as typeof Button;

export const WhiteIconButton = ({ text, icon }: { text: string, icon: JSX.Element }) => {
  return (
    <WhiteIconButtonStyled>{icon}{text}</WhiteIconButtonStyled>
  )
}

import Button from "@mui/material/Button";
import { experimental_sx as sx, styled } from "@mui/system";
import React from "react";
import { COLORS } from "./constants/ColorsConstatnts";

export const TextButtonStyled = styled(Button)(() => sx({
  color: COLORS.PRIMARY,
  padding: "10px 0px",
  backgroundColor: COLORS.WHITE,
  fontSize: "0.875rem",
  ':hover': {
    textDecoration: "underline",
    backgroundColor: COLORS.WHITE,
  }
})) as typeof Button;

export const TextButton = ({ text }: { text: string }) => {
  return (
    <TextButtonStyled variant="text">{text}</TextButtonStyled >
  )
}


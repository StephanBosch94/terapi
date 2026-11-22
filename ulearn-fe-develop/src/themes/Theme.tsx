import { createTheme } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";

export const appTheme = createTheme({
  palette: {
    primary: {
      main: "rgba(94,76,198,1)",
      light: "rgba(94,76,198,0.04)",
      dark: "rgba(94,76,198,0.1)",
      contrastText: COLORS.WHITE,
    },
    secondary: {
      main: "rgba(17,203,95,1)",
      light: "rgba(17,203,95,0.04)",
      dark: "rgba(17,203,95,0.1)",
    },
    customGray: {
      main: "#7C7F82"
    },
    customWhite: {
      main: "#F5F5F5"
    },
    forms: {
      main: "#FAFBFC"
    }
  },

  components: {
    MuiButton: {
      styleOverrides: {
        outlined: {
          textTransform: "none",
        },
        text: {
          textTransform: "none",
        },
      },
    },
  },

  typography: {
    pSm: {
      fontSize: 14,
    },
    fontFamily: 'inherit',
  }
});

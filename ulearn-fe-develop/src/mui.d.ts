import "@mui/material/styles/createPalette";
import { PaletteColor, SimplePaletteColorOptions } from "@mui/material/styles/createPalette";
import React from "react";
declare module "@mui/material/styles/createPalette" {
    interface Palette {
        customGray: SimplePaletteColorOptions;
        customWhite: SimplePaletteColorOptions;
        forms: SimplePaletteColorOptions;
    }
    interface PaletteOptions {
        customGray: SimplePaletteColorOptions;
        customWhite: SimplePaletteColorOptions;
        forms: SimplePaletteColorOptions;
    }
}

declare module '@mui/material/styles' {
    interface TypographyVariants {
        pSm: React.CSSProperties;
        fontFamily: string;
    }

    // allow configuration using `createTheme`
    interface TypographyVariantsOptions {
        pSm?: React.CSSProperties;
        fontFamily?: string;
    }
}

// Update the Typography's variant prop options
declare module '@mui/material/Typography' {
    interface TypographyPropsVariantOverrides {
        pSm: true;
        fontFamily: true;
    }
}


declare module 'notistack' {
    interface VariantOverrides {

    }
}
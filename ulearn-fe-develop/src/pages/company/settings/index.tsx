import { styled, experimental_sx as sx } from "@mui/material/styles";
import Box from "@mui/material/Box";
import React from "react";
import Navbar from "components/navbar/Navbar";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import CompanyProfileSettings from "components/registration/CompanyProfileSettings";

const CompanyViewBoxStyled = styled(Box)(() => sx({
    height: "100%",
    width: "100%",
    alignItems: "center",
    display: "flex",
    flexFlow: "column noWrap",
    overflow: "auto"
})) as typeof Box;

const CompanyProfilePage = () => {

    return (
        <CompanyViewBoxStyled>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "row noWrap", flex: "0 1 auto" }}>
                <Navbar />
            </Box>
            <CompanyProfileSettings pageType="settings" />
        </CompanyViewBoxStyled>
    )
}

export default withAppClientSideProvider(CompanyProfilePage);
import React from 'react';
import { Box, Typography } from '@mui/material';
import CircularProgressCustom from '../styles/CircularProgressStyled';
import { COLORS } from '../styles/constants/ColorsConstatnts';

const InitialLoadingScreen = () => {
    return (
        <Box
            sx={{
                position: 'fixed',
                top: 0,
                left: 0,
                right: 0,
                bottom: 0,
                backgroundColor: COLORS.WHITE,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                zIndex: 9999,
            }}
        >
            <CircularProgressCustom size={50} color={COLORS.PRIMARY} />
            <Typography
                variant="body1"
                sx={{
                    marginTop: 3,
                    color: COLORS.BLACK,
                    textAlign: 'center',
                    maxWidth: '400px',
                    padding: '0 20px',
                }}
            >
                Spinning up VM may take up to 1 minute. Please wait.
            </Typography>
        </Box>
    );
};

export default InitialLoadingScreen;


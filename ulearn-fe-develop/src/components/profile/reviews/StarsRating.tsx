import StarEmpty from "assets/star.svg";
import StarFilled from "assets/star-filled.svg";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { styled } from "@mui/material/styles";
import Box from "@mui/material/Box";
import React, { useEffect, useRef, useState } from "react";

const StarBoxStyled = styled(Box)({
    display: "inline-flex",
    gap: "8px",
    color: COLORS.PRIMARY,
    alignItems: "center"
}) as typeof Box;

const StarsRating = ({ rating, onClick }: { rating: number, onClick?: (newRating: number) => void }) => {
    const [hoverRating, setHoverRating] = useState<number | undefined>(rating);
    const [clickedRating, setClickedRating] = useState<number>(0);
    const acc = useRef<NodeJS.Timeout>();

    useEffect(() => {
        setHoverRating(rating);
        setClickedRating(rating);
    }, [rating]);
    
    const handleDebounceHover = (newRating: number) => {
        if (acc.current){
            clearTimeout(acc.current);
        }
        acc.current = setTimeout(() => {
            setHoverRating(newRating);
        }, 70);
    }

    const handleMouseEnter = (newRating: number) => {
        if (!onClick) return;
        handleDebounceHover(newRating);
    }

    const handleMouseLeave = () => {
        if (!onClick) return;
        handleDebounceHover(clickedRating);
    }

    const handleClick = (newRating: number) => {
        if (!onClick) return;
        setHoverRating(newRating);
        setClickedRating(newRating);
        if (onClick) {
            onClick(newRating);
        }
    }

    return (
        <StarBoxStyled>
            {
                Array(5).fill(1).map((x, idx) => (
                    <Box
                        key={idx}
                        onMouseEnter={() => handleMouseEnter(idx + 1)}
                        onMouseLeave={handleMouseLeave}
                        onClick={() => handleClick(idx + 1)}
                        sx={{ ":hover": { cursor: onClick ? "pointer" : "regular" } }}
                    >
                        {hoverRating && (idx + 1 <= hoverRating) ? <StarFilled /> : <StarEmpty />}
                    </Box>
                ))
            }
        </StarBoxStyled>
    )
}

export default StarsRating;
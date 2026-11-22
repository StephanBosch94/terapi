import { ModalStyled } from "components/styles/ModalStyled";
import { ModalBoxStyled } from "components/styles/boxes/ModalBoxStyled";
import { useEffect, useState } from "react";
import ModalsHeader from "./common/ModalsHeader";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import Box from "@mui/material/Box";
import PStyled from "components/styles/typography/PStyled";
import { useTranslation } from "components/core/TranslationProvider";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectDefaultCouponInfo, setDefaultCouponInfo } from "core/redux/reservationSlice";
import { getStorage } from "core/services/Core/StorageService";
import { cacheConstants } from "core/constants/cacheConstants";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { PrimaryButtonRectangle } from "components/styles/PrimaryButtonRectangle";
import H2Styled from "components/styles/typography/H2Styled";
import H5Styled from "components/styles/typography/H5Styled";

const ModalCenterBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    gap: "24px"
})) as typeof Box;

const CopyBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    justifyContent: "space-between",
    alignItems: "center",
    flexFlow: "row noWrap",
    gap: "12px",
    backgroundColor: COLORS.PRIMARY_10,
    borderRadius: "4px",
    border: `1px dashed ${COLORS.PRIMARY}`,
    padding: { xs: "6px", md: "8px" },
    margin: "0 auto"
})) as typeof Box;

const DeviderBoxStyled = styled(Box)(() => sx({
    display: "flex",
    height: "1px",
    width: "100%",
    background: COLORS.BLACK_10
})) as typeof Box;

const DetailsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    gap: "8px"
})) as typeof Box;

const CodeTypographyStyled = styled(H2Styled)(() => sx({
    color: COLORS.BLACK,
    letterSpacing: { xs: "2px", md: "6px" },
})) as typeof Box;

const LessonDiscountModal = () => {
    const [open, setOpen] = useState(false);
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const defaultCoupon = useAppSelector(selectDefaultCouponInfo);

    useEffect(() => {
        let timer: NodeJS.Timeout | undefined = undefined;
        handleOpen().then((res) => timer = res);

        return () => {
            if (timer) {
                clearTimeout(timer);
            }
        }
    }, [dispatch]);

    const hasAlreadyClosed = async () => {
        const storage = await getStorage();
        const value = await storage.getCommonValue<string>(cacheConstants.lessonDiscountClosed);
        return value === "true";
    }

    const handleOpen = async () => {
        const alreadyClosed = await hasAlreadyClosed();
        if (alreadyClosed) {
            return undefined;
        }
        return setTimeout(() => {
            dispatch(setDefaultCouponInfo());
            setOpen(true);
        }, 15 * 1000);
    }

    const handleClose = async () => {
        setOpen(false);
        const storage = await getStorage();
        await storage.setCommonValue(cacheConstants.lessonDiscountClosed, String(true));
    };

    const handleCopyToClipboard = () => {
        if (!defaultCoupon?.code) return;
        navigator.clipboard.writeText(defaultCoupon.code);
    }

    return defaultCoupon && defaultCoupon.readyToUse
        ? (
            <ModalStyled open={open} onClose={handleClose}>
                <ModalBoxStyled>
                    <ModalsHeader text={t(TRANSLATION_KEYS.LESSON_DISCOUNT_POPUP.TITLE, { discountPercentage: defaultCoupon.discountPercentage ?? 0 })} handleClose={handleClose} />
                    <ModalCenterBoxStyled>
                        <PStyled>{t(TRANSLATION_KEYS.LESSON_DISCOUNT_POPUP.ACTION_TEXT)}</PStyled>
                        <CopyBoxStyled>
                            <CodeTypographyStyled>{defaultCoupon.code ?? ""}</CodeTypographyStyled>
                            <PrimaryButtonRectangle onClick={handleCopyToClipboard}>{t(TRANSLATION_KEYS.LESSON_DISCOUNT_POPUP.COPY_BUTTON)}</PrimaryButtonRectangle>
                        </CopyBoxStyled>
                        <DeviderBoxStyled />
                        <DetailsBoxStyled>
                            <H5Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.LESSON_DISCOUNT_POPUP.OFFER_DETAILS_TITLE)}</H5Styled>
                            <PStyled>{t(TRANSLATION_KEYS.LESSON_DISCOUNT_POPUP.OFFER_DETAILS_DESCRIPTION, { discountPercentange: defaultCoupon.discountPercentage ?? 0 })}</PStyled>
                        </DetailsBoxStyled>
                    </ModalCenterBoxStyled>
                </ModalBoxStyled>
            </ModalStyled>
        ) : null
}

export default LessonDiscountModal;
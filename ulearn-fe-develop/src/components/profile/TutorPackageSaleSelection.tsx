import { styled } from "@mui/material/styles";
import RadioButton from "components/styles/RadioButton";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { selectNumberOfLessons, selectDuration, setNumberOfLessons } from "core/redux/availabilityCalendarSlice";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { NumberOfLessons } from "core/services/Types/AppTypes";
import { getDurationInMinutes, getPriceWithSale } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { usePriceLocalization } from "core/hooks/priceHook";

const RightPanelPackageBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    alignItems: "flex-start",
    width: "100%",
    padding: "8px",
    gap: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY}`,
    }
}) as typeof Box;

type PackageType = { numberOfLessons: NumberOfLessons, translationKey: string, saleTranslationKey: string | undefined, sale: number };
type PackagePropsType = { hourRate: number | null | undefined, currencyCode: string | null | undefined };
const packages: PackageType[] = [
    {
        numberOfLessons: 1,
        translationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.ONE_LESSON_WITH_DURATION,
        saleTranslationKey: undefined,
        sale: COMMON_CONSTANTS.PACKAGES.ONE_LESSON_SALE
    },
    {
        numberOfLessons: 5,
        translationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.FIVE_LESSONS_WITH_DURATION,
        saleTranslationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.FIVE_LESSONS_SALE,
        sale: COMMON_CONSTANTS.PACKAGES.FIVE_LESSONS_SALE
    },
    {
        numberOfLessons: 10,
        translationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TEN_LESSONS_WITH_DURATION,
        saleTranslationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TEN_LESSONS_SALE,
        sale: COMMON_CONSTANTS.PACKAGES.TEN_LESSONS_SALE
    }
]

const LessonSale = ({ translationKey, amount, currencyCode }: { translationKey: string, amount: number, currencyCode: string | null | undefined }) => {
    const { localize } = usePriceLocalization();

    return (
        <Box sx={{ display: "inline-flex", backgroundColor: COLORS.PRIMARY_10, padding: "1px 6px", borderRadius: "16px" }}>
            <Typography sx={{ color: COLORS.PRIMARY, fontSize: "0.75rem", fontWeight: "500", lineHeight: "16px" }}>{localize({ price: -amount, translationKey })}</Typography>
        </Box>
    )
}

const TutorPackageSaleSelectionItem = ({ tutorInfo, packageInfo }: { tutorInfo: PackagePropsType, packageInfo: PackageType }) => {
    const { t } = useTranslation();
    const { localize } = usePriceLocalization();
    const dispatch = useAppDispatch();
    const numberOfLessons = useAppSelector(selectNumberOfLessons);
    const duration = useAppSelector(selectDuration);
    const handleNumberOfLessonsChange = () => {
        dispatch(setNumberOfLessons(packageInfo.numberOfLessons));
    }
    const tutorPackageLessonPrice = getPriceWithSale(tutorInfo.hourRate ?? 0, packageInfo.sale, duration);
    const tutorTotalPackagePriceBeforeDiscount = getPriceWithSale((tutorInfo.hourRate ?? 0) * packageInfo.numberOfLessons, 0, duration);
    const tutorTotalPackagePriceAfterDiscount = getPriceWithSale((tutorInfo.hourRate ?? 0) * packageInfo.numberOfLessons, packageInfo.sale, duration);
    const discountAmount = tutorTotalPackagePriceBeforeDiscount - tutorTotalPackagePriceAfterDiscount;

    return (
        <RightPanelPackageBoxStyled sx={{ gap: "8px" }} onClick={handleNumberOfLessonsChange}>
            <Box sx={{ padding: "2px 0" }}>
                <RadioButton checked={numberOfLessons === packageInfo.numberOfLessons} />
            </Box>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", width: "100%", gap: "4px" }}>
                <Box sx={{ display: "inline-flex", gap: "4px" }}>
                    <Box>
                        <Typography sx={{ fontWeight: "700", lineHeight: "18px", fontSize: "0.875rem" }}>{localize({ price: tutorPackageLessonPrice, duration })}</Typography>
                    </Box>
                    {packageInfo.saleTranslationKey ? <LessonSale translationKey={packageInfo.saleTranslationKey} amount={discountAmount} currencyCode={tutorInfo.currencyCode} /> : <></>}
                </Box>
                <Box sx={{ display: "inline-flex" }}>
                    <Typography sx={{ color: COLORS.BLACK, fontSize: "0.75rem", lineHeight: "16px", fontWeight: "400" }}>{t(packageInfo.translationKey, { duration: getDurationInMinutes(duration) })}</Typography>
                </Box>
            </Box>
        </RightPanelPackageBoxStyled>
    )
}

const TutorPackageSaleSelection = ({ tutorInfo }: { tutorInfo: PackagePropsType }) => {

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
            {packages.map((x, idx) => (
                <TutorPackageSaleSelectionItem key={idx} tutorInfo={tutorInfo} packageInfo={x} />
            ))}
        </Box>
    )
}

export default TutorPackageSaleSelection;
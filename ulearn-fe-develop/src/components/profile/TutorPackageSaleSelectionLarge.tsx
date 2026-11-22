import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { NumberOfLessonsType, selectDuration, selectNumberOfLessons, setNumberOfLessons } from "core/redux/availabilityCalendarSlice";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { useTranslation } from "components/core/TranslationProvider";
import { getDurationInMinutes, getPriceWithSale } from "core/services/Utils/userUtils";
import Box from "@mui/material/Box";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import H3Styled from "components/styles/typography/H3Styled";
import H4Styled from "components/styles/typography/H4Styled";
import RadioButton from "components/styles/RadioButton";
import { nameof } from "core/services/Types/AppTypes";
import PackagesBoxStyled from "components/styles/boxes/LargePackagesBoxStyled";
import GoldMedal from "assets/emoji/gold_medal.svg";
import SilverMedal from "assets/emoji/silver_medal.svg";
import BronzeMedal from "assets/emoji/bronze_medal.svg";
import { usePriceLocalization } from "core/hooks/priceHook";
const PACKAGE_HEADER_HEIGHT = '30px';

type PackageBoxType = {
    isMostPopular: boolean
}

const PackageBoxStyled = styled(Box, {
    shouldForwardProp: (prop) => prop !== nameof<PackageBoxType>('isMostPopular')
})<PackageBoxType>(({ isMostPopular }) => ({
    border: `1px solid ${COLORS.BLACK_10}`,
    backgroundColor: COLORS.WHITE,
    borderRadius: "8px",
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    height: !isMostPopular ? `calc(100% - ${PACKAGE_HEADER_HEIGHT})` : "100%",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5
    }
}));

const PackageInnerBox = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    padding: "0px",
    gap: "16px"
}) as typeof Box;

const PriceBoxWithSaleStyled = styled(Box)({
    display: "inline-flex",
    gap: "8px",
    alignItems: "center"
}) as typeof Box;

const PackageHeaderTypography = styled(H4Styled)({
    color: COLORS.BLACK,
    fontWeight: "600"
}) as typeof H4Styled;

const PackagePriceTypography = styled(H3Styled)({
    color: COLORS.PRIMARY,
    lineHeight: "px",
    fontWeight: "600"
}) as typeof H3Styled;

const LessonSaleBoxStyled = styled(Box)({
    display: "inline-flex",
    backgroundColor: COLORS.PRIMARY_5,
    padding: "4px 12px",
    borderRadius: "18px",
    border: `1px solid ${COLORS.PRIMARY}`
}) as typeof Box;

const LessonSaleTypographyStyled = styled(CaptionTextStyled)({
    color: COLORS.PRIMARY,
}) as typeof CaptionTextStyled;

const MostPopularEmptyBox = styled(Box)(() => ({
    borderRadius: "4px 4px 0px 0px",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    color: COLORS.WHITE,
    fontSize: "0.875rem",
    fontWeight: 500,
    textTransform: "uppercase",
    padding: "8px 0px",
    height: PACKAGE_HEADER_HEIGHT
})) as typeof Box;

const MostPopularBox = styled(MostPopularEmptyBox)(() => ({
    backgroundColor: COLORS.PRIMARY
})) as typeof Box;

const PackageContentBoxStyled = styled(Box)(() => ({
    display: "flex",
    height: "100%",
    flexFlow: "column noWrap",
    width: "100%",
    gap: "24px",
    justifyContent: "space-between",
    padding: "32px",
})) as typeof Box;

const PriceTotalBoxStyled = styled(Box)(() => ({
    display: "flex",
    flexFlow: "row noWrap",
    gap: "4px",
})) as typeof Box;

type PackageType = {
    numberOfLessons: NumberOfLessonsType,
    headerKey: string,
    descriptionKey: string,
    sale: number,
    saleTranslationKey: string | undefined,
    mostPopular: boolean,
    borderColor: string,
    Icon: string
}
type PackagePropsType = { hourRate: number | null | undefined, currencyCode: string | null | undefined };
const packages: PackageType[] = [
    {
        numberOfLessons: 1,
        headerKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.ONE_LESSON_HEADER,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.ONE_LESSON_DESCRIPTION,
        saleTranslationKey: undefined,
        sale: COMMON_CONSTANTS.PACKAGES.ONE_LESSON_SALE,
        mostPopular: false,
        borderColor: COLORS.BLACK_10,
        Icon: BronzeMedal
    },
    {
        numberOfLessons: 5,
        headerKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.FIVE_LESSONS_HEADER,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.FIVE_LESSONS_DESCRIPTION,
        saleTranslationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.FIVE_LESSONS_SALE,
        sale: COMMON_CONSTANTS.PACKAGES.FIVE_LESSONS_SALE,
        mostPopular: true,
        borderColor: COLORS.PRIMARY,
        Icon: SilverMedal
    },
    {
        numberOfLessons: 10,
        headerKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TEN_LESSONS_HEADER,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TEN_LESSONS_DESCRIPTION,
        saleTranslationKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TEN_LESSONS_SALE,
        sale: COMMON_CONSTANTS.PACKAGES.TEN_LESSONS_SALE,
        mostPopular: false,
        borderColor: COLORS.BLACK_10,
        Icon: GoldMedal
    }
]


const LessonSale = ({ translationKey, amount, currencyCode }: { translationKey: string, amount: number, currencyCode: string | null | undefined }) => {
    const { localize } = usePriceLocalization();

    return (
        <LessonSaleBoxStyled>
            <LessonSaleTypographyStyled>{localize({ price: -amount, translationKey })}</LessonSaleTypographyStyled>
        </LessonSaleBoxStyled>
    )
}

const TutorPackageSaleSelectionLargeItem = ({ tutorInfo, numberOfLessons, headerKey, descriptionKey, sale, saleTranslationKey, mostPopular, borderColor, Icon }: { tutorInfo: PackagePropsType } & PackageType) => {
    const { t } = useTranslation();
    const selectedNumberOfLessons = useAppSelector(selectNumberOfLessons);
    const duration = useAppSelector(selectDuration);
    const dispatch = useAppDispatch();

    const onButtonClick = () => {
        dispatch(setNumberOfLessons(numberOfLessons));
    }

    const { localize } = usePriceLocalization();
    const tutorPackageLessonPrice = getPriceWithSale(tutorInfo.hourRate ?? 0, sale, duration);
    const tutorTotalPackagePriceBeforeDiscount = getPriceWithSale((tutorInfo.hourRate ?? 0) * numberOfLessons, 0, duration);
    const tutorTotalPackagePriceAfterDiscount = getPriceWithSale((tutorInfo.hourRate ?? 0) * numberOfLessons, sale, duration);
    const discountAmount = tutorTotalPackagePriceBeforeDiscount - tutorTotalPackagePriceAfterDiscount;

    return (
        <PackageBoxStyled borderColor={borderColor} isMostPopular={mostPopular} onClick={onButtonClick}>
            {mostPopular ? <MostPopularBox>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.MOST_POPUPLAR)}</MostPopularBox> : <></>}
            <PackageContentBoxStyled>
                <PackageInnerBox>
                    <Box sx={{ display: "inline-flex", justifyContent: "space-between" }}>
                        <Box sx={{ display: "inline-flex", gap: "4px", alignItems: "center" }}>
                            <Icon />
                            <PackageHeaderTypography>{t(headerKey)}</PackageHeaderTypography>
                        </Box>
                        <RadioButton checked={selectedNumberOfLessons === numberOfLessons} nonActiveColor={COLORS.PRIMARY_40} size="large" />
                    </Box>
                    <Body1Styled>{t(descriptionKey)}</Body1Styled>
                </PackageInnerBox>
                <PackageInnerBox>
                    <PriceBoxWithSaleStyled>
                        <PackagePriceTypography>{localize({ price: tutorPackageLessonPrice })}</PackagePriceTypography>
                        <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.PRICE_DESCRIPTION_WITH_DURATION, { duration: getDurationInMinutes(duration) })}</Body1Styled>
                    </PriceBoxWithSaleStyled>
                    <PriceBoxWithSaleStyled>
                        <PriceTotalBoxStyled>
                            <Body1Styled sx={{ color: COLORS.BLACK }}>{localize({ price: tutorTotalPackagePriceAfterDiscount })}</Body1Styled>
                            <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TOTAL)}</Body1Styled>
                        </PriceTotalBoxStyled>
                        {saleTranslationKey ? <LessonSale translationKey={saleTranslationKey} amount={discountAmount} currencyCode={tutorInfo.currencyCode} /> : <></>}
                    </PriceBoxWithSaleStyled>
                </PackageInnerBox>
            </PackageContentBoxStyled>
        </PackageBoxStyled>
    )
}


const TutorPackageSaleSelectionLarge = ({ tutorInfo }: { tutorInfo: PackagePropsType }) => (
    <PackagesBoxStyled>
        {packages.map((x, idx) => <TutorPackageSaleSelectionLargeItem key={idx} tutorInfo={tutorInfo} {...x} />)}
    </PackagesBoxStyled>
)

export default TutorPackageSaleSelectionLarge;

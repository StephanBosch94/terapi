import { styled } from "@mui/material/styles";
import RadioButton from "components/styles/RadioButton";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { PackageType, selectPackageType, setPackageType } from "core/redux/availabilityCalendarSlice";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { NumberOfLessons } from "core/services/Types/AppTypes";
import { getPriceWithSale } from "core/services/Utils/userUtils";
import { useTranslation } from "components/core/TranslationProvider";
import Box from "@mui/material/Box";
import KidEmoji from "assets/emoji/kid_emoji.svg";
import SmartEmoji from "assets/emoji/smart_emoji.svg";
import WonderingEmoji from "assets/emoji/wondering_emoji.svg";
import { ClassDurationEnum } from "core/constants/enums/ClassDurationEnum";
import Body1Styled from "components/styles/typography/Body1Styled";
import PackagesBoxStyled from "components/styles/boxes/LargePackagesBoxStyled";
import H4Styled from "components/styles/typography/H4Styled";
import { usePriceLocalization } from "core/hooks/priceHook";

const RightPanelPackageBoxStyled = styled(Box)({
    display: "inline-flex",
    justifyContent: "center",
    alignItems: "flex-start",
    width: "100%",
    padding: "8px",
    gap: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    ":hover": {
        cursor: "pointer",
        border: `1px solid ${COLORS.PRIMARY}`,
    }
}) as typeof Box;

const TitleBoxStyled = styled(Box)({
    display: "inline-flex",
    gap: "8px",
    alignItems: "center",
    svg: { transform: "scale(1.15)" }
}) as typeof Box;

const TitleTypographyStyled = styled(Body1Styled)({
    color: COLORS.BLACK,
    fontWeight: 500
}) as typeof Body1Styled;

const PriceTypographyStyled = styled(Body1Styled)({
    display: "flex",
    alignItems: "center",
    gap: "4px",
    color: COLORS.BLACK,
    "b": {
        fontWeight: 700,
    }
}) as typeof Body1Styled;

const LargeItemPriceTypographyStyled = styled(Body1Styled)({
    gap: "4px",
    color: COLORS.BLACK,
    ".gray": {
        color: COLORS.BLACK_60
    },
    "b": {
        color: COLORS.PRIMARY,
        fontWeight: 700,
        fontSize: "1.5rem"
    }
}) as typeof Body1Styled;

const RightBoxStyled = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    width: "100%",
    gap: "4px"
}) as typeof Box;

const PackageBoxStyled = styled(Box)({
    display: "flex",
    justifyContent: "space-between",
    border: `1px solid ${COLORS.BLACK_10}`,
    backgroundColor: COLORS.WHITE,
    borderRadius: "8px",
    padding: "32px",
    gap: "32px",
    flexFlow: "column noWrap",
    width: "100%",
    height: "100%",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5
    }
}) as typeof Box;

const PackageInnerBox = styled(Box)({
    display: "flex",
    flexFlow: "column noWrap",
    padding: "0px",
    gap: "24px"
}) as typeof Box;

const PackageHeaderTypography = styled(H4Styled)({
    color: COLORS.BLACK,
    fontWeight: "600"
}) as typeof H4Styled;

type PackageViewType = {
    type: PackageType,
    numberOfLessons: NumberOfLessons,
    titleKey: string,
    descriptionKey: string,
    duration: ClassDurationEnum,
    Icon: string,
    maxPossibleSale: number,
    isChildType: boolean
};
type PackagePropsType = { hourRate: number | null | undefined, currencyCode: string | null | undefined, teachesLessonsForChildren: boolean | undefined };
type PackageItemPropsType = { tutorInfo: PackagePropsType } & PackageViewType & { priceKey: string };
type ViewType = "small" | "large";
const packages: PackageViewType[] = [
    {
        type: "trial",
        numberOfLessons: 1,
        titleKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TRIAL_LESSON,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.TRIAL_LESSON_DESCRIPTION,
        duration: ClassDurationEnum.Half,
        Icon: WonderingEmoji,
        maxPossibleSale: 0,
        isChildType: false
    },
    {
        type: "standard",
        numberOfLessons: 5,
        titleKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.STANDART_LESSON,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.STANDART_LESSON_DESCRIPTION,
        duration: ClassDurationEnum.Full,
        Icon: SmartEmoji,
        maxPossibleSale: COMMON_CONSTANTS.PACKAGES.TEN_LESSONS_SALE,
        isChildType: false
    },
    {
        type: "kids",
        numberOfLessons: 10,
        titleKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.LESSON_FOR_KIDS,
        descriptionKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGES.LESSON_FOR_KIDS_DESCRIPTION,
        duration: ClassDurationEnum.Half,
        Icon: KidEmoji,
        maxPossibleSale: COMMON_CONSTANTS.PACKAGES.TEN_LESSONS_SALE,
        isChildType: true
    }
]

type PriceType = { type: PackageType, priceKey: string };
const packagePriceTitles: { normal: PriceType[], large: PriceType[] } = {
    normal: [
        {
            type: "trial",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_SMALL.PRICE_DURATION,
        },
        {
            type: "standard",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_SMALL.FROM_PRICE_DURATION,
        },
        {
            type: "kids",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_SMALL.FROM_PRICE_DURATION,
        }
    ],
    large: [
        {
            type: "trial",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_LARGE.PRICE_DURATION,
        },
        {
            type: "standard",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_LARGE.FROM_PRICE_DURATION,
        },
        {
            type: "kids",
            priceKey: TRANSLATION_KEYS.TUTOR_PUBLIC_PROFILE.PACKAGE_SELECTION_LARGE.FROM_PRICE_DURATION,
        }
    ]
}

const RadioButtonWithBox = ({ checked }: { checked: boolean }) => (
    <Box sx={{ padding: "2px 0" }}><RadioButton checked={checked} /></Box>
);

const TutorPackageItemNormal = ({ type, Icon, duration, maxPossibleSale, tutorInfo, priceKey, titleKey }: PackageItemPropsType) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const selectedPackageType = useAppSelector(selectPackageType);
    const { localize } = usePriceLocalization();
    const handleNumberOfLessonsChange = () => dispatch(setPackageType(type));
    const price = getPriceWithSale(tutorInfo.hourRate ?? 0, maxPossibleSale, duration);

    return (
        <RightPanelPackageBoxStyled onClick={handleNumberOfLessonsChange}>
            <RadioButtonWithBox checked={selectedPackageType === type} />
            <RightBoxStyled>
                <TitleBoxStyled>
                    <Icon />
                    <TitleTypographyStyled>{t(titleKey)}</TitleTypographyStyled>
                </TitleBoxStyled>

                <PriceTypographyStyled
                    dangerouslySetInnerHTML={{ __html: localize({ translationKey: priceKey, price, duration }) }}
                />
            </RightBoxStyled>
        </RightPanelPackageBoxStyled>
    )
}

const TutorPackageItemLarge = ({ type, Icon, duration, maxPossibleSale, tutorInfo, priceKey, titleKey, descriptionKey }: PackageItemPropsType) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const selectedPackageType = useAppSelector(selectPackageType);
    const { localize } = usePriceLocalization();
    const handleNumberOfLessonsChange = () => dispatch(setPackageType(type));
    const price = getPriceWithSale(tutorInfo.hourRate ?? 0, maxPossibleSale, duration);

    return (
        <PackageBoxStyled onClick={handleNumberOfLessonsChange}>
            <PackageInnerBox>
                <Box sx={{ display: "inline-flex", justifyContent: "space-between", }}>
                    <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
                        <Box sx={{ display: "flex", alignItems: "center", width: "24px", height: "24px", svg: { transform: "scale(1.5)" } }}><Icon /></Box>
                        <PackageHeaderTypography>{t(titleKey)}</PackageHeaderTypography>
                    </Box>
                    <RadioButton checked={selectedPackageType === type} nonActiveColor={COLORS.PRIMARY_40} size="large" />
                </Box>
                <Body1Styled>{t(descriptionKey)}</Body1Styled>
            </PackageInnerBox>
            <PackageInnerBox>
                <LargeItemPriceTypographyStyled
                    dangerouslySetInnerHTML={{ __html: localize({ translationKey: priceKey, price, duration }) }}
                />
            </PackageInnerBox>
        </PackageBoxStyled>
    )
}

const TutorPackageSelectionSmall = ({ tutorInfo }: { tutorInfo: PackagePropsType }) => {
    const priceKey = packagePriceTitles.normal;

    return (
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: "8px" }}>
            {packages
                .filter(x => !x.isChildType || tutorInfo.teachesLessonsForChildren === true)
                .map((x, idx) => (
                    <TutorPackageItemNormal key={idx} tutorInfo={tutorInfo} priceKey={priceKey.find(y => y.type === x.type)!.priceKey} {...x} />
                ))}
        </Box>
    )
}

const TutorPackageSelectionLarge = ({ tutorInfo }: { tutorInfo: PackagePropsType }) => {
    const priceKey = packagePriceTitles.large;

    return (
        <PackagesBoxStyled>
            {packages
                .filter(x => !x.isChildType || tutorInfo.teachesLessonsForChildren === true)
                .map((x, idx) => <TutorPackageItemLarge key={idx} tutorInfo={tutorInfo} priceKey={priceKey.find(y => y.type === x.type)!.priceKey} {...x} />)}
        </PackagesBoxStyled>
    )
}

const TutorPackageSelection = ({ tutorInfo, variant }: { tutorInfo: PackagePropsType, variant: ViewType }) => {
    return (
        <>
            {variant === "small" ? <TutorPackageSelectionSmall tutorInfo={tutorInfo} /> : <></>}
            {variant === "large" ? <TutorPackageSelectionLarge tutorInfo={tutorInfo} /> : <></>}
        </>
    )
}

export default TutorPackageSelection;
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { useTranslation } from "components/core/TranslationProvider";
import React, { useContext, useEffect, useState, createContext, useRef, useCallback } from "react";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { CodetableSpecializationsReponse, CodetableSubjectsResponse } from "core/redux/codetableSlice";
import Slider from '@mui/material/Slider';
import { selectPriceStatistics, selectPriceStatisticsStatus, setPriceStatistics } from "core/redux/userStatisticsSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import { useRouter } from "next/router";
import routes from "routes/routes";
import { UserSearchQueryType, UserSearchQueryParsed, SpecializationType } from "core/services/Types/AppTypes";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import MenuCustom from "components/menu/Menu";
import ErrorLarge from "components/core/error/ErrorLarge";
import { UIContext } from "components/core/UIContextProvider";
import { MenuItemBoxStyled } from "components/styles/boxes/MenuItemBoxStyled";
import { MuiCheckboxStyled } from "components/styles/MuiCheckboxStyled";
import { FilterTutorsResponseBody } from "core/redux/userSlice";
import { useAppDataContext } from "components/core/providers/AppProvider";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";
import { contextFactoryCreate } from "core/redux/reduxHooks";
import MunuInputIcon from "components/styles/icons/MenuInputIcon";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import Skeleton from "@mui/material/Skeleton";
import InjectIcon, { IconCategoryType } from 'components/core/icons/InjectIcon';
import UserService from 'core/services/UserService';
import { getJoinedPagedVM } from 'core/services/Utils/paginationUtil';
import { DesktopBoxStyled } from 'components/styles/boxes/DesktopBoxStyled';
import Empty from 'components/core/empty/Empty';
import { apiHasMoreItems } from 'core/services/Utils/apiUtil';
import { COMMON_CONSTANTS } from 'core/constants/commonConstants';
import { MobileBoxStyled } from 'components/styles/boxes/MobileBoxStyled';
import CircularProgress from '@mui/material/CircularProgress';
import Body1Styled from 'components/styles/typography/Body1Styled';
import InfiniteScroll from "react-infinite-scroll-component";
import PStyled from "components/styles/typography/PStyled";
import H1Styled from "components/styles/typography/H1Styled";
import { getOffsetTopRecursively } from "core/services/Utils/htmlElementUtil";
import { ModalStyled } from "components/styles/ModalStyled";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { getRelativeUrlWithLoacle } from "core/services/Utils/urlUtil";
import SearchGreeting from "./SearchGreeting";
import { ContainerStyled } from "components/styles/ContainerStyled";
import TutorItem from "components/tutor/TutorItem";
import { filterAndGroupSpecializations } from "core/services/Utils/specializationUtil";
import { usePriceLocalization } from "core/hooks/priceHook";

type SourceType = "specialization_page" | "subject_page" | "search_page";
type PropsType = {
    relativePath: string,
    source: SourceType,
    query?: UserSearchQueryParsed,
    initData?: FilterTutorsResponseBody,
    disabledSpecializationId?: number
};
const NON_ACTIVE_TEXT_COLOR = COLORS.BLACK_60;

const InputInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "8px",
})) as typeof Box;

const CheckboxStyled = styled(MuiCheckboxStyled)(() => sx({
    padding: 0
})) as typeof MuiCheckboxStyled;

const NonClickableMenuItemBoxStyled = styled(Box)(() => sx({
    gap: "16px",
    padding: "12px 8px"
})) as typeof Box;

const CategoryMenuItemBoxStyled = styled(Box)(() => sx({
    gap: "16px",
    padding: "4px 8px",
    borderBottom: `1px solid ${COLORS.BLACK_10}`
})) as typeof Box;

const CustomMenuItemBoxStyled = styled(MenuItemBoxStyled)(() => sx({
    gap: "16px",
    padding: "12px 8px"
})) as typeof MenuItemBoxStyled;

const PricePerHourMenuBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    padding: "16px",
    gap: "16px"
})) as typeof Box;

const SubjectMenuBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    padding: "8px"
})) as typeof Box;

const PriceTypographyStyled = styled(Body1Styled)(() => sx({
    fontWeight: "500",
    color: COLORS.BLACK
})) as typeof Body1Styled;

type ScrollableType = {
    scrolled: string,
    isdesktop: string
}

const SearchControllersBoxStyled = styled(Box)<ScrollableType>(({ scrolled: isScrolled, isdesktop }) => sx({
    backgroundColor: isScrolled === "true" && isdesktop === "true" ? COLORS.PRIMARY : "inherit",
    padding: isScrolled === "true" && isdesktop === "true" ? "12px" : 0,
    display: "flex",
    flexFlow: "column noWrap",
    position: { xs: "static", md: "sticky" },
    top: "0px",
    zIndex: isdesktop === "true" ? "1000" : "0",
    gap: isScrolled === "true" && isdesktop === "true" ? "4px" : "16px",
}));

const TitleDektopBoxStyled = styled(Box)<ScrollableType>(({ scrolled: isScrolled, isdesktop }) => sx({
    display: "inline-flex",
    alignItems: "center",
    gap: "12px",
    color: isScrolled === "true" && isdesktop === "true" ? COLORS.WHITE : COLORS.BLACK,
    "svg": {
        transform: "scale(2.5)",
        margin: "auto 8px"
    }
}));

const SearchControllersInnerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: { xs: "column noWrap", md: "row noWrap" },
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "4px",
    padding: "8px",
    fontSize: "1rem",
    gap: "8px",
    position: "sticky",
    top: "80px",
    zIndex: "1000"
})) as typeof Box;

const InputBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    height: "40px",
    border: "none",
    borderRadius: "4px",
    padding: "8px 12px 8px 16px",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    fontSize: "0.875rem",
    fontWeight: 400,
    width: "100%",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5
    },
    "fieldset": {
        border: "none",
    }
})) as typeof Box;

const ResetBoxStyled = styled(Box)(() => sx({
    ":hover": {
        cursor: "pointer"
    }
})) as typeof Box;

// Workaround for slider overflow bug
const MuiSliderBox = styled(Box)(() => sx({
    overflow: "hidden",
    boxSizing: "content-box",
    padding: "0 10px 0 0"
})) as typeof Box;

const BadgeBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.PRIMARY,
    borderRadius: "4px",
    color: COLORS.WHITE,
    padding: "4px 6px",
    ":hover": {
        backgroundColor: COLORS.PRIMARY_HOVER,
        color: COLORS.WHITE,
        cursor: "pointer"
    }
})) as typeof Box;

const BadgeTypographyStyled = styled(Box)(() => sx({
    fontWeight: "500",
    fontSize: "0.75rem"
})) as typeof Box;

const ResetTypographyStyled = styled(Body1Styled)(() => sx({
    textDecoration: "underline",
    color: COLORS.PRIMARY
})) as typeof Body1Styled;

const SliderStyled = styled(Slider)(() => sx({
    color: COLORS.PRIMARY,
    width: "100%",
    padding: "0",
    height: "20px",
    '& .MuiSlider-thumb': {
        height: "20px",
        width: "20px",
        backgroundColor: COLORS.WHITE,
        border: `1px solid ${COLORS.PRIMARY}`,
        '&:focus, &:hover, &.Mui-active': {
            boxShadow: "none"
        }
    },
    '& .MuiSlider-track': {
        border: 'none',
        height: "4px"
    },
    '& .MuiSlider-rail': {
        opacity: 1,
        backgroundColor: COLORS.BLACK_10,
        height: "4px",
    },
})) as typeof Slider;

const SearchItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "auto",
    flex: "1 1 auto"
})) as typeof Box;

const MainBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexWrap: "wrap",
    width: "100%",
    gap: "16px",
    overflowY: "auto",
    ".infinite-scroll-component__outerdiv": {
        display: "flex",
        width: "100%",
    }
})) as typeof Box;

const SearchResultsBoxStyled = styled(Box)(() => sx({
    border: `1px solid ${COLORS.BLACK_10}`,
    borderRadius: "4px",
    width: "100%",

})) as typeof Box;

const SkeletonItemStyled = styled(Skeleton)(() => sx({
    borderRadius: "4px",
})) as typeof Skeleton;

const TitleMobileBoxStyled = styled(Box)(() => sx({
    display: "block",
    "> *": {
        width: "fit-content",
    },
    "svg": {
        transform: "scale(2.5)",
        margin: "auto 8px"
    }
})) as typeof Box;

const SvgBoxItem = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    "svg": {
        transform: "scale(2.5)"
    }
})) as typeof Box;

const NO_RESULTS_FOUND = {
    titleKey: TRANSLATION_KEYS.SEARCH.NO_RESULTS.TITLE,
    descriptionKey: TRANSLATION_KEYS.SEARCH.NO_RESULTS.DESCRIPTION,
    buttonTextKey: TRANSLATION_KEYS.SEARCH.NO_RESULTS.CLEAR_FILTERS_BUTTON,
    imagePath: "/images/not_found.png",
}

type SearchContextType = {
    users: FilterTutorsResponseBody | undefined,
    usersStatus: LoadingStatusEnum | undefined,
    specializationIds: number[],
    subjectId: number,
    toTrialPrice: number,
    source: SourceType,
    disabledSpecializationId?: number,
    setSpecializationIds: (newSpecializationIds: number[]) => void,
    setSubjectId: (newSubjectId: number) => void,
    setToTrialPrice: (newHourRate: number) => void,
    handleSubmit: () => void,
    fetchNext: () => void
} | undefined;

const SearchContext = createContext<SearchContextType>(undefined);
export const useSearchContext = contextFactoryCreate('Search', SearchContext);

const getQuery = ({ subjectId, toTrialPrice, specializationIds, filtersApplied }: UserSearchQueryParsed): UserSearchQueryType => {
    var query: UserSearchQueryType = {};
    if (subjectId && subjectId > 0) {
        query.subjectId = `${subjectId}`;
    }
    if (toTrialPrice && toTrialPrice > 0) {
        query.toTrialPrice = `${toTrialPrice}`;
    }
    if (specializationIds && specializationIds.length) {
        query.specializationIds = specializationIds.join(',');
    }
    if (filtersApplied) {
        query.filtersApplied = `${filtersApplied}`;
    }
    return query;
}

const SearchProvider = ({ relativePath, query: inputQuery = {}, initData, source, children, disabledSpecializationId }: PropsType & { children: any }) => {
    const router = useRouter();
    const { subjects } = useAppDataContext();
    const [users, setUsers] = useState<FilterTutorsResponseBody | undefined>(initData);
    const [usersStatus, setUsersStatus] = useState<LoadingStatusEnum | undefined>();
    const [subjectId, setSubjectId] = useState(inputQuery?.subjectId ?? 0);
    const [specializationIds, setSpecializationIds] = useState<number[]>(inputQuery?.specializationIds ?? []);
    const [toTrialPrice, setToTrialPrice] = useState(inputQuery?.toTrialPrice ?? 0);

    useEffect(() => {
        setUsers(initData);
        if (!initData) {
            handleSubmit();
        }
    }, [initData]);

    useEffect(() => {
        setSubjectId(inputQuery?.subjectId ?? 0);
        setSpecializationIds(inputQuery?.specializationIds ?? []);
        setToTrialPrice(inputQuery?.toTrialPrice ?? 0);
    }, [inputQuery]);

    const fetchNext = async (newSkip: number, appendType: AppendTypeEnum, query: UserSearchQueryParsed) => {
        try {
            setUsersStatus(LoadingStatusEnum.Loading);
            const result = await UserService.filterTutors(
                {
                    filter: {
                        specializationIds: query.specializationIds,
                        toTrialPrice: query.toTrialPrice,
                        subjectId: query.subjectId
                    },
                    skip: newSkip
                }
            );
            const joined = getJoinedPagedVM(users, result.data, appendType);
            setUsers(joined);
            setUsersStatus(LoadingStatusEnum.Loaded);
        } catch (err) {
            setUsersStatus(LoadingStatusEnum.Error);
        }
    }

    const handleSubmit = () => {
        router.push({
            pathname: relativePath,
            query: getQuery({ subjectId, toTrialPrice: toTrialPrice, specializationIds })
        })
    }

    const handleSubjectChange = (newVal: number) => {
        setSubjectId(newVal);

        if (source === "subject_page" || source === "specialization_page") {
            const subject = subjects.find(x => x.subjectId === newVal)!;
            router.push({
                pathname: routes.subjectLandingPage.path(subject.enumKey ?? ""),
                query: getQuery({ toTrialPrice, specializationIds })
            })
            return;
        }

        router.push({
            pathname: relativePath,
            query: getQuery({ subjectId: newVal, toTrialPrice: toTrialPrice, specializationIds, filtersApplied: true })
        })
    }

    const handleSpecializationIdsChange = (newVal: number[]) => {
        setSpecializationIds(newVal);

        router.push({
            pathname: relativePath,
            query: getQuery({ subjectId, toTrialPrice: toTrialPrice, specializationIds: newVal, filtersApplied: true })
        })
    }

    const handleToTrialPriceChange = (newVal: number) => {
        setToTrialPrice(newVal);
        router.push({
            pathname: relativePath,
            query: getQuery({ subjectId, toTrialPrice: newVal, specializationIds, filtersApplied: true })
        })
    }

    return (
        <SearchContext.Provider value={{
            users,
            usersStatus,
            subjectId,
            specializationIds,
            toTrialPrice: toTrialPrice,
            disabledSpecializationId,
            source,
            handleSubmit: handleSubmit,
            fetchNext: () => fetchNext(users?.data.length ?? 0, AppendTypeEnum.End, { subjectId, toTrialPrice: toTrialPrice, specializationIds }),
            setSubjectId: handleSubjectChange,
            setSpecializationIds: handleSpecializationIdsChange,
            setToTrialPrice: handleToTrialPriceChange
        }}>
            {children}
        </SearchContext.Provider>
    )
}

const TutorItemSkeleton = () => {

    return (
        <>
            <DesktopBoxStyled>
                <SkeletonItemStyled animation="wave" variant="rectangular" height="240px" width="100%" />
            </DesktopBoxStyled>
            <MobileBoxStyled>
                <SkeletonItemStyled animation="wave" variant="rectangular" height="288px" width="100%" />
            </MobileBoxStyled>
        </>
    )
}

const TutorItemListSkeleton = ({ itemsNumber }: { itemsNumber: number }) => {

    return (
        <MainBoxStyled>
            <Skeleton animation="wave" variant="text" width="120px" height="21px" />
            {Array(itemsNumber).fill(1).map((i, idx) => <TutorItemSkeleton key={idx} />)}
        </MainBoxStyled>
    )
}

const TutorItemsError = () => {

    return (
        <ErrorLarge />
    )
}

const InfinityLoadSkeleton = () => {

    return (
        <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", height: "30px", width: "100%" }}>
            <CircularProgress size={20} />
        </Box>
    )
}

const NoResultsFound = () => {
    const router = useRouter();

    const handleClick = () => {
        router.push(routes.search.path);
    }

    return (
        <Empty content={NO_RESULTS_FOUND} onClick={handleClick} />
    )
}

const TutorItemList = () => {
    const { users, usersStatus, fetchNext } = useSearchContext();
    const { t } = useTranslation();
    const usersLength = users?.data.length ?? 0;

    return (
        <MainBoxStyled>
            {!users && usersStatus === LoadingStatusEnum.Loading ? <TutorItemListSkeleton itemsNumber={usersLength + COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW} /> : <></>}
            {usersStatus === LoadingStatusEnum.Error ? <TutorItemsError /> : <></>}
            {users && !users.size ? <NoResultsFound /> : <></>}
            {users && users.size ? (
                <>
                    <Body1Styled sx={{ color: COLORS.BLACK }}>{t(TRANSLATION_KEYS.SEARCH.NUMBER_OF_RESULTS, { count: users?.size })}</Body1Styled>
                    <InfiniteScroll
                        dataLength={users.data.length}
                        next={fetchNext}
                        hasMore={apiHasMoreItems(users.size!, usersLength)}
                        loader={<InfinityLoadSkeleton />}
                        inverse={false}
                    >
                        <MainBoxStyled>
                            {users.data.map((x, idx) => (
                                <SearchResultsBoxStyled key={x.userId!}>
                                    <TutorItem user={x} />
                                </SearchResultsBoxStyled>))}
                        </MainBoxStyled>
                    </InfiniteScroll>
                </>
            ) : <></>}
        </MainBoxStyled>
    )
}

const PricePerHourInnerMenuSkeleton = () => {
    return (
        <Grid container spacing={2}>
            <Grid item xs={4}><Skeleton variant="text" animation="wave" /></Grid>
            <Grid item xs={12}><Skeleton variant="rectangular" height="20px" animation="wave" /></Grid>
            <Grid item xs={4}><Skeleton variant="text" animation="wave" /></Grid>
        </Grid>
    )
}

const TextMenuItem = ({ text, onClick }: { text: string, onClick?: () => void }) => {

    return (
        <CustomMenuItemBoxStyled onClick={onClick}>
            <Typography>
                {text}
            </Typography>
        </CustomMenuItemBoxStyled>
    )
}

const MenuItem = ({ item }: { item: { nameKey?: string | null | undefined, iconCategory: IconCategoryType, iconName?: string | null | undefined } }) => {
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    return (
        <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
            {screenType === "desktop" ? (
                <Box sx={{ display: "flex", alignItems: "center", svg: { transform: "scale(1.25)", overflow: "visible" }, image: { overflow: "visible" }, width: "20px", height: "20px", }}>
                    <InjectIcon category={item.iconCategory} iconName={item.iconName ?? ""} />
                </Box>
            ) : <></>}

            <Typography>{t(item.nameKey ?? "")}</Typography>
        </Box >
    )
}

const PricePerHour = () => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const [open, setOpen] = useState(false);
    const [selectedTrialPrice, setSelectedToRate] = useState(0);
    const { toTrialPrice, setToTrialPrice } = useSearchContext();
    const priceStatistics = useAppSelector(selectPriceStatistics);
    const priceStatisticsStatus = useAppSelector(selectPriceStatisticsStatus);
    const { localize } = usePriceLocalization();

    useEffect(() => {
        dispatch(setPriceStatistics());
    }, []);

    useEffect(() => {
        if (priceStatisticsStatus == LoadingStatusEnum.Loaded && priceStatistics) {
            setSelectedToRate(toTrialPrice || priceStatistics.maxTrialPrice!); // do not update parent state here
        }
    }, [toTrialPrice, priceStatistics, priceStatisticsStatus]);

    const handleChange = (
        event: Event,
        newValue: number | number[],
        activeThumb: number,
    ) => {
        setToTrialPrice(newValue as number);
        setSelectedToRate(newValue as number);
    };

    const handleReset = () => {
        if (!priceStatistics) return;
        setSelectedToRate(priceStatistics.maxTrialPrice ?? 0);
        setToTrialPrice(priceStatistics.maxTrialPrice ?? 0);
    }

    const isPriceSet = () => {
        if (!priceStatistics || !selectedTrialPrice) {
            return false;
        }

        return selectedTrialPrice !== priceStatistics.maxTrialPrice;
    }

    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    return (
        <MenuCustom
            sameWidths={true}
            content={
                <InputBoxStyled>
                    <InputInnerBoxStyled sx={{ color: isPriceSet() && !open ? "inherit" : NON_ACTIVE_TEXT_COLOR }}>
                        <Typography>
                            {
                                isPriceSet() && !open
                                    ? t(TRANSLATION_KEYS.SEARCH.PRICE, { fromPrice: localize({ price: priceStatistics?.minTrialPrice ?? 0 }), toPrice: localize({ price: selectedTrialPrice }) })
                                    : t(TRANSLATION_KEYS.SEARCH.PRICE_PLACEHOLDER)
                            }
                        </Typography>
                    </InputInnerBoxStyled>
                    <MunuInputIcon isOpen={open} />
                </InputBoxStyled>
            }
            innerMenu={
                <PricePerHourMenuBoxStyled>
                    {priceStatisticsStatus === LoadingStatusEnum.Loading ? <PricePerHourInnerMenuSkeleton /> : <></>}
                    {priceStatisticsStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
                    {priceStatisticsStatus === LoadingStatusEnum.Loaded && priceStatistics ? (
                        <>
                            <PriceTypographyStyled>
                                {t(TRANSLATION_KEYS.SEARCH.PRICE, { fromPrice: localize({ price: priceStatistics?.minTrialPrice ?? 0 }), toPrice: localize({ price: selectedTrialPrice }) })}
                            </PriceTypographyStyled>
                            <MuiSliderBox>
                                <SliderStyled
                                    value={selectedTrialPrice}
                                    onChange={handleChange}
                                    min={priceStatistics?.minTrialPrice}
                                    max={priceStatistics?.maxTrialPrice}
                                />
                            </MuiSliderBox>
                            <ResetBoxStyled onClick={handleReset}>
                                <ResetTypographyStyled>{t(TRANSLATION_KEYS.SEARCH.RESET)}</ResetTypographyStyled>
                            </ResetBoxStyled>
                        </>
                    ) : <></>}
                </PricePerHourMenuBoxStyled >
            }
            sx={{
                width: "100%"
            }}
            onClick={handleClick}
        />
    )
}

const SubjectItemWithIcon = ({ subject, onClick }: { subject: CodetableSubjectsResponse[0], onClick: () => void }) => {

    return (
        <CustomMenuItemBoxStyled onClick={() => onClick()}>
            <MenuItem item={{ ...subject, iconCategory: "emoji" }} />
        </CustomMenuItemBoxStyled>
    )
}

const SpecializationItemWithCheckbox = ({ specialization, checked, onClick, disabled = false }: { specialization: CodetableSpecializationsReponse[0], checked: boolean, onClick: () => void, disabled?: boolean }) => {

    return (
        <CustomMenuItemBoxStyled onClick={() => onClick()} disabled={disabled}>
            <CheckboxStyled checked={checked} disabled={disabled} />
            <MenuItem item={{ ...specialization, iconCategory: "specializations" }} />
        </CustomMenuItemBoxStyled>
    )
}

const SpecializationBadge = () => {
    const { specializationIds } = useSearchContext();
    return (
        <BadgeBoxStyled>
            <BadgeTypographyStyled>+{specializationIds.length - 1}</BadgeTypographyStyled>
        </BadgeBoxStyled>
    )
}

const SpecializationItemWithBadge = () => {
    const { specializationIds } = useSearchContext();
    const { specializations } = useAppDataContext();
    const firstSpecialization = specializationIds.length > 0 ? getSpecializationById(specializations, specializationIds[0]) : undefined;

    return (
        <Box sx={{ display: "inline-flex", gap: "8px", alignItems: "center" }}>
            <MenuItem item={{ ...firstSpecialization, iconCategory: "specializations" }} />
            <SpecializationBadge />
        </Box>
    )
}

const SpecializationOverviewItem = () => {
    const { t } = useTranslation();
    const { specializationIds } = useSearchContext();
    const { specializations } = useAppDataContext();
    const firstSpecialization = specializationIds.length > 0 ? getSpecializationById(specializations, specializationIds[0]) : undefined;


    return (
        <>
            {!specializationIds.length ? <Typography>{t(TRANSLATION_KEYS.SEARCH.SPECIALIZATIONS)}</Typography> : <></>}
            {specializationIds.length === 1 ? <MenuItem item={{ ...firstSpecialization, iconCategory: "specializations" }} /> : <></>}
            {specializationIds.length > 1 ? <SpecializationItemWithBadge /> : <></>}
        </>
    )
}

const SpecializationCategory = ({ categoryNameKey }: { categoryNameKey: string }) => {
    const { t } = useTranslation();

    return (
        <CategoryMenuItemBoxStyled>{t(categoryNameKey)}</CategoryMenuItemBoxStyled>
    )
}

const EmptySpecializationItem = () => {
    const { t } = useTranslation();

    return (
        <NonClickableMenuItemBoxStyled>{t(TRANSLATION_KEYS.SEARCH.SPECIALIZATION_SELECT_SUBJECT)}</NonClickableMenuItemBoxStyled>
    )
}

const getSpecializationById = (specializations: CodetableSpecializationsReponse, specializationsId: number) => {
    return specializations.find(x => x.specializationId === specializationsId)
}

const Specializations = () => {
    const { subjectId, specializationIds, setSpecializationIds, disabledSpecializationId } = useSearchContext();
    const [open, setOpen] = useState(false);
    const { specializations } = useAppDataContext();
    const currentSpecializations = filterAndGroupSpecializations({ specializations, subjectId });

    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    const handleItemClick = (newId: number) => {
        const index = specializationIds.findIndex(x => x === newId);

        if (index === -1) {
            setSpecializationIds([...specializationIds, newId]);
            return;
        }
        setSpecializationIds([...specializationIds.slice(0, index), ...specializationIds.slice(index + 1, specializationIds.length)])
    }

    return (
        <MenuCustom
            sameWidths={true}
            fullHeight={false}
            content={
                <InputBoxStyled>
                    <InputInnerBoxStyled sx={{ color: specializationIds.length ? "inherit" : NON_ACTIVE_TEXT_COLOR }}>
                        <SpecializationOverviewItem />
                    </InputInnerBoxStyled>
                    <MunuInputIcon isOpen={open} />
                </InputBoxStyled>
            }
            innerMenu={
                <SubjectMenuBoxStyled>
                    {currentSpecializations.map((x, idx) => (
                        <React.Fragment key={x.specializationId!}>
                            {x.includeCategoryTitle ? <SpecializationCategory categoryNameKey={x.specializationCategoryNameKey ?? ""} /> : <></>}
                            <SpecializationItemWithCheckbox
                                specialization={x}
                                onClick={() => handleItemClick(x.specializationId!)}
                                checked={specializationIds.some(y => y === x.specializationId!)}
                                disabled={disabledSpecializationId === x.specializationId}
                            />
                        </React.Fragment>
                    ))}
                    {subjectId === 0 ? <EmptySpecializationItem /> : <></>}
                </SubjectMenuBoxStyled >
            }
            sx={{
                width: "100%"
            }}
            onClick={handleClick}
        />
    )
}

const SubjectSelectOverview = ({ selectedId }: { selectedId: number }) => {
    const { subjects } = useAppDataContext();
    const { t } = useTranslation();
    return (
        <>
            {selectedId === -1 ? <Typography>{t(TRANSLATION_KEYS.SEARCH.ALL_SUBJECTS)}</Typography> : <></>}
            {selectedId === 0 ? <Typography>{t(TRANSLATION_KEYS.SEARCH.SUBJECT_PLACEHOLDER)}</Typography> : <></>}
            {selectedId > 0 ? <MenuItem item={{ ...subjects.find(x => x.subjectId === selectedId), iconCategory: "emoji" }} /> : <></>}
        </>
    )
}

const SubjectSelect = () => {
    const { subjectId, setSubjectId, source } = useSearchContext();
    const [open, setOpen] = useState(false);
    const { t } = useTranslation();
    const { subjects } = useAppDataContext();

    const handleClick = (newValue: boolean) => {
        setOpen(newValue);
    }

    const handleItemClick = (id: number) => {
        setSubjectId(id);
    }

    return (
        <MenuCustom
            sameWidths={true}
            content={
                <InputBoxStyled>
                    <InputInnerBoxStyled sx={{ color: subjectId ? "inherit" : NON_ACTIVE_TEXT_COLOR }}>
                        <SubjectSelectOverview selectedId={subjectId} />
                    </InputInnerBoxStyled>
                    <MunuInputIcon isOpen={open} />
                </InputBoxStyled>
            }
            innerMenu={
                <SubjectMenuBoxStyled>
                    {source === "search_page" ? <TextMenuItem onClick={() => handleItemClick(-1)} text={t(TRANSLATION_KEYS.SEARCH.ALL_SUBJECTS)} /> : <></>}
                    {subjects.map((x, idx) => (<SubjectItemWithIcon key={x.subjectId!} subject={x} onClick={() => handleItemClick(x.subjectId!)} />))}
                </SubjectMenuBoxStyled >
            }
            sx={{
                width: "100%"
            }}
            onClick={handleClick}
            autoClose={true}
        />
    )
}

const SubjectPageTitle = ({ isScrolled, query }: { isScrolled: boolean, query: UserSearchQueryParsed }) => {
    const { t } = useTranslation();
    const { subjectId, users } = useSearchContext();
    const { subjects } = useAppDataContext();
    const subject = subjects.find(x => x.subjectId === subjectId);
    const { screenType } = useContext(UIContext);
    const isDesktop = screenType === "desktop";

    const ref = useCallback((node: HTMLElement | null) => {
        if (node !== null && query.filtersApplied) {
            window.scrollTo({ top: getOffsetTopRecursively(node) });
        }
    }, [query]);

    return (
        <TitleDektopBoxStyled scrolled={isScrolled.toString()} isdesktop={isDesktop.toString()} ref={ref}>
            {screenType === "desktop" ? (
                <>
                    <Box>
                        <PStyled sx={{ fontSize: { xs: "1.5rem !important", md: "2rem !important", fontWeight: "800", color: "inherit" } }}>{t(subject?.nameKey ?? "")}</PStyled>
                    </Box>
                    <SvgBoxItem>
                        <InjectIcon category="emoji" iconName={subject?.iconName ?? ""} size={40} />
                    </SvgBoxItem>
                </>
            ) : <></>}
            <Box>
                <H1Styled sx={{ fontSize: { xs: "1.5rem !important", md: "2rem !important" } }}>{t(TRANSLATION_KEYS.SEARCH.SUBJECT_PAGE_TITLE, { count: users?.size ?? 0 })}</H1Styled>
            </Box>
        </TitleDektopBoxStyled>
    )
}

const SearchHandlers = ({ query, source }: { query: UserSearchQueryParsed, source: SourceType }) => {
    const [isScrolled, setScrolled] = React.useState(false);
    const initOffset = useRef(0);
    const { screenType } = useContext(UIContext);
    const isDesktop = screenType === "desktop";

    const ref = useCallback((node: HTMLElement | null) => {
        if (node !== null) {
            const offset = getOffsetTopRecursively(node.parentElement);
            initOffset.current = offset;
            setScrolled(window.scrollY >= offset);
        }
    }, []);

    useEffect(() => {
        const updatePosition = () => {
            const offset = initOffset.current;
            const scrolled = window.scrollY >= offset;
            if (scrolled === isScrolled) {
                return;
            }
            setScrolled(scrolled);
        }

        window.addEventListener('scroll', updatePosition)
        return () => window.removeEventListener('scroll', updatePosition)
    }, [isScrolled])

    return (
        <SearchControllersBoxStyled scrolled={isScrolled.toString()} isdesktop={isDesktop.toString()} ref={ref}>
            <ContainerStyled>
                {source === "subject_page" ? <SubjectPageTitle isScrolled={isScrolled} query={query} /> : <></>}
                {source === "search_page" ? <SearchGreeting isScrolled={isScrolled} /> : <></>}
                {source === "specialization_page" ? <SearchGreeting isScrolled={isScrolled} textVariant="body1" /> : <></>}
            </ContainerStyled>
            <ContainerStyled>
                <SearchControllersInnerBoxStyled>
                    <SearchItemBoxStyled>
                        <SubjectSelect />
                    </SearchItemBoxStyled>
                    <SearchItemBoxStyled>
                        <Specializations />
                    </SearchItemBoxStyled>
                    <SearchItemBoxStyled>
                        <PricePerHour />
                    </SearchItemBoxStyled>
                </SearchControllersInnerBoxStyled>
            </ContainerStyled>
        </SearchControllersBoxStyled>
    )
}

const SearchRouterLoading = ({ relativePath }: { relativePath: string }) => {
    const router = useRouter();
    const [showProgress, setShowProgress] = useState(false);
    const { locale } = useAppDataContext();

    useEffect(() => {
        router.events.on("routeChangeStart", (url) => {
            const urlString = url as string;
            if (urlString.startsWith(getRelativeUrlWithLoacle(locale, relativePath))) {
                setShowProgress(true);
            }
        })
        router.events.on("routeChangeComplete", () => {
            setShowProgress(false);
        })
    }, []);

    return (
        <ModalStyled open={showProgress}>
            <Box sx={{ display: "flex", width: "100%", height: "100%", justifyContent: "center", alignItems: "center" }}>
                <CircularProgressCustom color={COLORS.WHITE} size={40} />
            </Box>
        </ModalStyled>
    )
}

const SearchWithContexts = (props: PropsType) => (
    <SearchProvider {...props}>
        <SearchRouterLoading relativePath={props.relativePath} />
        <Box sx={{ display: "flex", flexFlow: "column noWrap", gap: { xs: "16px", md: "24px" } }}>
            <SearchHandlers query={props.query ?? {}} source={props.source} />
            <ContainerStyled><TutorItemList /></ContainerStyled>
        </Box>
    </SearchProvider>
)

export default SearchWithContexts;
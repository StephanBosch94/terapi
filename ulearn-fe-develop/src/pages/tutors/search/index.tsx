import React, { useContext } from "react";
import { UserSearchQueryParsed } from "core/services/Types/AppTypes";
import Footer from "components/landingPage/Footer";
import Copyright from "components/landingPage/Copyright";
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import Search from "components/search/Search";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { DefaultPagePropsType, fetchDefaultPageDataFromProps, getSsrCacheConfig } from "core/services/Utils/ssrUtil";
import { withAppProvider } from "components/core/providers/AppProvider";
import { GetServerSideProps } from 'next'
import Box from "@mui/material/Box";
import { FilterTutorsResponseBody } from "core/redux/userSlice";
import { removeUndefinedProperties } from "core/services/Utils/objectUtils";
import UserService from "core/services/UserService";
import { isFulfilled } from "core/services/Utils/apiUtil";
import { parseSearchQuery } from "routes/routesQuery";
import routes from "routes/routes";
import { UIContext } from "components/core/UIContextProvider";
import GoogleOneTap from "components/auth/GoogleOneTap";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";

type PropsType = {
    query: UserSearchQueryParsed,
    tutors: FilterTutorsResponseBody
};

type PageDataType = DefaultPagePropsType & PropsType;

export const getServerSideProps: GetServerSideProps<PageDataType> = async (props) => {
    try {
        props.res.setHeader('Cache-Control', getSsrCacheConfig())
        const query = parseSearchQuery(props.query);
        const tutorsPromise = UserService.filterTutors({
            filter: {
                specializationIds: query.specializationIds,
                toTrialPrice: query.toTrialPrice,
                subjectId: query.subjectId
            }
        });

        const staticDataPromise = fetchDefaultPageDataFromProps(props);
        const [tutorsRes, staticDataRes] = await Promise.allSettled([tutorsPromise, staticDataPromise]);

        const tutors = isFulfilled(tutorsRes) ? tutorsRes.value : undefined;
        const staticData = isFulfilled(staticDataRes) ? staticDataRes.value : undefined;
        if (!tutors) {
            throw new Error("Failed to fetch tutors");
        }
        if (!staticData) {
            throw new Error("Failed to fetch user statistics");
        }

        return {
            props: {
                ...staticData,
                tutors: tutors.data,
                query: removeUndefinedProperties(query)
            }
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
};

const SearchPage = ({ query, tutors }: PropsType) => {
    const { screenType } = useContext(UIContext);
    const isDesktop = screenType === "desktop";
    return (
        <>
            <GoogleOneTap role={UserRoleEnum.Student} autoRedirectToHomePage={false} />
            <Box>
                <Navbar position={isDesktop ? "static" : "sticky"} source={NavbarSourceEnum.studentPage} />
                <Box sx={{ padding: "24px 0 48px 0", backgroundColor: COLORS.BACKGROUND }}>
                    <Search source="search_page" query={query} initData={tutors} relativePath={routes.search.path} />
                </Box>
                <Footer bgColor={COLORS.WHITE} />
                <Copyright />
            </Box>
        </>
    )
}

export default withAppProvider<PageDataType>(SearchPage);
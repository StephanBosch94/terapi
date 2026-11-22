import UserService from "core/services/UserService";
import { GetServerSidePropsContext, GetServerSidePropsResult } from "next";
import { paths } from "apiTypes";
import { DefaultPagePropsType, fetchDefaultPageDataFromProps } from "core/services/Utils/ssrUtil";
import dynamic from "next/dynamic";
import UserStatisticsService from "core/services/UserStatisticsService";
import { isFulfilled } from "core/services/Utils/apiUtil";

type PageParams = {
    id: string[]
}

type UserInfoType = paths['/api/user']['get']['responses']['200']['content']['application/json'];
type UserStatisticsType = paths['/api/user-statistics/tutor/{userId}']['get']['responses']['200']['content']['application/json'];

type ContentPageProps = DefaultPagePropsType & {
    userInfo: UserInfoType,
    statistics: UserStatisticsType
}

const parseParams = (params: string[]) => {
    return parseInt(params[0]);
}

export const getServerSideProps = async (props: GetServerSidePropsContext<PageParams>): Promise<GetServerSidePropsResult<ContentPageProps>> => {
    try {
        // temporarily disable cache due to mobile device screen detection
        //res.setHeader('Cache-Control', getSsrCacheConfig())

        const userId = props.params?.id ? parseParams(props.params?.id) : undefined;
        if (!userId) {
            return { notFound: true }
        }
        const [userInfoResponseRes, statisticsRes, dataRes] = await Promise.allSettled([
            UserService.getPublicTutorInfoById(userId),
            UserStatisticsService.getTutorPublicStatistics(userId),
            fetchDefaultPageDataFromProps(props)
        ]);
        const userInfo = isFulfilled(userInfoResponseRes) ? userInfoResponseRes.value : undefined;
        const statistics = isFulfilled(statisticsRes) ? statisticsRes.value : undefined;
        const data = isFulfilled(dataRes) ? dataRes.value : undefined;

        if (!userInfo) {
            throw new Error("Failed to fetch user information");
        }
        if (!statistics) {
            throw new Error("Failed to fetch user statistics");
        }
        if (!data) {
            throw new Error("Failed to fetch static tables");
        }

        return {
            props: {
                ...data,
                statistics: statistics.data,
                userInfo: userInfo.data
            },
        }
    } catch (error) {
        console.error(error);
        return {
            notFound: true
        }
    }
}

const TutorPage = dynamic(() => import("components/pages/TutorPage"));
const TutorPageWithProps = (props: ContentPageProps) => (<TutorPage {...props} />)

export default TutorPageWithProps;
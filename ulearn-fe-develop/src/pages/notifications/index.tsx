import React, { useEffect } from "react";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import Navbar from "components/navbar/Navbar";
import Notifications from "components/navbar/Notifications";
import { selectNotificationsFilter, setNotificationFilter } from "core/redux/notificationSlice";
import { COMMON_CONSTANTS } from "core/constants/commonConstants";
import { AppendTypeEnum } from "core/constants/enums/AppendTypeEnum";

const NotificationsPage = () => {
    const dispatch = useAppDispatch();
    const notifications = useAppSelector(selectNotificationsFilter);

    useEffect(() => {
        if (!notifications) {
            dispatch(setNotificationFilter({
                appendType: AppendTypeEnum.None,
                request: {
                    skip: 0,
                    take: COMMON_CONSTANTS.DEFAULT_NUMBER_OF_ITEMS_TO_SHOW
                }
            }));
        }
    }, []);

    return (
        <>
            <Navbar />
            <Notifications />
        </>
    )
};

export default withAppClientSideProvider(NotificationsPage);
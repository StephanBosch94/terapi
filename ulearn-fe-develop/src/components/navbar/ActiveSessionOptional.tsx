import { useAppSelector } from "core/redux/reduxHooks";
import { selectShortUserInfo } from "core/redux/userSlice";
import ActiveSession from "./ActiveSession";

const ActiveSessionOptional = () => {
    const shortUserInfo = useAppSelector(selectShortUserInfo);

    return shortUserInfo?.runningActivity ? <ActiveSession runningActivity={shortUserInfo.runningActivity} /> : <></>
}

export default ActiveSessionOptional;
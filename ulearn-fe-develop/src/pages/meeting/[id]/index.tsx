import dynamic from "next/dynamic";

const MeetingAppContainer = dynamic(
    () => import("components/pages/MeetingPage"),
    {
        ssr: false
    }
);

export default MeetingAppContainer;
import Navbar, { NavbarSourceEnum } from "components/navbar/Navbar";
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { setRoomInfo, selectRoomInfo, selectRoomInfoStatus, selectRoomOpponent, RoomInfoResponse } from "core/redux/roomSlice";
import { useTranslation } from "components/core/TranslationProvider";
import React, { createContext, useCallback, useContext } from "react";
import { useEffect, useState, useRef } from "react";
import routes from "routes/routes";
import CircularProgress from "components/styles/CircularProgressStyled";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { COLORS, getColor } from "components/styles/constants/ColorsConstatnts";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import { getProfilePhotoUrl } from "core/services/Utils/imageUtil";
import { getVideoBlockHeight } from "core/services/Utils/mediaStreamUtil";
import { PrimaryButton } from "components/styles/PrimaryButton";
import VideoIcon from "assets/video-camera.svg";
import VideoIconSlash from "assets/video-camera-slash.svg"
import AudioIcon from "assets/microphone.svg";
import AudioIconSlash from "assets/microphone-slash.svg";
import SettingsIcon from "assets/gear.svg";
import PhoneSlashIcon from "assets/phone-slash.svg";
import CheckIcon from "assets/check.svg";
import ChatIcon from "assets/chat.svg";
import StarFilledIcon from "assets/star-filled.svg";
import StarIcon from "assets/star.svg";
import ScreencastIcon from "assets/screencast.svg";
import { useRouter } from 'next/router';
import { selectShortUserInfo } from "core/redux/userSlice";
import moment, { Moment } from "moment";
import { getFromToTime } from "core/services/Utils/timeUtil";
import { addActivity } from "core/redux/roomActivitySlice";
import { ActivityTypeEnum } from "core/constants/enums/ActivityTypeEnum";
import { AudioVolumeContextProvider, VideoControllersContextProvider, useAudioVolumeContext } from "components/videoCall/VideoControllersProvider";
import { useVideoControllersContext } from "components/videoCall/context/VideoControllersContext";
import { getUserName } from "core/services/Utils/userUtils";
import { UIContext } from "components/core/UIContextProvider";
import Chat from "components/chat/Chat";
import { selectSelectedConversation } from "core/redux/conversationSlice";
import { MenuStyled } from "components/styles/MenuStyled";
import { addStatistic } from "core/redux/userRoomStatisticSlice";
import { RoomStatisticTypeEnum } from "core/constants/enums/RoomStatisticTypeEnum";
import { SecondaryButton } from "components/styles/SecondaryButton";
import RoomSocketConnectorProvider, { useRoomSocketConnectorContext } from "components/core/RoomSocketConnectorProvider";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import Badge from "@mui/material/Badge";
import PStyled from "components/styles/typography/PStyled";
import Body1Styled from "components/styles/typography/Body1Styled";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import {
    MeetingProvider,
    useMeeting,
    useParticipant
} from "@videosdk.live/react-sdk";
import ConfettiBox from "components/styles/boxes/ConfettiBox";
import useRenderingTrace from "core/services/hooks/useRenderingTrace";
import { ContainerStyled } from "components/styles/ContainerStyled";

const VideoPreviewComponentSizes = {
    container: {
        gap: 24
    },
    centerBox: {
        padding: 24,
        gap: 24
    }
}

const CenterContainerBoxStyled = styled(Box)(() => sx({
    minHeight: 0,
    height: "100%",
    display: "flex",
    alignItems: "center",
    flexWrap: "wrap",
    justifyContent: "center"
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    display: "flex",
    flexFlow: "column nowrap",
    gap: `${VideoPreviewComponentSizes.container.gap}px`,
    alignItems: "center",
})) as typeof ContainerStyled;

const CenterBoxStyled = styled(Box)(() => sx({
    minHeight: "40vh",
    margin: "0 auto",
    gap: `${VideoPreviewComponentSizes.centerBox.gap}px`,
    padding: `${VideoPreviewComponentSizes.centerBox.padding}px`,
    width: { md: "60%", xs: "100%" },
    display: "flex",
    flexWrap: "wrap",
    justifyContent: "center",
    alignItems: "space-between",
    background: COLORS.WHITE,
    borderRadius: "8px",
    border: `1px solid ${COLORS.BLACK_10}`
})) as typeof Box;

const VideoBoxStyled = styled(Box)(() => sx({
    width: "100%",
    p: 0,
    display: "flex",
    alignContent: "center",
    "video": {
        width: "100%",
        maxHeight: "100%",
        borderRadius: "8px"
    },
    ".invisible": {
        "width": "0",
        "height": "0",
        // do not add display: "none", otherwise it might not work 
    }
})) as typeof Box;

const AvatarBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    justifyContent: "center",
    alignContent: "center",
    flexWrap: "wrap",
    gap: "24px"
})) as typeof Box;

const AvatarStyled = styled(Avatar)(() => sx({
    width: "120px",
    height: "120px"
})) as typeof Avatar;

const IconStyled = styled(Box)(() => sx({
    transform: "scale(1.5)",
    color: COLORS.BLACK,
    display: "flex",
    alignContent: "center",
    justifyContent: "center",
    ".MuiBadge-badge": {
        backgroundColor: COLORS.RED,
        color: COLORS.WHITE,
        fontSize: "8px",
        height: "16px",
        width: "16px",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        minWidth: 0
    }
})) as typeof Box;

const RedButtonBoxContainer = styled(Box)(() => sx({
    display: "flex",
    ".active": {
        svg: {
            color: COLORS.PRIMARY
        }
    },
    ".active:hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_10,
        border: `1px solid ${COLORS.PRIMARY}`
    },
    ".nonActive": {
        backgroundColor: getColor(COLORS.RED, 0.9),
        border: "none",
        svg: {
            color: `${COLORS.WHITE} !important`
        }
    },
    ".nonActive:hover": {
        cursor: "pointer",
        backgroundColor: COLORS.RED
    }
})) as typeof Box;

const PrimaryButtonBoxContainer = styled(Box)(() => sx({
    display: "flex",
    ".active": {
        backgroundColor: COLORS.PRIMARY,
        svg: {
            color: COLORS.WHITE
        }
    },
    ".active:hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_HOVER,
        color: COLORS.WHITE
    },
    ".nonActive": {
        svg: {
            color: COLORS.PRIMARY
        }
    },
    ".nonActive:hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_10,
        border: `1px solid ${COLORS.PRIMARY}`
    }
})) as typeof Box;

const ButtonBoxStyled = styled(Box)(() => sx({
    display: "flex",
    padding: "8px",
    width: "40px",
    height: "40px",
    alignItems: "center",
    justifyContent: "center",
    borderRadius: "50%",
})) as typeof Box;

const MenuBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
})) as typeof Box;

const MenuItemContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column noWrap",
    padding: "10px 12px",
    width: "100%",
    gap: "10px",
    ":hover": {
        "cursor": "pointer",
        "backgroundColor": COLORS.BLACK_10
    }
})) as typeof Box;

const MenuItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    padding: "10px 12px",
    ":hover": {
        "cursor": "pointer",
        "backgroundColor": COLORS.BLACK_10
    }
})) as typeof Box;

const MenuItemTitleBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "row noWrap",
    padding: "10px 12px"
})) as typeof Box;

const MenuItemBody1Styled = styled(Body1Styled)(() => sx({
    fontSize: "0.875rem",
    lineHeight: "20px",
    fontWeight: "400",
    color: COLORS.BLACK
})) as typeof Body1Styled;

const ICON_MARGIN_LEFT = 28;
const MenuTitleTypographyStyled = styled(CaptionTextStyled)(() => sx({
    fontWeight: "400",
    textTransform: "uppercase",
})) as typeof CaptionTextStyled;

const DeviderStyled = styled(Divider)(() => sx({
    my: "4px",
})) as typeof Divider;

const SelectedDeviceIconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    minWidth: `${ICON_MARGIN_LEFT}px`,
    fontWeight: "500",
    svg: {
        fill: COLORS.BLACK
    }
})) as typeof Box;

const PBlackStyled = styled(PStyled)(() => sx({
    color: COLORS.BLACK
})) as typeof PStyled;

const OpponentNamePStyled = styled(PStyled)(() => sx({
    color: COLORS.WHITE,
    fontWeight: { xs: "600", md: "500" },
    textAlign: "center"
})) as typeof PStyled;

const TimeBody1Styled = styled(Body1Styled)(() => sx({
    color: COLORS.WHITE,
    textAlign: "center"
})) as typeof Typography;

const CenterPStyled = styled(PStyled)(() => sx({
    color: COLORS.WHITE,
    width: "100%",
    textAlign: "center",
})) as typeof PStyled;

const VideoCallMainBoxStyled = styled(Box)(() => sx({
    position: "relative",
    display: "flex",
    width: "100%",
    height: "100%",
    justifyContent: "center",
    background: COLORS.BLACK,
    borderRadius: "8px",
    overflow: "hidden",
    "video": {
        width: "100%",
        height: "auto",
        borderRadius: "4px"
    },
    ".invisible": {
        "width": "0",
        "height": "0",
        // do not add display: "none", otherwise it might not work 
    }
})) as typeof Box;

const MobileVideoCallSecondaryBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    justifyContent: "flex-end",
    paddingRight: "16px",
    "video": {
        maxWidth: "150px",
        maxHeight: "160px",
        height: "auto",
        width: "auto",
        borderRadius: "8px"
    }
})) as typeof Box;

const VideoCallSecondaryBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    right: "24px",
    bottom: "24px",
    "video": {
        maxWidth: "320px",
        height: "auto",
        maxHeight: "320px",
        borderRadius: "8px"
    }
})) as typeof Box;

const CallViewBoxStyled = styled(Box)(() => sx({
    backgroundColor: COLORS.WHITE,
    display: "flex",
    height: "100%",
    width: "100%",
    flexFlow: "column noWrap",
    overflow: "auto",
    gap: { xs: "16px", md: "24px" },
    padding: { xs: "16px", md: "24px" }
})) as typeof Box;

const CallViewTopBoxStyled = styled(Box)(() => sx({
    position: "absolute",
    width: "100%",
    color: COLORS.WHITE,
    top: { xs: "16px", md: "32px" },
    display: "flex",
    flexFlow: "column noWrap",
    gap: "16px",
    textShadow: `1px 1px 2px ${COLORS.BLACK}`,
    alignItems: "center",
    justifyContent: "center"
})) as typeof Box;

const ReviewTextareaBoxStyled = styled(Box)(() => sx({
    width: { xs: "100%", md: "40%" },
    minHeight: "160px",
    padding: "8px",
    border: `1px solid ${COLORS.BLACK_10}`,
    fontWidth: "400",
    lineHeight: "20px",
    fontSize: "0.875rem",
    borderRadius: "4px",
    "textarea": {
        resize: "none",
        overflow: "hidden",
        border: "none",
        width: "100%",
        height: "100%",
        font: "inherit"
    },
    "textarea:focus-visible": {
        outline: "none"
    },
    ":textarea": {
        border: "none",
        color: COLORS.BLACK
    }
})) as typeof Box;

const ControllersButtonsBoxStyled = styled(Box)(() => sx({
    display: "flex",
    gap: "24px",
    width: "100%",
    alignItems: "center",
    justifyContent: "center",
    flex: "0 1 auto"
})) as typeof Box;

type MeetingStateType = "preview" | "joined" | "reviewing";

type MeetingPageContextType = {
    meetingState: MeetingStateType,
    join: () => void,
    leave: () => void
};

const MeetingPageContext = createContext<MeetingPageContextType>({
    meetingState: "preview",
    join: () => { },
    leave: () => { }
})


const MeetingPageContextProvider = ({ children, roomInfo }: { children: any, roomInfo: RoomInfoResponse }) => {
    const [meetingState, setMeetingState] = useState<MeetingStateType>("preview");

    useEffect(() => {
        if (roomInfo && roomInfo.isAlreadyJoined) {
            setMeetingState("joined"); // join if already joined
        }
    }, [roomInfo]);

    const handleJoinClick = () => {
        setMeetingState("joined");
    }

    const handleLeaveClick = () => {
        setMeetingState("reviewing");
    }

    return (
        <MeetingPageContext.Provider value={{
            meetingState,
            join: handleJoinClick,
            leave: handleLeaveClick
        }}>
            {children}
        </MeetingPageContext.Provider>
    )
};

const AudioDeviceSoundMeter = () => {
    const { audioVolume } = useAudioVolumeContext();
    const NUMBER_OF_ITEMS = 10;
    const ITEMS_GAP = 4;
    const offset = (ITEMS_GAP * (NUMBER_OF_ITEMS - 1) + ICON_MARGIN_LEFT) / NUMBER_OF_ITEMS;
    return (
        <Box sx={{ display: "flex", width: "100%", height: "12px", gap: `${ITEMS_GAP}px`, marginLeft: `${ICON_MARGIN_LEFT}px` }}>
            {Array(NUMBER_OF_ITEMS).fill(1).map((_, idx) => (
                <Box key={idx} sx={{ display: "flex", height: "100%", width: `calc(10% - ${offset}px)`, backgroundColor: ((idx + 1) <= audioVolume) ? COLORS.GREEN : getColor(COLORS.GREEN, 0.2) }} />
            ))}
        </Box>
    )
}

const AudioDeviceSoundMeterWithContext = () => {
    const { audioStream } = useVideoControllersContext();

    return (
        <AudioVolumeContextProvider stream={audioStream}>
            <AudioDeviceSoundMeter />
        </AudioVolumeContextProvider>
    )
}

const MediaDevicesSettings = () => {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const {
        audioDeviceInfo,
        videoDeviceInfo,
        setVideoDeviceId: handleSetVideoDeviceId,
        setAudioDeviceId: handleSetAudioDeviceId,
        noiceCancellationChange: handleNoiceCancellationChange,
        videoDevices,
        audioDevices
    } = useVideoControllersContext();
    const { t } = useTranslation();
    const open = Boolean(anchorEl);

    const handleOpenMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleCloseMenu = () => {
        setAnchorEl(null);
    };

    return (
        <React.Fragment>
            <RedButtonBox active={true} onClick={handleOpenMenu}>
                <IconStyled><SettingsIcon /></IconStyled>
            </RedButtonBox>
            <MenuStyled
                anchorEl={anchorEl}
                open={open}
                onClose={handleCloseMenu}
            >
                <MenuBoxStyled>
                    <MenuItemTitleBoxStyled>
                        <MenuTitleTypographyStyled>{t(TRANSLATION_KEYS.MEETING_ROOM.VIDEO_DEVICES)}</MenuTitleTypographyStyled>
                    </MenuItemTitleBoxStyled>

                    {videoDevices.map((videoDevice, idx) => (
                        <MenuItemBoxStyled key={videoDevice.deviceId} onClick={() => handleSetVideoDeviceId(videoDevice.deviceId, videoDevice.label)}>
                            <SelectedDeviceIconBoxStyled>{videoDevice.label === (videoDeviceInfo?.label ?? "") ? <CheckIcon /> : <></>}</SelectedDeviceIconBoxStyled>
                            <MenuItemBody1Styled>{videoDevice.label}</MenuItemBody1Styled>
                        </MenuItemBoxStyled>
                    ))}

                    <DeviderStyled />

                    <MenuItemTitleBoxStyled>
                        <MenuTitleTypographyStyled>{t(TRANSLATION_KEYS.MEETING_ROOM.AUDIO_DEVICES)}</MenuTitleTypographyStyled>
                    </MenuItemTitleBoxStyled>

                    {audioDevices.map((audioDevice, idx) => (
                        <MenuItemContainerBoxStyled key={audioDevice.deviceId} onClick={() => handleSetAudioDeviceId(audioDevice.deviceId, audioDevice.label)}>
                            <Box sx={{ display: "flex", width: "100%" }}>
                                <SelectedDeviceIconBoxStyled>{audioDevice.label === (audioDeviceInfo?.label ?? "") ? <CheckIcon /> : <></>}</SelectedDeviceIconBoxStyled>
                                <MenuItemBody1Styled>{audioDevice.label}</MenuItemBody1Styled>
                            </Box>
                            {audioDevice.label === (audioDeviceInfo?.label ?? "") ? <AudioDeviceSoundMeterWithContext /> : <></>}
                        </MenuItemContainerBoxStyled>
                    ))}

                    <DeviderStyled />

                    <MenuItemTitleBoxStyled>
                        <MenuTitleTypographyStyled>{t(TRANSLATION_KEYS.MEETING_ROOM.NOISE_CANCELLATION)}</MenuTitleTypographyStyled>
                    </MenuItemTitleBoxStyled>
                    {[true, false].map((boolValue, idx) => (
                        <MenuItemBoxStyled key={idx} onClick={() => handleNoiceCancellationChange(boolValue)}>
                            <SelectedDeviceIconBoxStyled>{(audioDeviceInfo?.echoCancellation ?? false) === boolValue ? <CheckIcon /> : <></>}</SelectedDeviceIconBoxStyled>
                            <MenuItemBody1Styled>{boolValue ? t(TRANSLATION_KEYS.MEETING_ROOM.NOISE_CANCELLATION_ON) : t(TRANSLATION_KEYS.MEETING_ROOM.NOISE_CANCELLATION_OFF)}</MenuItemBody1Styled>
                        </MenuItemBoxStyled>
                    ))}

                </MenuBoxStyled>
            </MenuStyled>
        </React.Fragment>
    )
}

const getTimeFromToTranslationKey = (startTime: Moment, currentTime: Moment) => {
    return currentTime.isAfter(startTime)
        ? TRANSLATION_KEYS.MEETING_ROOM.TIME_RUNNING
        : TRANSLATION_KEYS.MEETING_ROOM.TIME_UNTIL_MEETING;
}

const EndCallController = () => {
    const { leave: handleLeaveClick } = useContext(MeetingPageContext);
    const { hangUp: handleHangUp } = useVideoControllersContext();

    const handleClick = () => {
        handleHangUp();
        handleLeaveClick();
    }

    return (
        <RedButtonBox active={false} onClick={handleClick}>
            <IconStyled >
                <PhoneSlashIcon />
            </IconStyled>
        </RedButtonBox>
    )
}

const RedButtonBox = ({ active, children, onClick }: { active: boolean, onClick?: (event: React.MouseEvent<HTMLElement>) => void, children: any }) => {
    return (
        <RedButtonBoxContainer onClick={onClick}>
            <ButtonBoxStyled className={active ? "active" : "nonActive"}>
                {children}
            </ButtonBoxStyled>
        </RedButtonBoxContainer>
    )
}

const PrimaryButtonBox = ({ active, children, onClick }: { active: boolean, onClick?: (event: React.MouseEvent<HTMLElement>) => void, children: any }) => {
    return (
        <PrimaryButtonBoxContainer onClick={onClick}>
            <ButtonBoxStyled className={active ? "active" : "nonActive"}>
                {children}
            </ButtonBoxStyled>
        </PrimaryButtonBoxContainer>
    )
}

const VideoIconController = () => {
    const {
        videoClick: handleVideoClick,
        videoOn
    } = useVideoControllersContext();

    return (
        <RedButtonBox active={videoOn} onClick={handleVideoClick}>
            <IconStyled>
                {videoOn ? <VideoIcon /> : <VideoIconSlash />}
            </IconStyled>
        </RedButtonBox>
    )
}

const AudioIconController = () => {
    const {
        audioClick: handleAudioClick,
        audioOn
    } = useVideoControllersContext();

    return (
        <RedButtonBox active={audioOn} onClick={handleAudioClick}>
            <IconStyled>
                {audioOn ? <AudioIcon /> : <AudioIconSlash />}
            </IconStyled>
        </RedButtonBox>
    )
}

const ScreenSharingIconController = () => {
    const {
        displayClick: handleDisplayClick,
        displayOn
    } = useVideoControllersContext();

    return (
        <PrimaryButtonBox active={displayOn} onClick={handleDisplayClick}>
            <IconStyled><ScreencastIcon /></IconStyled>
        </PrimaryButtonBox>
    )
}

const MessagesIconController = () => {
    const {
        chatClick: handleChatClick
    } = useVideoControllersContext();
    const conversation = useAppSelector(selectSelectedConversation);

    return (
        <RedButtonBox active={true} onClick={handleChatClick}>
            <IconStyled>
                <Badge badgeContent={conversation?.countOfUnreadMessages ?? 0} >
                    <ChatIcon />
                </Badge>
            </IconStyled>
        </RedButtonBox>
    )
}

const VideoPreviewMediaControllers = () => {
    return (
        <Box sx={{ display: "flex", gap: "32px", width: "100%", alignItems: "center", justifyContent: "center" }}>
            <VideoIconController />
            <AudioIconController />
            <MediaDevicesSettings />
        </Box>
    )
}

const CallViewMediaControllers = () => {
    const { screenType } = useContext(UIContext);
    return (
        <ControllersButtonsBoxStyled>
            <VideoIconController />
            <AudioIconController />
            {screenType === "desktop" ? <ScreenSharingIconController /> : <></>}
            <MessagesIconController />
            <MediaDevicesSettings />
            <EndCallController />
        </ControllersButtonsBoxStyled>
    )
}

const OpponentInfo = () => {
    const [time, setTime] = useState(moment());
    const opponent = useAppSelector(selectRoomOpponent);
    const roomInfo = useAppSelector(selectRoomInfo);
    const { t } = useTranslation();

    useEffect(() => {
        // Infinity dependency loop
        const timer = setTimeout(() => {
            setTime(moment())
        }, 1000);

        return () => clearTimeout(timer);
    }, [time]);

    return (
        <Box sx={{ display: "flex", width: "100%", flexFlow: "column nowrap", gap: "8px" }}>
            <OpponentNamePStyled>{getUserName(opponent)}</OpponentNamePStyled>
            <TimeBody1Styled>{t(getTimeFromToTranslationKey(moment(roomInfo!.scheduledFromLocalTime), time), { time: getFromToTime(moment(roomInfo!.scheduledFromLocalTime), time) })}</TimeBody1Styled>
        </Box>
    )
}

const VideoCallCircularProgress = () => <CircularProgress size={30} color={COLORS.PRIMARY_10} />;

const CallWaitingForOpponentView = () => {
    const { t } = useTranslation();

    return (
        <AvatarBoxStyled>
            <CenterPStyled>{t(TRANSLATION_KEYS.MEETING_ROOM.WAITING_FOR_OPPONENT_TO_JOIN)}</CenterPStyled>
            <VideoCallCircularProgress />
        </AvatarBoxStyled>
    )
}

const MeetingJoinView = () => {
    const { t } = useTranslation();
    return (
        <AvatarBoxStyled>
            <CenterPStyled>{t(TRANSLATION_KEYS.MEETING_ROOM.JOINING_ROOM)}</CenterPStyled>
            <VideoCallCircularProgress />
        </AvatarBoxStyled>
    )
}

const CallConnectedView = ({ opponentId }: { opponentId: string }) => {
    const { micOn, micStream, webcamOn, webcamStream, screenShareOn, screenShareStream } = useParticipant(opponentId);
    const opponent = useAppSelector(selectRoomOpponent);

    const videoRef = useCallback((node: HTMLVideoElement | null) => {
        if (!node) return;
        if (webcamOn && webcamStream) {
            const stream = new MediaStream();
            stream.addTrack(webcamStream.track);
            node.srcObject = stream;
        }
        else {
            node.srcObject = null;
        }
    }, [webcamOn, webcamStream]);

    const displayRef = useCallback((node: HTMLVideoElement | null) => {
        if (!node) return;
        if (screenShareOn && screenShareStream) {
            const stream = new MediaStream();
            stream.addTrack(screenShareStream.track);
            node.srcObject = stream;
        } else {
            node.srcObject = null;
        }
    }, [screenShareOn, screenShareStream]);

    const audioRef = useCallback((node: HTMLVideoElement | null) => {
        if (!node) return;
        if (micOn && micStream) {
            const stream = new MediaStream();
            stream.addTrack(micStream.track);
            node.srcObject = stream;
        } else {
            node.srcObject = null;
        }
    }, [micOn, micStream]);

    return (
        <>
            <video playsInline autoPlay ref={videoRef} style={{ display: webcamOn ? "block" : "none", borderRadius: "4px" }} />
            <video playsInline autoPlay ref={displayRef} style={{ display: screenShareOn ? "block" : "none", borderRadius: "4px" }} />
            <audio autoPlay ref={audioRef} />
            {!webcamOn && !screenShareOn ? (
                <AvatarBoxStyled>
                    <AvatarStyled src={getProfilePhotoUrl(opponent)} />
                </AvatarBoxStyled>
            ) : <></>}
        </>
    )
}

const CurrenUserVideo = () => {
    const { videoStream } = useVideoControllersContext();
    const myVideoRefCallback = useCallback((node: HTMLVideoElement | null) => {
        if (node !== null && videoStream) {
            node.srcObject = videoStream;
        }
    }, [videoStream]);

    return (
        <video playsInline autoPlay muted ref={myVideoRefCallback} />
    )
}

const CurrenUserDisplay = () => {
    const { displayStream } = useVideoControllersContext();
    const myVideoRefCallback = useCallback((node: HTMLVideoElement | null) => {
        if (node !== null && displayStream) {
            node.srcObject = displayStream;
        }
    }, [displayStream]);

    return (
        <video playsInline autoPlay muted ref={myVideoRefCallback} />
    )
}

const CallErrorView = () => {

    return (
        <Box sx={{ height: "100%", display: "flex", alignItems: "center" }}>
            <ErrorLarge color={COLORS.WHITE} />
        </Box>
    )
}

const CallCenterViews = () => {
    const { opponentId, meetingState, errorCode } = useVideoControllersContext();
    const errorState = meetingState === "FAILED" || errorCode !== undefined;
    const connectingState = !errorState && meetingState === "CONNECTING";
    const waitingState = !errorState && !opponentId && meetingState !== "CONNECTING";

    return (
        <>
            {errorState ? <CallErrorView /> : <></>}
            {connectingState ? <MeetingJoinView /> : <></>}
            {waitingState ? <CallWaitingForOpponentView /> : <></>}
            {!errorState && opponentId && meetingState === "CONNECTED" ? <CallConnectedView opponentId={opponentId} /> : <></>}
        </>
    )
}

const CallInnerMobileView = ({ opponentId }: { opponentId: number }) => {
    const [mouseOn, setMouseOn] = useState(true);
    const { videoOn, displayOn, chatShowed } = useVideoControllersContext();

    const onMouseEnter = () => {
        setMouseOn(true);
    }

    const onMouseLeave = () => {
        setTimeout(() => {
            setMouseOn(false);
        }, 300);
    }

    return (
        <>
            {!chatShowed ? (
                <VideoCallMainBoxStyled
                    height="100%"
                    onMouseEnter={onMouseEnter}
                    onMouseLeave={onMouseLeave}
                    onTouchStart={onMouseEnter}
                    onTouchEnd={onMouseLeave}
                >
                    <CallViewTopBoxStyled>
                        {mouseOn ? <OpponentInfo /> : <></>}
                        {videoOn ? (
                            <MobileVideoCallSecondaryBoxStyled>
                                <CurrenUserVideo />
                            </MobileVideoCallSecondaryBoxStyled>
                        ) : <></>}
                        {displayOn ? (
                            <MobileVideoCallSecondaryBoxStyled>
                                <CurrenUserDisplay />
                            </MobileVideoCallSecondaryBoxStyled>
                        ) : <></>}
                    </CallViewTopBoxStyled>
                    <CallCenterViews />
                </VideoCallMainBoxStyled>
            ) : <Chat variant="embedded" opponentId={opponentId} />}
        </>
    )
}

const CallInnerDesktopView = ({ opponentId }: { opponentId: number }) => {
    const [mouseOn, setMouseOn] = useState(true);
    const { videoOn, displayOn, chatShowed } = useVideoControllersContext();

    const onMouseEnter = () => {
        setMouseOn(true);
    }

    const onMouseLeave = () => {
        setTimeout(() => {
            setMouseOn(false);
        }, 300);
    }

    return (
        <Box sx={{ display: "flex", flex: "1 1 auto", flexFlow: "row noWrap", overflow: "auto", gap: "24px" }}>
            <VideoCallMainBoxStyled
                onMouseEnter={onMouseEnter}
                onMouseLeave={onMouseLeave}
                onTouchStart={onMouseEnter}
                onTouchEnd={onMouseLeave}
            >
                <CallViewTopBoxStyled>
                    {mouseOn ? <OpponentInfo /> : <></>}
                </CallViewTopBoxStyled>
                <CallCenterViews />
                {videoOn ? (<VideoCallSecondaryBoxStyled><CurrenUserVideo /></VideoCallSecondaryBoxStyled>) : <></>}
                {displayOn ? (<VideoCallSecondaryBoxStyled><CurrenUserDisplay /></VideoCallSecondaryBoxStyled>) : <></>}
            </VideoCallMainBoxStyled>
            {chatShowed ? <Chat variant="embedded" opponentId={opponentId} /> : <></>}
        </Box>
    )
}

const CallView = ({ opponentId }: { opponentId: number }) => {
    const { screenType } = useContext(UIContext);

    return (
        <CallViewBoxStyled>
            {screenType === "mobile" ? <CallInnerMobileView opponentId={opponentId} /> : <></>}
            {screenType === "desktop" ? <CallInnerDesktopView opponentId={opponentId} /> : <></>}
            <CallViewMediaControllers />
        </CallViewBoxStyled>
    )
}

const VideoAudioPreView = () => {
    const { join: setJoiningView } = useContext(MeetingPageContext);
    const { videoStream, videoOn, join: videoCallJoin } = useVideoControllersContext();
    const [minHeight, setMinHeight] = useState(0);
    const [height, setHeight] = useState(0);
    const videoBoxRef = useRef<HTMLDivElement>(null);
    const videoControllersRef = useRef<HTMLDivElement>(null);
    const opponentInfoRef = useRef<HTMLDivElement>(null);
    const navbarBoxRef = useRef<HTMLTextAreaElement>(null);
    const userInfo = useAppSelector(selectShortUserInfo);
    const roomOpponent = useAppSelector(selectRoomOpponent);
    const { t } = useTranslation();
    const { screenType } = useContext(UIContext);

    const videoRefCallback = useCallback((node: HTMLVideoElement | null) => {
        if (node !== null && videoStream) {
            node.srcObject = videoStream;
        }
    }, [videoStream]);

    useEffect(() => {
        updateHeights();
    }, [videoBoxRef, videoControllersRef, opponentInfoRef, navbarBoxRef]);

    const updateHeights = () => {
        if (videoControllersRef.current && opponentInfoRef.current && videoBoxRef.current && navbarBoxRef.current) {
            const newHeight = videoControllersRef.current.scrollHeight +
                opponentInfoRef.current.scrollHeight +
                navbarBoxRef.current.scrollHeight +
                VideoPreviewComponentSizes.container.gap +
                VideoPreviewComponentSizes.centerBox.gap +
                VideoPreviewComponentSizes.centerBox.padding * 2;
            setHeight(newHeight);
            setMinHeight(getVideoBlockHeight(videoBoxRef));
        }
    }
    const handleJoinClick = () => {
        videoCallJoin();
        setJoiningView();
    }

    return (
        <React.Fragment>
            <Box ref={navbarBoxRef} sx={{ display: "flex", width: "100%", flexFlow: "row nowrap" }}>
                <Navbar source={NavbarSourceEnum.videoPage} />
            </Box>
            <CenterContainerBoxStyled>
                <ContainerCustomStyled>
                    <Box ref={opponentInfoRef} sx={{ display: "flex", flexFlow: "column nowrap", gap: "12px" }}>
                        <Box sx={{ display: "flex", width: "100%", justifyContent: "center", textAlign: "center" }}>
                            <PBlackStyled dangerouslySetInnerHTML={{ __html: (t(t(TRANSLATION_KEYS.MEETING_ROOM.CHOOSE_YOUR_VIDEO_AUDIO_SETTINGS))) }} />
                        </Box>
                        {screenType === "desktop" ? (
                            <Box sx={{ display: "flex", width: "100%", justifyContent: "center", alignItems: "center", gap: "8px" }}>
                                <Avatar sx={{ width: "40px", height: "40px" }} src={getProfilePhotoUrl(roomOpponent)} />
                                <PBlackStyled>{getUserName(roomOpponent)}</PBlackStyled>
                            </Box>
                        ) : <></>}
                    </Box>

                    <CenterBoxStyled>
                        <VideoBoxStyled ref={videoBoxRef} sx={{ minHeight: `${minHeight}px`, maxHeight: `calc(100vh - ${height}px)` }}>
                            <video playsInline autoPlay muted ref={videoRefCallback} className={!videoOn ? "invisible" : ""} />
                            {!videoOn
                                ? (
                                    <AvatarBoxStyled >
                                        <AvatarStyled src={getProfilePhotoUrl(userInfo)} />
                                    </AvatarBoxStyled>
                                ) : <></>}
                        </VideoBoxStyled>

                        <Box ref={videoControllersRef} sx={{ display: "flex", alignContent: "flex-end", flexWrap: "wrap", justifyContent: "center", gap: "24px" }}>
                            <VideoPreviewMediaControllers />
                            <PrimaryButton onClick={handleJoinClick}>{t(TRANSLATION_KEYS.MEETING_ROOM.JOIN_NOW)}</PrimaryButton>
                        </Box>
                    </CenterBoxStyled>
                </ContainerCustomStyled>
            </CenterContainerBoxStyled>
        </React.Fragment >
    )
}

const Confetti = () => {
    const [showConfetti, setShowConfetti] = useState(false);

    useEffect(() => {
        setShowConfetti(true);

        setTimeout(() => {
            setShowConfetti(false);
        }, 2500);
    }, []);

    return showConfetti ? <ConfettiBox gravity={0.4} /> : <></>
}

const CallRateView = ({ roomId }: { roomId: number }) => {
    const { t } = useTranslation();
    const dispatch = useAppDispatch();
    const router = useRouter();
    const [hoverRating, setHoverRating] = useState<number | undefined>(undefined);
    const [clickedRating, setClickedRating] = useState<number>(0);
    const [showTextarea, setShowTextarea] = useState(false);
    const [textReview, setTextReview] = useState<string | undefined>(undefined);

    const sendRequestAndRedirect = async (newRating: number) => {
        setClickedRating(newRating);
        await dispatch(addStatistic({
            roomId: roomId,
            roomStatisticTypeId: RoomStatisticTypeEnum.RoomQualityRating,
            textValue: textReview,
            value: newRating
        }))
        router.push(routes.home.path);
    }

    const handleMouseEnter = (newRating: number) => {
        setHoverRating(newRating);
    }

    const handleMouseLeave = () => {
        setHoverRating(clickedRating);
    }

    const handleStarClick = (newRating: number) => {
        if (newRating > 3) {
            sendRequestAndRedirect(newRating);
            return;
        }
        setHoverRating(newRating);
        setClickedRating(newRating);
        setShowTextarea(true);
    }

    const handleReviewTextChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const newVal = e.target.value;
        setTextReview(newVal);
    }

    const handleConfirmClick = () => {
        sendRequestAndRedirect(clickedRating);
    }

    const handleDismissClick = () => {
        router.push(routes.home.path);
    }

    return (
        <React.Fragment>
            <Navbar source={NavbarSourceEnum.videoPage} />
            <CenterContainerBoxStyled>
                <ContainerCustomStyled sx={{ gap: "20px" }}>
                    <Typography sx={{ fontWeight: "600", color: COLORS.BLACK, fontSize: "1.75rem", lineHeight: "32px", textAlign: "center" }}>{t(TRANSLATION_KEYS.MEETING_ROOM.HOW_WAS_THE_QUALITY)}</Typography>
                    <Box sx={{ display: "flex", gap: "8px", flexFlow: "row noWrap", justifyContent: "center", svg: { color: COLORS.PRIMARY, transform: "scale(1.75)" } }}>
                        {Array(5).fill(1).map((x, idx) => (
                            <Box key={idx} width="32px" height="32px" sx={{ ":hover": { cursor: "pointer" } }} onMouseEnter={() => handleMouseEnter(idx + 1)} onMouseLeave={handleMouseLeave} onClick={() => handleStarClick(idx + 1)}>
                                {hoverRating && (idx + 1 <= hoverRating) ? <StarFilledIcon /> : <StarIcon />}
                            </Box>
                        )
                        )}
                    </Box>
                    {
                        showTextarea ? (
                            <>
                                <ReviewTextareaBoxStyled>
                                    <textarea value={textReview} onChange={handleReviewTextChange} placeholder={t(TRANSLATION_KEYS.MEETING_ROOM.TELL_US_MORE)} />
                                </ReviewTextareaBoxStyled>
                                <Box sx={{ display: "flex", gap: "8px", width: "100%", justifyContent: "center" }}>
                                    <SecondaryButton onClick={handleDismissClick}>{t(TRANSLATION_KEYS.COMMON.DISMISS)}</SecondaryButton>
                                    <PrimaryButton onClick={handleConfirmClick}>{t(TRANSLATION_KEYS.COMMON.CONFIRM)}</PrimaryButton>
                                </Box>
                            </>
                        ) : <></>
                    }
                </ContainerCustomStyled>
            </CenterContainerBoxStyled>
            <Confetti />
        </React.Fragment>
    )
}

const VideoAudioPreViewLoadingView = () => {
    return (
        <React.Fragment>
            <Navbar source={NavbarSourceEnum.videoPage} />
            <CenterContainerBoxStyled>
                <ContainerCustomStyled>
                    <CenterBoxStyled sx={{ alignItems: "center" }}>
                        <CircularProgress size={30} />
                    </CenterBoxStyled>
                </ContainerCustomStyled>
            </CenterContainerBoxStyled>
        </React.Fragment>
    )
}

const VideoAudioPreViewErrorView = () => {
    return (
        <React.Fragment>
            <Navbar source={NavbarSourceEnum.videoPage} />
            <ContainerCustomStyled>
                <CenterBoxStyled sx={{ alignItems: "center" }}>
                    <ErrorLarge />
                </CenterBoxStyled>
            </ContainerCustomStyled>
        </React.Fragment>
    )
}

const MeetingPageLoaded = ({ roomInfo }: { roomInfo: RoomInfoResponse }) => {
    const { meetingState } = useContext(MeetingPageContext);
    const currentUserInfo = useAppSelector(selectShortUserInfo);
    const roomOpponent = roomInfo.host?.userId === currentUserInfo?.userId ? roomInfo.attender : roomInfo.host;
    const roomKey = roomInfo.roomKey ?? "";

    const { t } = useTranslation();

    useEffect(() => {
        if (!routes.meetingPage.titleKey) {
            return;
        }
        if (!currentUserInfo) {
            return;
        }

        document.title = t(routes.meetingPage.titleKey, { opponentName: getUserName(roomOpponent) })
    }, [roomInfo]);

    return (
        <>
            {meetingState === "preview" ? <VideoAudioPreView /> : <></>}
            {meetingState === "joined" && roomInfo ? <CallView opponentId={roomOpponent?.userId ?? 0} /> : <></>}
            {meetingState === "reviewing" && roomInfo ? <CallRateView roomId={roomInfo.roomId!} /> : <></>}
        </>
    )
}

const MeetingPageWithContexts = ({ roomInfo }: { roomInfo: RoomInfoResponse }) => {
    const currentUserInfo = useAppSelector(selectShortUserInfo);
    const roomKey = roomInfo.roomKey ?? "";

    return (
        <MeetingProvider
            config={{
                meetingId: roomKey,
                micEnabled: true,
                webcamEnabled: true,
                name: getUserName(currentUserInfo),
                participantId: `${currentUserInfo?.userId ?? 0}`,
                multiStream: false,
                mode: "CONFERENCE",
                maxResolution: "hd"
            }}
            token={roomInfo.accessToken ?? ""}
            joinWithoutUserInteraction={roomInfo.isAlreadyJoined ?? false}
        >
            <MeetingPageContextProvider roomInfo={roomInfo}>
                <VideoControllersContextProvider roomKey={roomKey}>
                    <MeetingPageLoaded roomInfo={roomInfo} />
                </VideoControllersContextProvider>
            </MeetingPageContextProvider>
        </MeetingProvider>
    )
}

const MeetingPage = ({ roomKey }: { roomKey: string }) => {
    const roomInfo = useAppSelector(selectRoomInfo);
    const roomInfoStatus = useAppSelector(selectRoomInfoStatus);
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setRoomInfo({ roomKey: roomKey }));
        dispatch(addActivity({ roomKey: roomKey, input: { activityTypeId: ActivityTypeEnum.Join } }));
    }, []);

    return (
        <>
            {roomInfoStatus === LoadingStatusEnum.Loading ? <VideoAudioPreViewLoadingView /> : <></>}
            {roomInfoStatus === LoadingStatusEnum.Error ? <VideoAudioPreViewErrorView /> : <></>}
            {roomInfoStatus === LoadingStatusEnum.Loaded && roomInfo ? <MeetingPageWithContexts roomInfo={roomInfo} /> : <></>}
        </>
    )
}

const MeetingPageRaw = () => {
    const { query } = useRouter();
    return (
        <>
            {query.id ? <MeetingPage roomKey={query.id as string} /> : <></>}
        </>
    )
}

export default withAppClientSideProvider(MeetingPageRaw, false);
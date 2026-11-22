import { useMeeting } from '@videosdk.live/react-sdk';
import { contextFactoryCreate, useAppDispatch, useAppSelector } from 'core/redux/reduxHooks';
import { getVolume, getAnalyser, createVideoStream, createAudioStream, stopTracks, getDevices, getDisplayStream } from 'core/services/Utils/mediaStreamUtil';
import moment from 'moment';
import React, { createContext, useState, useEffect, useRef } from 'react';
import VideoControllersContext, { AudioDeviceInfoType, MeetingStateType, VideoDeviceInfoType } from './context/VideoControllersContext';
import useRenderingTrace from 'core/services/hooks/useRenderingTrace';
import { Participant } from '@videosdk.live/react-sdk/dist/types/participant';
import { addActivity, selectLatestOpponentActivity } from 'core/redux/roomActivitySlice';
import { ActivityTypeEnum } from 'core/constants/enums/ActivityTypeEnum';

export enum ConnectionStateEnum {
    Connecting = 1,
    Connected = 2,
    Error = 3
}

type AudioVolumeContextType = {
    audioVolume: number
} | undefined;


const AudioVolumeContext = createContext<AudioVolumeContextType>(undefined);


const useAudioVolumeContext = contextFactoryCreate('AudioVolume', AudioVolumeContext);

const AudioVolumeContextProvider = ({ stream, children }: { stream: MediaStream | undefined, children: any }) => {
    const [audioVolume, setAudioVolume] = useState<number>(0);
    const [audioAnalyser, setAudioAnalyser] = useState<AnalyserNode | undefined>();
    const [time, setTime] = useState(moment());

    // Sync volume infinity loop
    useEffect(() => {
        const timer = setTimeout(() => {
            setTime(moment())
        }, 100);

        if (!audioAnalyser) return;
        const newVolumne = getVolume(audioAnalyser);

        setAudioVolume(newVolumne);
        return () => clearTimeout(timer);
    }, [time]);

    useEffect(() => {
        if (!stream) return;
        setAudioAnalyser(getAnalyser(stream));
    }, [stream]);

    // Sync audio analyzer on stream or audioDeviceInfo change 
    useEffect(() => {
        if (!stream) return;
        setAudioAnalyser(getAnalyser(stream));
    }, [stream]);

    return (
        <AudioVolumeContext.Provider value={{
            audioVolume
        }}
        >
            {children}
        </AudioVolumeContext.Provider>
    );
}

const VideoControllersContextProvider = ({ roomKey, children }: { roomKey: string, children: any }) => {
    const [videoDeviceInfo, setVideoDeviceInfo] = useState<VideoDeviceInfoType | undefined>();
    const [audioDeviceInfo, setAudioDeviceInfo] = useState<AudioDeviceInfoType | undefined>();
    const [videoDevices, setVideoDevices] = useState<MediaDeviceInfo[]>([]);
    const [audioDevices, setAudioDevices] = useState<MediaDeviceInfo[]>([]);
    const [chatShowed, setChatShowed] = useState(false);
    const [meetingState, setMeetingState] = useState<MeetingStateType>('CONNECTING');
    const [videoStream, setVideoStream] = useState<MediaStream | undefined>();
    const [audioStream, setAudioStream] = useState<MediaStream | undefined>();
    const [displayStream, setDisplayStream] = useState<MediaStream | undefined>();
    const [errorCode, setErrorCode] = useState<string | undefined>();
    const [opponentId, setOpponentId] = useState<string | undefined>();
    // const latestOpponentActivity = useAppSelector(selectLatestOpponentActivity);
    const isBlockedRef = useRef(false);
    const videoOn = videoStream !== undefined;
    const audioOn = audioStream !== undefined;
    const displayOn = displayStream !== undefined;
    const dispatch = useAppDispatch();

    const {
        enableWebcam,
        disableWebcam,
        muteMic,
        unmuteMic,
        join,
        leave,
        changeMic,
        changeWebcam,
        enableScreenShare,
        disableScreenShare
    } = useMeeting({
        onMeetingStateChanged: ({ state }: { state: MeetingStateType }) => setMeetingState(state),
        onError: ({ code, message }: { code: string; message: string }) => {
            console.error('Error code: ', code, ' message: ', message);
            setErrorCode(code);
        },
        onParticipantLeft: (participant: Participant) => {
            if (participant.local) return;
            setOpponentId(undefined);
        },
        onParticipantJoined: (participant: Participant) => {
            if (participant.local) return;
            setOpponentId(participant.id);
        }
    });

    // useEffect(() => {
    //     if (latestOpponentActivity?.activityTypeId === ActivityTypeEnum.Disconnected) {
    //         location.reload(); // workaround for connection issues
    //     }
    // }, [latestOpponentActivity]);

    useEffect(() => {
        upsertStreamsBlocking();

        return () => {
            if (videoStream) {
                clearVideo(videoStream);
            }
            if (audioStream) {
                clearAudio(audioStream);
            }
        }
    }, []);

    const criticalSectionCallback = async (f: () => Promise<void>) => {
        try {
            if (isBlockedRef.current) return;
            isBlockedRef.current = true;
            await f();
        } finally {
            isBlockedRef.current = false;
        }
    }

    const upsertVideoStream = async (info?: VideoDeviceInfoType | undefined, send: boolean = true) => {
        const res = await createVideoStream(info);
        const devices = await getDevices("videoinput");
        setVideoStream(res.stream);
        setVideoDeviceInfo(res.videoDevice);
        setVideoDevices(devices);

        try {
            if (send && res.stream) {
                enableWebcam(res.stream);
            }
        } catch (err) {
            setErrorCode(err as string);
        }

        return res;
    }

    const upsertDisplayStream = async () => {
        const displayStream = await getDisplayStream();
        const track = displayStream?.getVideoTracks()[0];
        if (track){
            track.onended = () => {
                clearDisplay(displayStream);
            }
        }

        setDisplayStream(displayStream);

        try {
            enableScreenShare(displayStream);
        } catch (err) {
            setErrorCode(err as string);
        }

        return displayStream;
    }

    const upsertAudioStream = async (info?: AudioDeviceInfoType | undefined, send: boolean = true) => {
        const res = await createAudioStream(info);
        const devices = await getDevices("audioinput");
        setAudioStream(res.stream);
        setAudioDeviceInfo(res.audioDevice);
        setAudioDevices(devices);

        try {
            if (send && res.stream) {
                unmuteMic(res.stream);
            }
        } catch (err) {
            setErrorCode(err as string);
        }

        return res;
    }

    const clearAudio = (stream: MediaStream) => {
        stopTracks(stream);
        setAudioStream(undefined);
        setAudioDevices([]);
        muteMic();
    }

    const clearVideo = (stream: MediaStream) => {
        stopTracks(stream);
        setVideoStream(undefined);
        setVideoDevices([]);
        disableWebcam();
    }

    const clearDisplay = (stream: MediaStream) => {
        stopTracks(stream);
        setDisplayStream(undefined);
        disableScreenShare();
    }

    const upsertStreamsBlocking = () => criticalSectionCallback(async () => {
        await upsertAudioStream(undefined, false); // 'false' used as a workaround 
        await upsertVideoStream(undefined, false); // 'false' used as a workaround 
    });

    const handleVideoClick = () => criticalSectionCallback(async () => {
        if (videoStream) {
            clearVideo(videoStream);
        }
        else {
            if (displayStream) {
                clearDisplay(displayStream);
            }
            await upsertVideoStream(videoDeviceInfo);
        }
    });

    const handleAudioClick = () => criticalSectionCallback(async () => {
        if (audioStream) {
            clearAudio(audioStream);
        }
        else {
            await upsertAudioStream(audioDeviceInfo);
        }
    });

    const handleDisplayClick = () => criticalSectionCallback(async () => {
        if (displayStream) {
            clearDisplay(displayStream);
        }
        else {
            if (videoStream) {
                clearVideo(videoStream);
            }
            await upsertDisplayStream();
        }
    });

    const handleSetVideoDeviceId = (deviceId: string, label: string) => criticalSectionCallback(async () => {
        if ((videoDeviceInfo?.label ?? "") === label) return;

        const newValue = { deviceId, label };
        if (videoStream) {
            stopTracks(videoStream);
            const res = await upsertVideoStream(newValue, false);
            if (res.stream) {
                changeWebcam(res.stream);
            }
        }
        else {
            setVideoDeviceInfo(newValue);
        }
    });

    const handleSetAudioDeviceId = (deviceId: string, label: string) => criticalSectionCallback(async () => {
        if ((audioDeviceInfo?.label ?? "") === label) return;

        const newValue = { deviceId, label, echoCancellation: audioDeviceInfo?.echoCancellation ?? true };
        if (audioStream) {
            stopTracks(audioStream);
            const res = await upsertAudioStream(newValue, false);
            if (res.stream) {
                changeMic(res.stream);
            }
        }
        else {
            setAudioDeviceInfo(newValue);
        }
    });

    const handleNoiceCancellationChange = (enabled: boolean) => criticalSectionCallback(async () => {
        if (audioDeviceInfo?.echoCancellation === enabled) return;

        const newValue = { deviceId: audioDeviceInfo?.deviceId, label: audioDeviceInfo?.label ?? "", echoCancellation: enabled };
        if (audioStream) {
            stopTracks(audioStream);
            const res = await upsertAudioStream(newValue, false);
            if (res.stream) {
                changeMic(res.stream);
            }
        }
        else {
            setAudioDeviceInfo(newValue);
        }
    });

    const handleChatClick = () => {
        setChatShowed(!chatShowed);
    }

    const handleHangUp = () => {
        if (videoStream) {
            clearVideo(videoStream);
        }
        if (audioStream) {
            clearAudio(audioStream);
        }
        if (displayStream) {
            clearDisplay(displayStream);
        }
        try {
            leave();
        } catch (e) {
            console.error(e);
        }
    }

    const handleJoin = () => {
        dispatch(addActivity({ roomKey: roomKey, input: { activityTypeId: ActivityTypeEnum.Join } }));
        join();
    }

    return (
        <VideoControllersContext.Provider value={{
            videoDeviceInfo,
            audioDeviceInfo,
            meetingState,
            videoStream,
            audioStream,
            displayStream,
            videoDevices,
            audioDevices,
            chatShowed,
            audioOn,
            videoOn,
            displayOn,
            opponentId,
            errorCode,
            videoClick: handleVideoClick,
            audioClick: handleAudioClick,
            displayClick: handleDisplayClick,
            setVideoDeviceId: handleSetVideoDeviceId,
            setAudioDeviceId: handleSetAudioDeviceId,
            chatClick: handleChatClick,
            hangUp: handleHangUp,
            noiceCancellationChange: handleNoiceCancellationChange,
            join: handleJoin
        }}
        >
            {children}
        </VideoControllersContext.Provider>
    );
}

export { VideoControllersContextProvider, AudioVolumeContextProvider, useAudioVolumeContext };
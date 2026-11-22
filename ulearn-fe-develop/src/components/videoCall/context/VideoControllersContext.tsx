import { createContext } from "react";
import { contextFactoryCreate } from "core/redux/reduxHooks";


export type MeetingStateType = 'CONNECTING' | 'CONNECTED' | 'FAILED' | 'DISCONNECTED' | 'CLOSING' | 'CLOSED';
export type AudioDeviceInfoType = {
    deviceId: string | undefined,
    label: string,
    echoCancellation: boolean
};
export type VideoDeviceInfoType = {
    deviceId: string | undefined;
    label: string;
};
type VideoControllersContextType = {
    videoDevices: MediaDeviceInfo[],
    audioDevices: MediaDeviceInfo[],
    audioDeviceInfo: AudioDeviceInfoType | undefined,
    videoDeviceInfo: VideoDeviceInfoType | undefined,
    meetingState: MeetingStateType,
    videoStream: MediaStream | undefined,
    audioStream: MediaStream | undefined,
    displayStream: MediaStream | undefined,
    chatShowed: boolean,
    audioOn: boolean,
    videoOn: boolean,
    displayOn: boolean,
    opponentId: string | undefined,
    errorCode: string | undefined,
    videoClick: () => void,
    audioClick: () => void,
    displayClick: () => void,
    setVideoDeviceId: (deviceId: string, label: string) => void,
    setAudioDeviceId: (deviceId: string, label: string) => void,
    noiceCancellationChange: (enabled: boolean) => void,
    chatClick: () => void,
    join: () => void,
    hangUp: () => void,
} | undefined;

const VideoControllersContext = createContext<VideoControllersContextType>(undefined);
export const useVideoControllersContext = contextFactoryCreate('VideoControllers', VideoControllersContext);

export default VideoControllersContext;
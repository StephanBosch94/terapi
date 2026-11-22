import { AudioDeviceInfoType, VideoDeviceInfoType } from "components/videoCall/context/VideoControllersContext";

export type DeviceInputElement = {
    deviceId: string | undefined,
    deviceLabel: string
}

export type DeviceType = "videoinput" | "audioinput";
export type ActionType = "audioOn" | "audioOff" | "videoOn" | "videoOff" | "none";

export const getDevices = async (type: DeviceType) => {
    const devices = await navigator.mediaDevices.enumerateDevices();
    return devices.filter(x => x.kind === type);
}

export const getAvailableDevices = async (type: DeviceType) => {
    const videoDevices = await getDevices(type);
    return videoDevices.some(x => x.label.length > 0);
}

export const isVideoAvailable = () => getAvailableDevices("videoinput");
export const isAudioAvailable = () => getAvailableDevices("audioinput");

export const getDevice = async (deviceId: string) => {
    const devices = await navigator.mediaDevices.enumerateDevices();
    return devices.find(x => x.deviceId === deviceId);
}

export const getTrack = (stream: MediaStream, deviceLabel: string) => {
    return stream.getTracks().find(x => x.label === deviceLabel);
}

export const getLiveAudioTrack = (stream: MediaStream) => {
    return stream.getAudioTracks()
        .find(x => x.readyState === "live");
}

export const getLiveVideoTrack = (stream: MediaStream) => {
    return stream.getVideoTracks()
        .find(x => x.readyState === "live");
}

export const isTrackLive = (track: MediaStreamTrack | undefined) => {
    if (!track) return false;
    return track.readyState === "live";
}

export const isVideoLive = (stream: MediaStream | null) => {
    if (!stream) return false;
    const videoTrack = getLiveVideoTrack(stream);
    return videoTrack !== undefined;
}

export const isLive = (stream: MediaStream | undefined, device: DeviceInputElement | undefined) => {
    if (!device) return false;
    if (!stream) return false;
    return isTrackLive(stream.getTracks().find(x => x.label === device.deviceLabel));
}


export const stopTracks = (stream: MediaStream | null) => {
    if (!stream) return;
    stream.getTracks()
        .filter(x => x.kind === "video" || x.kind === "audio")
        .filter(x => x.readyState === "live")
        .forEach(x => { x.stop(); });
}

export const getVideoStream = async (info: VideoDeviceInfoType | undefined) => {
    try {
        return await navigator.mediaDevices.getUserMedia({
            video: {
                deviceId: info?.deviceId
            }
        });
    } catch (e) {
        console.error('Failed to get video stream: ', e);
        return undefined;
    }
}

export const getDisplayStream = async () => {
    try {
        return await navigator.mediaDevices.getDisplayMedia({
            video: true,
            audio: false            
        });
    } catch (e) {
        console.error('Failed to get display stream: ', e);
        return undefined;
    }
}

export const getAudioStream = async (info: AudioDeviceInfoType | undefined) => {
    try {
        return await navigator.mediaDevices.getUserMedia({ audio: { echoCancellation: info?.echoCancellation ?? true, deviceId: info?.deviceId } });
    } catch (e) {
        console.error('Failed to get audio stream');
        return undefined;
    }
}

export const mergeStreams = (videoStream: MediaStream | null, audioStream: MediaStream | null) => {
    if (!videoStream && !audioStream) return null;
    if (!videoStream) return audioStream;
    if (!audioStream) return videoStream;
    const audioTrack = audioStream.getAudioTracks()[0];
    audioStream.removeTrack(audioTrack);
    videoStream.addTrack(audioTrack);
    return videoStream;
}

export const getVideoDeviceInfo = (stream: MediaStream | undefined): VideoDeviceInfoType | undefined => {
    if (!stream) return undefined;
    const videoTracks = stream.getVideoTracks();
    const track = videoTracks[0];
    const settings = track?.getSettings();

    return videoTracks.length > 0
        ? { deviceId: settings.deviceId, label: track.label }
        : undefined;
}

export const getAudioDeviceInfo = (stream: MediaStream | undefined): AudioDeviceInfoType | undefined => {
    if (!stream) return undefined;
    const tracks = stream.getAudioTracks();
    const track = tracks[0];
    const settings = track?.getSettings();

    return tracks.length > 0
        ? { deviceId: settings?.deviceId, label: track.label, echoCancellation: settings.echoCancellation ?? false }
        : undefined;
}

// Get the volume of the audio
export const getVolume = (analyser: AnalyserNode): number => {
    try {
        const min = 0;
        const max = Math.log2(255);
        const dataArray = new Uint8Array(analyser.frequencyBinCount);
        analyser.getByteFrequencyData(dataArray);

        let sum = 0;
        for (let i = 0; i < dataArray.length; i++) {
            sum += dataArray[i];
        }

        const avg = sum / dataArray.length;
        const posLogAvg = avg >= 1 ? Math.log2(avg) : avg;
        const normalized = (posLogAvg - min) / (max - min);
        return Math.round(normalized * 10);
    } catch (err) {
        console.error(err);
        return 0;
    }
}

export const getAnalyser = (stream: MediaStream) => {
    try {
        const audioContext = new AudioContext();
        const analyser = audioContext.createAnalyser();
        audioContext.createMediaStreamSource(stream).connect(analyser);
        return analyser;
    } catch (err) {
        console.error(err);
        return undefined;
    }
}

export const createAudioStream = async (info: AudioDeviceInfoType | undefined): Promise<{ stream: MediaStream | undefined, audioDevice: AudioDeviceInfoType | undefined }> => {
    const stream = await getAudioStream(info);

    return {
        stream: stream,
        audioDevice: getAudioDeviceInfo(stream)
    }
}

export const createVideoStream = async (info: VideoDeviceInfoType | undefined): Promise<{ stream: MediaStream | undefined, videoDevice: VideoDeviceInfoType | undefined }> => {
    const stream = await getVideoStream(info);

    return {
        stream: stream,
        videoDevice: getVideoDeviceInfo(stream)
    }
}

export const getVideoTrack = async (device: DeviceInputElement | undefined) => {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({
            video: device !== undefined && device.deviceId !== undefined && device.deviceId.length > 0
                ? {
                    deviceId: device.deviceId,
                }
                : true
        });
        const track = stream.getVideoTracks()[0];
        stream.removeTrack(track);
        return track;
    }
    catch (e) {
        console.error(e);
        return undefined;
    }
}


export const getLiveTracks = (stream: MediaStream) => {
    return stream.getTracks().filter(track => track.readyState === "live");
}

export const removeTrackFromStream = (track: MediaStreamTrack, stream: MediaStream) => {
    track.enabled = false;
    track.stop();
    stream.removeTrack(track);
}


export const removeTrack = (stream: MediaStream, track: MediaStreamTrack) => {
    removeTrackFromStream(track, stream);
}

export const getVideoBlockHeight = (videoDivRef: React.RefObject<HTMLDivElement>) => {
    // Video camera has 4 * 3 format (TODO check on other cameras)
    return ((videoDivRef.current?.scrollWidth ?? 0) * 3 / 4) + 1;
}

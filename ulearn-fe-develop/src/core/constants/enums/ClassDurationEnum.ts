export enum ClassDurationEnum {
    Half = 1,
    Full = 2
}

export const getDurationInMinutes = (duration: ClassDurationEnum) => {
    return duration === ClassDurationEnum.Full ? 50 : 25;
} 
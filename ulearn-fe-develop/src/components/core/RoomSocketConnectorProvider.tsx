import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { LoadingStatusEnum } from 'core/constants/enums/LoadingStatusEnum';
import { contextFactoryCreate, useAppDispatch } from 'core/redux/reduxHooks';
import { setOpponentActivity } from 'core/redux/roomActivitySlice';
import { setOpponentJoinInfo } from 'core/redux/userSlice';
import { getWebApiHubUrl } from "core/services/Core/CoreApiService";
import { OpponentActivityMessageVM, TokenVM } from 'core/services/Types/AppTypes';
import { customSleep } from 'core/services/Utils/apiUtil';
import { createContext, useEffect, useRef, useState } from "react";

const SignalMethods = {
  NewMessage: 'newMessage',
  NewConversation: 'newConversation',
  RefreshCountOfUnreadMessages: 'refreshCountOfUnreadMessages',
  RoomOpponentActivity: "roomOpponentActivity",
  RoomStateChange: "roomStateChange"
};

type RoomSocketConnectorType = {
  sendSignal: (data: string) => void,
  connectionStatus: LoadingStatusEnum | undefined
} | undefined;

export const RoomSocketConnectorContext = createContext<RoomSocketConnectorType>(undefined);
export const useRoomSocketConnectorContext = contextFactoryCreate('RoomSocketConnector', RoomSocketConnectorContext);

const RoomSocketConnectorProvider = ({ roomKey, children }: { roomKey: string, children: any }) => {
  const dispatch = useAppDispatch();
  const connection = useRef<HubConnection | undefined>();
  const [connectionStatus, setConnectionStatus] = useState<LoadingStatusEnum | undefined>(undefined);

  useEffect(() => {
    const recursiveCallback = () => {
      startConnection()
        .then(() => { })
        .catch((err) => {
          console.error(err);
          setConnectionStatus(LoadingStatusEnum.Error);
          recursiveCallback();
          customSleep(1000);
        })
    }

    recursiveCallback();
    return () => {
      stopConnection();
    }
  }, [connection]);

  const stopConnection = async () => {
    if (connection.current) {
      await connection.current.stop();
    }
  };

  const onNewRoomOpponentActivity = (args: any) => {
    const opponentActivity = args as OpponentActivityMessageVM;
    dispatch(setOpponentActivity(opponentActivity));
    dispatch(setOpponentJoinInfo(opponentActivity));
  }

  const startConnection = async () => {
    if (connection.current) return;
    setConnectionStatus(LoadingStatusEnum.Loading);
    const url = await getWebApiHubUrl("hub/roomHub", { roomKey });
    const newConnection = new signalR.HubConnectionBuilder().withUrl(url).build();
    newConnection.on(SignalMethods.RoomOpponentActivity, onNewRoomOpponentActivity);
    newConnection.onclose(() => {
      connection.current = undefined;
      setConnectionStatus(LoadingStatusEnum.Error);
    })
    await newConnection.start();
    connection.current = newConnection;
    setConnectionStatus(LoadingStatusEnum.Loaded);
  };

  const sendSignal = (data: string) => {
    if (!connection.current) {
      console.error('Cannot send signal when socket is not initialized');
      return;
    }
    connection.current.invoke("SentSignalToOpponent", data);
  }

  return (
    <RoomSocketConnectorContext.Provider value={{
      sendSignal,
      connectionStatus
    }}>
      {children}
    </RoomSocketConnectorContext.Provider>
  )
}

export default RoomSocketConnectorProvider;
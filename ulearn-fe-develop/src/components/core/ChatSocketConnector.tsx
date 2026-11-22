import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { paths } from 'apiTypes';
import { selectIsAuthorized, selectUserId } from 'core/redux/authSlice';
import { addLatestMessage, addConversation, ConversationVM } from 'core/redux/conversationSlice';
import { addMessage } from 'core/redux/messageSlice';
import { addNotification } from 'core/redux/notificationSlice';
import { useAppDispatch, useAppSelector } from 'core/redux/reduxHooks';
import { updateUserShortInfoStatistics } from 'core/redux/userSlice';
import { getWebApiHubUrl } from "core/services/Core/CoreApiService";
import { AppMessageVM } from 'core/services/Types/AppTypes';
import { useRouter } from 'next/router';
import { useEffect } from "react";

const SignalMethods = {
  NewMessage: 'newMessage',
  NewConversation: 'newConversation',
  NewNotification: 'newNotification',
  RefreshCountOfUnreadMessages: 'refreshCountOfUnreadMessages',
  RoomOpponentActivity: "roomOpponentActivity",
  RoomStateChange: "roomStateChange"
};

const ChatSocketConnector = ({ silent }: { silent: boolean }) => {
  const dispatch = useAppDispatch();
  const isUserAuthorized = useAppSelector(selectIsAuthorized);
  const userId = useAppSelector(selectUserId);
  const router = useRouter();
  
  let connection: HubConnection | null = null;

  const stopConnection = async () => {
    if (connection) {
      await connection.stop();
      connection = null;
    }
  };

  const playSound = () => {
    if (silent) return;
    const audio = new Audio('https://static.videosdk.live/prebuilt/notification.mp3');
    audio.volume = 0.25;
    audio.play();
  }

  const onNewMessageReceived = (args: any, userId: number) => {
    const message = args as AppMessageVM;
    dispatch(updateUserShortInfoStatistics());
    dispatch(addLatestMessage({ message: message, currentUserId: userId }));
    dispatch(addMessage(message));
    if (message.senderId !== userId){
      playSound();
    }
  }

  const onNewConversationReceived = (args: any) => {
    dispatch(addConversation(args as ConversationVM));
  }

  const onRefreshCountOfUnreadMessagesReceived = () => {
    dispatch(updateUserShortInfoStatistics());
  }

  const onNewNotificationReceived = (args: any, userId: number) => {
    const notification = args as NonNullable<paths['/api/notification/filter']['post']['responses']['200']['content']['application/json']['data'][0]>;
    dispatch(addNotification(notification));
    dispatch(updateUserShortInfoStatistics());

    if (notification.sender && notification.sender.userId !== userId){
      playSound();
    }
  }

  const startConnection = async (userId: number, path: string) => {
    const url = await getWebApiHubUrl("hub/chatHub", { url: path });

    try {
      connection = new signalR.HubConnectionBuilder().withUrl(url).build();

      connection.on(SignalMethods.NewMessage, (args) => onNewMessageReceived(args, userId));
      connection.on(SignalMethods.NewNotification, (args) => onNewNotificationReceived(args, userId));
      connection.on(SignalMethods.NewConversation, onNewConversationReceived);
      connection.on(SignalMethods.RefreshCountOfUnreadMessages, onRefreshCountOfUnreadMessagesReceived);
      
      await connection.start();
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (!router.isReady) return;
    if (!isUserAuthorized) return;
    if (!userId) return;

    startConnection(userId, router.asPath);

    return () => {
      stopConnection();
    }
  }, [isUserAuthorized, router.isReady])

  return (
    <></>
  )
}

export default ChatSocketConnector;


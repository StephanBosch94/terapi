import React, { createContext, useContext, useEffect, useRef, useState } from "react";
import { ConversationVM, GetConversationsResponse, selectOpponent, selectSelectedConversation, setRead, setSelectedConversation } from 'core/redux/conversationSlice';
import { styled, experimental_sx as sx } from "@mui/material/styles";
import { useAppDispatch, useAppSelector } from "core/redux/reduxHooks";
import { selectConversations, selectConversationsStatus, setConversations } from "core/redux/conversationSlice";
import { selectMessages, setMessagesFilter, selectHasMoreMessages, createMessageWithAttachment } from "core/redux/messageSlice";
import { getProfilePhotoThumbnailUrl } from "core/services/Utils/imageUtil";
import FileService from "core/services/FileService";
import { getShortName, getUserName } from "core/services/Utils/userUtils";
import Moment from 'react-moment';
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import { useTranslation } from "components/core/TranslationProvider";
import { TRANSLATION_KEYS } from "core/constants/translationConstants";
import { calculateHeight, getGrouppedMessagesByDay, getGrouppedMessagesByUserAndTime, hasUnreadMessages } from "core/services/Utils/messageUtil";
import { getShortenText } from "core/services/Utils/textUtil";
import InfiniteScroll from "react-infinite-scroll-component";
import moment from "moment";
import data from '@emoji-mart/data'
import Picker from '@emoji-mart/react'
import produce from "immer";
import { AppMessageAttachmentVM, AppMessageVM } from "core/services/Types/AppTypes";
import SendIcon from "assets/send.svg";
import AttachmentIcon from "assets/attachment.svg";
import EmojiIcon from "assets/emoji.svg";
import FileIcon from "assets/file-text.svg";
import CloseIcon from "assets/x.svg";
import FilledCircleIcon from "assets/circle-fill.svg";
import BackIcon from "assets/arrow-left.svg"
import { selectShortUserInfo } from "core/redux/userSlice";
import { LoadingStatusEnum } from "core/constants/enums/LoadingStatusEnum";
import ErrorLarge from "components/core/error/ErrorLarge";
import { UIContext } from "components/core/UIContextProvider";
import Navbar from "components/navbar/Navbar";
import { useVideoControllersContext } from "components/videoCall/context/VideoControllersContext";
import CircularProgress from "components/styles/CircularProgressStyled";
import Menu from "components/menu/Menu";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Avatar from "@mui/material/Avatar";
import Skeleton from "@mui/material/Skeleton";
import Link from "next/link";
import EmptyMessagesIcon from "assets/empty_messages.svg";
import { UserRoleEnum } from "core/constants/enums/UserRoleEnum";
import PStyled from "components/styles/typography/PStyled";
import H3Styled from "components/styles/typography/H3Styled";
import CircularProgressCustom from "components/styles/CircularProgressStyled";
import { ContainerStyled } from "components/styles/ContainerStyled";
import { useRouter } from "next/router";

const DEFAULT_MESSAGE_TAKE = 50;
const MAX_ATTACHMENT_NAME_LEN = 32;

const InputBoxSizes = {
    padding: 16,
    marginTop: 16,
    gap: 16,
    border: 1,
    buttonBox: {
        padding: 4
    },
    textareaBox: {
        padding: 10,
        border: 1,
        textarea: {
            lineHeight: 20
        },
    },
    attachmentsContainer: {
        gap: 8,
        itemBox: {
            height: 32
        }
    }
}

const ConverstionalBoxStyled = styled(Box)(() => sx({
    ".active": {
        backgroundColor: COLORS.PRIMARY_10,
        borderRadius: "4px"
    }
})) as typeof Box;

const UserNameTypographyStyled = styled(Typography)(() => sx({
    fontSize: "0.875rem",
    fontWeight: 600,
    lineHeight: "16px"
})) as typeof Typography;

const TimeTypographyStyled = styled(Typography)(() => sx({
    fontSize: "0.625rem",
    fontWeight: 400,
    lineHeight: "16px",
    color: COLORS.BLACK_60
})) as typeof Typography;

const MessageTypographyStyled = styled(Typography)(() => sx({
    fontSize: "0.875rem",
    fontWeight: 400,
    lineHeight: "20px",
    whiteSpace: "pre-line"
})) as typeof Typography;

const DownloadableAttachmentTypographyStyled = styled(Typography)(() => sx({
    fontSize: "0.75rem",
    fontWeight: 400,
    lineHeight: "20px",
    textDecoration: "none"
})) as typeof Typography;

const ConversationBoxStyled = styled(Box)(() => sx({
    marginBottom: "4px",
    padding: "8px",
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.BLACK_10,
        borderRadius: "4px"
    },
    ".active": {
        backgroundColor: COLORS.PRIMARY_10,
        borderRadius: "4px"
    }
})) as typeof Box;

const AvatarStyled = styled(Avatar)(() => sx({
    width: "40px",
    height: "40px"
})) as typeof Avatar;

const MessagesBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexWrap: "wrap",
    width: "100%",
    rowGap: "48px",
    overflow: "auto",
    flexDirection: 'column-reverse',
    padding: "0px 8px 0px 16px",
})) as typeof Box;

const AttachmentItemBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    textAlign: "center",
    height: "100%",
})) as typeof Box;

const AttachmentIconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: COLORS.PRIMARY,
    color: COLORS.WHITE,
    borderRadius: "4px",
    padding: "8px",
    "svg": {
        transform: "scale(1.25)"
    },
    "svg path": {
        stroke: COLORS.WHITE
    }
})) as typeof Box;

const MessageTextInputBoxStyled = styled(Box)(() => sx({
    position: "relative",
    width: "100%",
    marginTop: `${InputBoxSizes.marginTop}px`,
    padding: `${InputBoxSizes.padding}px`,
    display: "flex",
    flex: "0 1 auto",
    flexFlow: "column nowrap",
    gap: `${InputBoxSizes.gap}px`,
    borderTop: `${InputBoxSizes.border}px solid ${COLORS.BLACK_10}`,
    "textarea": {
        width: "100%",
        border: "none",
        padding: 0,
        margin: "auto",
        resize: "none",
        font: "inherit",
        fontSize: "0.875rem",
        lineHeight: `${InputBoxSizes.textareaBox.textarea.lineHeight}px`,
        overflow: "hidden",
        display: "flex",
        justifyContent: "center",
    },
    "textarea:focus-visible": {
        outline: "none"
    },
    ":textarea": {
        border: "none",
        color: COLORS.PRIMARY
    }
})) as typeof Box;

const MessageItemBoxStyled = styled(Box)(() => sx({
    width: "100%",
    fontSize: "0.875rem",
    display: "flex",
    alignItems: "start",
    justifyContent: "start",
    flexFlow: "column wrap",
    gap: "8px"
}))

const MessageDateBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "center",
    width: "100%"
}))
const MessageDateItemBoxStyled = styled(Box)(() => sx({
    p: "4px 8px",
    fontSize: "0.75rem",
    borderRadius: "4px",
    color: COLORS.BLACK
}))

const ClickableSvgBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    padding: `${InputBoxSizes.buttonBox.padding}px`,
    "svg": {
        transform: "scale(1.25)",
    },
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY,
        color: COLORS.WHITE,
        borderRadius: "4px"
    }
})) as typeof Box;

const InnerRowBoxStyled = styled(Box)(() => sx({
    width: "100%",
    display: "flex",
    alignItems: "flex-end",
    backgroundColor: COLORS.WHITE,
    padding: `${InputBoxSizes.textareaBox.padding}px 16px`,
    borderRadius: "4px",
    border: `${InputBoxSizes.textareaBox.border}px solid ${COLORS.BLACK_10}`
})) as typeof Box;

const FirstRowIconBoxStyled = styled(Box)(() => sx({
    display: "inline-flex",
    height: `${InputBoxSizes.textareaBox.padding * 2 + InputBoxSizes.textareaBox.border * 2 + InputBoxSizes.textareaBox.textarea.lineHeight + InputBoxSizes.buttonBox.padding}px `,
    width: "24px",
    alignItems: "center",
    "> input": {
        visibility: "hidden",
        width: 0,
        height: 0
    }
})) as typeof Box;

const DownloadableAttachmentBoxStyled = styled(Box)(() => sx({
    borderRadius: "4px",
    backgroundColor: "white",
    color: COLORS.PRIMARY,
    textDecoration: "none",
    display: "flex",
    flexFlow: "row nowrap",
    alignItems: "center",
    justifyContent: "space-between",
    gap: "10px",
    padding: "4px",
    border: `1px solid ${COLORS.BLACK_10}`,
    ":hover": {
        cursor: "pointer",
        color: COLORS.PRIMARY,
        textDecoration: "underline"
    }
})) as typeof Box;

const TextRowBoxStyled = styled(Box)(() => sx({
    display: "flex",
    justifyContent: "space-between",
    alignItems: "flex-end",
    gap: "8px"
})) as typeof Box;

const DeleteIconBoxStyled = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    justifyContent: "flex-end",
    opacity: "0.7",
    "svg": {
        transform: "scale(1.25)"
    },
    ":hover": {
        opacity: 1,
        cursor: "pointer"
    }
})) as typeof Box;

const AttachmentBoxStyled = styled(Box)(() => sx({
    height: `${InputBoxSizes.attachmentsContainer.itemBox.height}px`,
    display: "flex",
    alignItem: "center",
    justifyContent: "space-between",
    width: "100%",
    fontSize: "0.875rem"
})) as typeof Box;

const AttachmentContainerBoxStyled = styled(Box)(() => sx({
    display: "flex",
    flexFlow: "column nowrap",
    marginTop: "16px",
    gap: `${InputBoxSizes.attachmentsContainer.gap}px`,
    margin: "0 auto",
    paddingRight: { xs: 0, md: "20px" }
})) as typeof Box;

const TopOpponentBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    flexFlow: "row nowrap",
    alignItems: "center",
    padding: "12px 16px",
    borderBottom: `1px solid ${COLORS.BLACK_10}`
})) as typeof Box;

const ConversationListBoxStyled = styled(Box)(() => sx({
    display: "flex",
    width: "100%",
    flexFlow: "column nowrap",
    padding: { xs: "12px", md: "0px" }
})) as typeof Box;

const ContainerCustomStyled = styled(ContainerStyled)(() => sx({
    display: "flex",
    flex: "1 1 auto",
    width: "100%",
    height: "100%",
    minHeight: 0,
    flexFlow: "row nowrap",
    flexGrow: 1,
    gap: "12px",
})) as typeof ContainerStyled;

const LeftBoxStyled = styled(Box)(() => sx({
    height: "100%",
    display: "flex",
    flexDirection: "column",
    flexGrow: 1,
    flexBasis: "30%",
    padding: "8px 0 0",
    gap: "8px"
})) as typeof Box;

const RightBoxStyled = styled(Box)(() => sx({
    display: "flex",
    height: "100%",
    width: "100%",
    flexGrow: 2,
    flexBasis: "70%",
    overflowX: "hidden",
    overflowY: "auto",
    flexFlow: "column noWrap",
    borderLeft: `1px solid ${COLORS.BLACK_10}`,
    borderRight: `1px solid ${COLORS.BLACK_10}`
})) as typeof Box;

const EmojiPrickerBoxStyled = styled(Box)(() => sx({
    "em-emoji-picker": {
        width: { xs: "310px", md: "360px" },
        resize: "horizontal",
        overflow: "auto"
    }
})) as typeof Box;

const EmptyMessagesContent = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "24px",
    justifyContent: "center",
    flexFlow: "column noWrap",
    height: "100%",
    alignContent: "center"
})) as typeof Box;

const EmptyMessagesHeader = styled(Box)(() => sx({
    display: "flex",
    alignItems: "center",
    gap: "8px",
    justifyContent: "center",
    flexFlow: "column noWrap"
})) as typeof Box;

const getMessageShorten = (message: string | undefined | null, length: number) => {
    return !message
        ? ""
        : message.length <= length
            ? message
            : `${message.slice(0, length)}...`
}

type MessageContextType = {
    attachments: number,
    textareaHeight: number,
    setTextareaHeight: (textareaHeight: number) => void,
    setAttachments: (attachmentNumber: number) => void
    getTextFieldOffset: () => number
};

const MessageContext = createContext<MessageContextType>({
    textareaHeight: InputBoxSizes.textareaBox.textarea.lineHeight,
    attachments: 0,
    setTextareaHeight: () => { },
    setAttachments: () => { },
    getTextFieldOffset: () => 0
});

const ChatContextProvider = ({ children }: { children: any }) => {
    const [textareaHeight, setTextareaHeight] = useState<number>(InputBoxSizes.textareaBox.textarea.lineHeight);
    const [attachments, setAttachments] = useState<number>(0);

    const getInputBoxHeight = () => {
        return textareaHeight +
            InputBoxSizes.marginTop +
            InputBoxSizes.padding * 2 +
            InputBoxSizes.border +
            InputBoxSizes.textareaBox.padding * 2 +
            InputBoxSizes.textareaBox.border * 2 +
            InputBoxSizes.buttonBox.padding +
            (attachments > 0 ? InputBoxSizes.gap : 0) +
            (attachments > 1 ? ((attachments - 1) * InputBoxSizes.attachmentsContainer.gap) : 0) +
            (attachments > 0 ? attachments * InputBoxSizes.attachmentsContainer.itemBox.height : 0);
    }

    return (
        <MessageContext.Provider value={{
            textareaHeight,
            attachments,
            setTextareaHeight: (textareaHeight: number) => {
                setTextareaHeight(textareaHeight)
            },
            setAttachments: (attachmentsNumber: number) => { setAttachments(attachmentsNumber) },
            getTextFieldOffset: getInputBoxHeight
        }}>
            {children}
        </MessageContext.Provider>
    )
}

const Conversation = ({ conversation, currentUserId, isSelected }: { conversation: ConversationVM, currentUserId: number, isSelected: boolean }) => {
    const userInfo = currentUserId === conversation.initiator!.userId!
        ? conversation.opponent
        : conversation.initiator;

    return userInfo
        ? (
            <ConverstionalBoxStyled>
                <ConversationBoxStyled className={isSelected ? "active" : ""}>
                    <Box sx={{ display: "flex", width: "100%", flexFlow: "row nowrap", justifyContent: "space-between", alignItems: "center", gap: "8px" }}>
                        <AvatarStyled src={getProfilePhotoThumbnailUrl(userInfo)} />
                        <Box sx={{ display: "flex", width: "100%", flexFlow: "column nowrap", alignItems: "center" }}>
                            <Box sx={{ display: "flex", width: "100%", flexFlow: "row nowrap", justifyContent: "space-between", alignItems: "center" }}>
                                <Typography sx={{ fontSize: "1rem", lineHeight: "20px", fontWeight: "600" }}>
                                    {getShortName(userInfo)}
                                </Typography>
                                {conversation.latestMessage ? (
                                    <Typography sx={{ fontSize: "0.75rem", lineHeight: "20px", color: COLORS.BLACK_60, height: "100%" }}>
                                        <Moment fromNow>{conversation.latestMessage.created}</Moment>
                                    </Typography>
                                ) : <></>}
                            </Box>
                            <Box sx={{ display: "flex", width: "100%", flexFlow: "row nowrap", justifyContent: "space-between", alignItems: "center" }}>
                                {conversation.latestMessage ? (
                                    <Typography sx={{ fontSize: "0.875rem", lineHeight: "20px", color: (conversation.countOfUnreadMessages && conversation.countOfUnreadMessages > 0) ? COLORS.PRIMARY : COLORS.BLACK_60, width: "100%" }}>
                                        {getMessageShorten(conversation.latestMessage.textValue, 35)}
                                    </Typography>
                                ) : <></>}
                                {conversation.countOfUnreadMessages && conversation.countOfUnreadMessages > 0 ? (
                                    <Box sx={{ svg: { transform: "scale(0.7)", fill: COLORS.PRIMARY } }}>
                                        <FilledCircleIcon />
                                    </Box>
                                ) : <></>}
                            </Box>
                        </Box>
                    </Box>
                </ConversationBoxStyled>
            </ConverstionalBoxStyled>
        ) : <></>
}

const DownloadableAttachment = ({ messageId, attachment }: { messageId: number, attachment: AppMessageAttachmentVM }) => {
    return (
        <Link href={FileService.getFileUrl(attachment.filePath ?? "")} target="_blank" rel="noreferrer">
            <DownloadableAttachmentBoxStyled>
                <AttachmentIconBoxStyled><FileIcon /></AttachmentIconBoxStyled>
                <Box sx={{ display: "inline-flex", width: "100%", justifyContent: "flex-start" }}>
                    <DownloadableAttachmentTypographyStyled>
                        {getShortenText(attachment.name, MAX_ATTACHMENT_NAME_LEN)}
                    </DownloadableAttachmentTypographyStyled>
                </Box>
            </DownloadableAttachmentBoxStyled>
        </Link>
    )
}

const InputAttachmentFile = ({ file, onFileDelete }: { file: File, onFileDelete: () => void }) => {

    return (
        <AttachmentBoxStyled>
            <Box sx={{ display: "flex", gap: "8px", width: "100%" }}>
                <AttachmentIconBoxStyled><FileIcon /></AttachmentIconBoxStyled>
                <AttachmentItemBoxStyled>
                    {getShortenText(file.name, MAX_ATTACHMENT_NAME_LEN)}
                </AttachmentItemBoxStyled>
            </Box>
            <DeleteIconBoxStyled onClick={onFileDelete}>
                <CloseIcon />
            </DeleteIconBoxStyled>
        </AttachmentBoxStyled>
    )
}

const OpponentSkeleton = () => {

    return (
        <Skeleton animation="wave" variant="rectangular" width="140px" height="48px" sx={{ borderRadius: "4px" }} />
    )
}

const OpponentComponent = () => {
    const opponent = useAppSelector(state => selectOpponent()(state));

    return opponent ? (
        <Box sx={{ display: "flex", flexFlow: "row nowrap", alignItems: "center", gap: "12px" }}>
            <AvatarStyled src={getProfilePhotoThumbnailUrl(opponent)} />
            <Typography sx={{ fontSize: "1rem", lineHeight: "20px", fontWeight: "600" }}>{getUserName(opponent!)}</Typography>
        </Box>
    ) : <></>
}

const DesktopTopOpponentsBox = () => {
    const opponent = useAppSelector(state => selectOpponent()(state));

    return (
        <TopOpponentBoxStyled sx={{ justifyContent: "flex-end" }}>
            {opponent ? (<OpponentComponent />) : <></>}
            {!opponent ? (<OpponentSkeleton />) : <></>}
        </TopOpponentBoxStyled>
    )
}

const VideoCallTopBox = () => {
    const { t } = useTranslation();
    const { chatClick: handleChatClick } = useVideoControllersContext();

    return (
        <TopOpponentBoxStyled sx={{ justifyContent: "space-between", padding: "20px 16px" }}>
            <Typography sx={{ fontSize: "1.25rem", lineHeight: "20px", fontWeight: "600" }}>{t(TRANSLATION_KEYS.CHAT.CHAT)}</Typography>
            <DeleteIconBoxStyled onClick={handleChatClick}>
                <CloseIcon />
            </DeleteIconBoxStyled>
        </TopOpponentBoxStyled>
    )
}

const MobileTopOpponentsBox = () => {
    const opponent = useAppSelector(state => selectOpponent()(state));
    const dispatch = useAppDispatch();

    const handleBack = () => {
        dispatch(setSelectedConversation(undefined));
    }

    return (
        <TopOpponentBoxStyled sx={{ gap: "12px" }}>
            <Box sx={{ display: "flex", height: "100%", alignItems: "center", ":hover": { cursor: "pointer" } }} onClick={handleBack}>
                <BackIcon />
            </Box>
            {opponent ? (<OpponentComponent />) : <></>}
            {!opponent ? (<OpponentSkeleton />) : <></>}
        </TopOpponentBoxStyled>
    )
}

const MessageTextFieldBox = () => {
    const { textareaHeight, setTextareaHeight, setAttachments } = useContext(MessageContext);
    const { screenType } = useContext(UIContext);

    const [text, setText] = useState("");
    const [files, setFiles] = useState<File[]>([]);
    const firstUpdate = useRef(true);
    const textareaRef = useRef<HTMLTextAreaElement>(null);
    const textareaBoxRef = useRef<HTMLTextAreaElement>(null);

    const dispatch = useAppDispatch();
    const conversation = useAppSelector(selectSelectedConversation);
    const opponent = useAppSelector(state => selectOpponent()(state));
    const { t } = useTranslation();

    useEffect(() => {
        if (firstUpdate.current) {
            // Fix height issue when textarea placeholder doesn't fit during "opening" 
            firstUpdate.current = false;
            return;
        }
        if (!textareaRef.current) return;
        const calculatedHeight = calculateHeight(textareaRef.current!, InputBoxSizes.textareaBox.textarea.lineHeight);
        if (calculatedHeight !== textareaHeight) {
            setTextareaHeight(calculatedHeight);
        }
    }, [textareaRef, text]);

    useEffect(() => {
        if (screenType === "desktop") {
            document.addEventListener('keydown', handleKeydown);
        }

        return () => {
            if (screenType === "desktop") {
                document.removeEventListener('keydown', handleKeydown);
            }
        }
    }, [screenType, text, textareaRef, textareaHeight]);

    const resetInputBox = () => {
        setText("");
        setFiles([]);
        setAttachments(0);
    }

    const submitMessage = () => {
        if (!opponent) return;
        if (text.length === 0 && files.length === 0) return;

        dispatch(createMessageWithAttachment({ opponentId: opponent.userId!, messageText: text, files: files }));
        resetInputBox();
    }

    const handleKeydown = (e: KeyboardEvent) => {
        if (e.repeat) {
            return;
        }
        if (e.key === 'Enter' && e.ctrlKey) {
            setText(`${text}\n`);
        }
        if (e.key === 'Enter' && !e.ctrlKey) {
            submitMessage();
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const newVal = e.target.value;
        const newLastChar = newVal[newVal.length - 1];
        const prevLastChar = text[text.length - 2];
        const lineSeparators = ["\n", "\r\n"];
        if (screenType === "desktop" && !lineSeparators.some(n => n === prevLastChar) && lineSeparators.some(n => newLastChar === n)) {
            return;
        }
        setText(newVal);
    }

    const handleSelectEmoji = (icon: { native: string }) => {
        setText(text + icon.native);
    }

    const handleAddFiles = (list: FileList | null) => {
        if (!list) return;
        setAttachments(list.length);
        setFiles(Array.from(list));
    }

    const handleDeleteFile = (fileIdx: number) => {
        if (!files) return;
        setAttachments(files.length - 1);
        setFiles(produce(files, draft => {
            draft.splice(fileIdx, 1);
        }));
    }

    return (
        <MessageTextInputBoxStyled>
            <form>
                <TextRowBoxStyled>
                    <FirstRowIconBoxStyled>
                        <label htmlFor="file-input"><ClickableSvgBoxStyled><AttachmentIcon /></ClickableSvgBoxStyled></label>
                        <input id="file-input" type="file" multiple onChange={(e) => handleAddFiles(e.target.files)} />
                    </FirstRowIconBoxStyled>

                    <InnerRowBoxStyled ref={textareaBoxRef}>
                        <textarea
                            ref={textareaRef}
                            value={text}
                            onChange={handleChange}
                            placeholder={t(TRANSLATION_KEYS.CHAT.WRITE_A_MESSAGE)}
                            style={{ height: `${textareaHeight}px` }}
                        />
                        <Menu
                            content={<ClickableSvgBoxStyled><EmojiIcon /></ClickableSvgBoxStyled>}
                            innerMenu={
                                <EmojiPrickerBoxStyled>
                                    <Picker data={data} theme="light" onEmojiSelect={handleSelectEmoji} />
                                </EmojiPrickerBoxStyled>
                            }
                            autoClose={false} // cannot be true otherwise cannot change icon category
                            anchorOrigin={{
                                vertical: 'top',
                                horizontal: 'center',
                            }}
                            transformOrigin={{
                                vertical: 'bottom',
                                horizontal: 'center',
                            }}
                        />
                    </InnerRowBoxStyled>

                    <FirstRowIconBoxStyled>
                        <ClickableSvgBoxStyled onClick={submitMessage}><SendIcon /></ClickableSvgBoxStyled>
                    </FirstRowIconBoxStyled>
                </TextRowBoxStyled>
            </form>
            {
                files.length > 0 && (
                    <AttachmentContainerBoxStyled sx={{ width: { xs: "100%", md: `${textareaBoxRef.current!.scrollWidth}px` } }}>
                        {
                            files.map((file, idx) => (
                                <InputAttachmentFile key={idx} file={file} onFileDelete={() => handleDeleteFile(idx)} />
                            ))
                        }
                    </AttachmentContainerBoxStyled>
                )
            }
        </MessageTextInputBoxStyled>
    )
}

const UserMessages = ({ messageGroup }: { messageGroup: { senderId: number, date: moment.Moment, messages: AppMessageVM[] } }) => {
    const currentUser = useAppSelector(selectShortUserInfo);
    const opponent = useAppSelector(state => selectOpponent()(state));
    const conversation = useAppSelector(selectSelectedConversation);

    const HOUR_FORMAT = "hh:mm A";

    return (conversation && currentUser) ? (
        <Box sx={{ width: "100%", display: "flex", flexFlow: "row nowrap", gap: "8px" }}>
            <Box >
                {messageGroup.senderId === currentUser!.userId! ? <AvatarStyled src={getProfilePhotoThumbnailUrl(currentUser)} /> : <></>}
                {messageGroup.senderId === opponent!.userId! ? <AvatarStyled src={getProfilePhotoThumbnailUrl(opponent)} /> : <></>}
            </Box>
            <Box sx={{ width: "100%", display: "flex", flexFlow: "column nowrap", gap: "4px" }}>
                <Box sx={{ width: "100%", display: "flex", flexFlow: "row nowrap", gap: "4px", alignContent: "flex-end" }}>
                    <Box>
                        {messageGroup.senderId === currentUser!.userId! ? <UserNameTypographyStyled>{getUserName(currentUser)}</UserNameTypographyStyled> : <></>}
                        {messageGroup.senderId === opponent!.userId! ? <UserNameTypographyStyled>{getUserName(opponent)}</UserNameTypographyStyled> : <></>}
                    </Box>
                    <Box>
                        <TimeTypographyStyled>{messageGroup.date.format(HOUR_FORMAT)}</TimeTypographyStyled>
                    </Box>
                </Box>
                <Box sx={{ display: "flex", width: "100%", flexDirection: 'column-reverse', rowGap: "8px" }}>
                    {
                        messageGroup.messages.map((message, messageIdx) => (
                            <Box key={message.messageId!} sx={{ display: "flex", width: "100%", justifyContent: "flex-start" }}>
                                <MessageItemBoxStyled>
                                    <MessageTypographyStyled>{message.textValue ?? ""}</MessageTypographyStyled>
                                    {
                                        message.attachments && message.attachments.length > 0
                                            ? (
                                                <Box sx={{ display: "flex", width: "100%", flexFlow: "column wrap", gap: "8px", a: { textDecoration: "none" } }}>
                                                    {
                                                        message.attachments.map((attachment, attachmentIdx) => (
                                                            <DownloadableAttachment key={attachmentIdx} messageId={message.messageId!} attachment={attachment} />
                                                        ))
                                                    }
                                                </Box>
                                            ) : <></>
                                    }
                                </MessageItemBoxStyled>
                            </Box>
                        ))
                    }
                </Box>
            </Box>
        </Box>
    ) : (<></>)
}

const MessagesSkeleton = () => {
    return (
        <Box sx={{ display: "flex", height: "100%", width: "100%", alignItems: "center", justifyContent: "center" }}>
            <CircularProgress />
        </Box>
    )
}

const InfinityLoadSkeleton = () => {

    return (
        <Box sx={{ width: "100%", display: "flex", alignItems: "center", justifyContent: "center" }}>
            <CircularProgress />
        </Box>
    )
}

const DesktopMessagesEmptyState = () => {
    const userInfo = useAppSelector(selectShortUserInfo);
    const { t } = useTranslation();

    return (
        <Box sx={{ height: "100%", display: "flex", flexFlow: "column noWrap" }}>
            <NavbarWrapped />
            <EmptyMessagesContent>
                <EmptyMessagesHeader>
                    <H3Styled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.TITLE)}</H3Styled>
                    {userInfo?.userRoleId === UserRoleEnum.Student ? (
                        <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.STUDENT_DESCRIPTION)}</PStyled>) :
                        <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.TUTOR_DESCRIPTION)}</PStyled>}
                </EmptyMessagesHeader>
                <EmptyMessagesIcon />
            </EmptyMessagesContent>
        </Box>
    )
}

const MobileMessagesEmptyState = () => {
    const userInfo = useAppSelector(selectShortUserInfo);
    const { t } = useTranslation();

    return (
        <EmptyMessagesContent>
            <EmptyMessagesHeader>
                <H3Styled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.TITLE)}</H3Styled>
                {userInfo?.userRoleId === UserRoleEnum.Student ? (
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.STUDENT_DESCRIPTION)}</PStyled>) :
                    <PStyled sx={{ textAlign: "center" }}>{t(TRANSLATION_KEYS.CHAT.EMPTY_STATE.TUTOR_DESCRIPTION)}</PStyled>}
            </EmptyMessagesHeader>
            <EmptyMessagesIcon />
        </EmptyMessagesContent>
    )
}

const Messages = () => {
    const dispatch = useAppDispatch();
    const conversation = useAppSelector(selectSelectedConversation);
    const user = useAppSelector(selectShortUserInfo);
    const messages = useAppSelector(selectMessages);
    const hasMoreData = !messages || messages.data.length < (messages.size ?? 0);
    const DAY_FORMAT = "ddd, DD MMM";

    useEffect(() => {
        if (conversation !== undefined) {
            dispatch(setMessagesFilter({ conversationId: conversation.conversationId!, addToThePrevious: false, filter: { take: DEFAULT_MESSAGE_TAKE } }));
        }
    }, [conversation]);

    useEffect(() => {
        if (conversation && messages && hasUnreadMessages(messages.data, user!.userId!)) {
            dispatch(setRead(conversation.conversationId!));
        }
    }, [conversation, messages]);

    const fetchMoreData = () => {
        if (!conversation) return;
        dispatch(setMessagesFilter({ conversationId: conversation.conversationId!, addToThePrevious: true, filter: { take: DEFAULT_MESSAGE_TAKE, skip: messages?.data?.length ?? 0 } }));
    }

    return (
        <Box sx={{ display: "flex", flex: "1 1 auto", paddingRight: "8px", overflow: "auto" }}>
            <MessagesBoxStyled id="scrollableDiv" height="100%">
                {messages ? (
                    <InfiniteScroll
                        dataLength={messages.data.length}
                        style={{ display: 'flex', flexDirection: 'column-reverse' }}
                        next={fetchMoreData}
                        hasMore={hasMoreData}
                        loader={<InfinityLoadSkeleton />}
                        inverse={true}
                        scrollableTarget="scrollableDiv"
                    >
                        {getGrouppedMessagesByDay(messages.data).map((messageGroup, idx) => (
                            <Box key={idx} sx={{ display: "flex", flexWrap: "wrap", width: "100%", rowGap: "8px" }}>
                                <MessageDateBoxStyled>
                                    <MessageDateItemBoxStyled>
                                        {messageGroup.date.format(DAY_FORMAT)}
                                    </MessageDateItemBoxStyled>
                                </MessageDateBoxStyled>
                                <Box sx={{ display: "flex", width: "100%", flexDirection: 'column-reverse', rowGap: "16px" }}>
                                    {
                                        getGrouppedMessagesByUserAndTime(messageGroup.messages).map((userMessages, idx) => (
                                            <UserMessages key={idx} messageGroup={userMessages} />
                                        ))
                                    }
                                </Box>
                            </Box>
                        ))}
                    </InfiniteScroll>
                ) : (<MessagesSkeleton />)}
            </MessagesBoxStyled>
        </Box>
    )
}

const ConversationsList = ({ conversations, autoselect = true }: { conversations: GetConversationsResponse, autoselect?: boolean }) => {
    const selectedConversation = useAppSelector(selectSelectedConversation);
    const userInfo = useAppSelector(selectShortUserInfo);
    const dispatch = useAppDispatch();

    const handleSelectConversation = (conversation: GetConversationsResponse[0]) => {
        dispatch(setSelectedConversation(conversation.conversationId!));
    }

    useEffect(() => {
        if (conversations.length > 0 && !selectedConversation && autoselect) {
            handleSelectConversation(conversations[0]);
            dispatch(setSelectedConversation(conversations[0].conversationId!));
        }
    }, [conversations, selectedConversation, autoselect]);

    return (
        <ConversationListBoxStyled>
            {
                userInfo ? conversations.map((conversation, idx) => (
                    <Box key={conversation.conversationId!} onClick={() => handleSelectConversation(conversation)}>
                        <Conversation conversation={conversation} currentUserId={userInfo.userId!} isSelected={(selectedConversation?.conversationId ?? 0) === conversation.conversationId} />
                    </Box>
                )) : <></>
            }
        </ConversationListBoxStyled>
    )
}

const ConversationListLoading = () => {

    return (
        <ConversationListBoxStyled sx={{ display: "flex", width: "100%", height: "100%", alignItems: "center", justifyContent: "center" }}>
            <CircularProgressCustom />
        </ConversationListBoxStyled>
    )
}

const DesktopConversationListView = () => {
    const conversations = useAppSelector(selectConversations);
    const conversationsStatus = useAppSelector(selectConversationsStatus);

    return (
        <>
            {conversationsStatus === LoadingStatusEnum.Loading ? <ConversationListLoading /> : <></>}
            {conversationsStatus === LoadingStatusEnum.Loaded && conversations ? <ConversationsList conversations={conversations} autoselect={true} /> : <></>}
            {conversationsStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
        </>
    )
}

const DesktopConversationMessageView = () => {
    return (
        <>
            <Box sx={{ display: "flex", flex: "0 1 auto", width: "100%", flexFlow: "row nowrap" }}>
                <DesktopTopOpponentsBox />
            </Box>
            <Box sx={{ display: "flex", flex: "1 1 auto", width: "100%", flexFlow: "column nowrap", overflow: "auto" }}>
                <Messages />
                <MessageTextFieldBox />
            </Box>
        </>
    )
}

const MobileConversationMessagesView = () => {

    return (
        <>
            <Box sx={{ display: "flex", flex: "0 1 auto", width: "100%", flexFlow: "row nowrap" }}>
                <MobileTopOpponentsBox />
            </Box>
            <Box sx={{ display: "flex", flex: "1 1 auto", width: "100%", flexFlow: "column nowrap", overflow: "auto" }}>
                <Messages />
                <MessageTextFieldBox />
            </Box>
        </>
    )
}

const MobileConversationListView = () => {
    const conversations = useAppSelector(selectConversations);
    const conversationsStatus = useAppSelector(selectConversationsStatus);

    return (
        <React.Fragment>
            {conversationsStatus === LoadingStatusEnum.Loading ? <ConversationListLoading /> : <></>}
            {conversationsStatus === LoadingStatusEnum.Loaded && conversations ? <ConversationsList conversations={conversations} autoselect={false} /> : <></>}
            {conversationsStatus === LoadingStatusEnum.Error ? <ErrorLarge /> : <></>}
        </React.Fragment>
    )
}

const NavbarWrapped = () => {

    return (
        <Box sx={{ display: "inline-flex", flex: "0 1 auto" }}>
            <Navbar />
        </Box>
    )
}

const DesktopChatPage = () => {
    const conversations = useAppSelector(selectConversations);

    return (
        <>
            {conversations?.length === 0 ? <DesktopMessagesEmptyState /> :
                <Box sx={{ height: "100%", display: "flex", flexFlow: "column noWrap" }}>
                    <NavbarWrapped />
                    <ContainerCustomStyled>
                        <LeftBoxStyled>
                            <DesktopConversationListView />
                        </LeftBoxStyled>
                        <RightBoxStyled>
                            <DesktopConversationMessageView />
                        </RightBoxStyled>
                    </ContainerCustomStyled>
                </Box>}
        </>
    )
}

const MobileChatPage = () => {
    const selectedConversation = useAppSelector(selectSelectedConversation);
    const conversations = useAppSelector(selectConversations);

    return (
        <>
            {!selectedConversation ? <NavbarWrapped /> : <></>}
            {conversations?.length === 0 ? <MobileMessagesEmptyState /> :
                <Box sx={{ display: "flex", flexFlow: "column nowrap", flex: "1 1 auto", overflow: "auto" }}>
                    {!selectedConversation ? <MobileConversationListView /> : <></>}
                    {selectedConversation ? <MobileConversationMessagesView /> : <></>}
                </Box>}
        </>
    )
}

const EmbeddedChat = ({ opponentId }: { opponentId?: number }) => {
    const selectedConversation = useAppSelector(selectSelectedConversation);
    const dispatch = useAppDispatch();
    const conversations = useAppSelector(selectConversations);
    const userInfo = useAppSelector(selectShortUserInfo);

    useEffect(() => {
        if (!conversations) return;
        if (!conversations.length) return;
        if (!userInfo) return;

        const userIds = [opponentId, userInfo.userId!];
        const conversation = conversations.find(x => userIds.includes(x.initiator!.userId!) && userIds.includes(x.opponent!.userId!))
        if (!conversation) return;

        dispatch(setSelectedConversation(conversation.conversationId!));
    }, [conversations, userInfo, opponentId]);

    return (
        <ChatContextProvider>
            <Box sx={{ display: "flex", flexFlow: "column noWrap", width: { sm: "100%", md: "500px" }, height: "100%", border: `1px solid ${COLORS.BLACK_10}`, borderRadius: "8px", overflow: "auto" }}>
                {selectedConversation ? (
                    <>
                        <Box sx={{ display: "flex", flex: "0 1 auto", width: "100%", flexFlow: "row nowrap" }}>
                            <VideoCallTopBox />
                        </Box>
                        <Box sx={{ display: "flex", flex: "1 1 auto", width: "100%", flexFlow: "column nowrap", overflow: "auto" }}>
                            <Messages />
                            <MessageTextFieldBox />
                        </Box>
                    </>
                ) : <></>}
            </Box>
        </ChatContextProvider>
    )
}

const ChatPage = ({ conversationId }: { conversationId?: number }) => {
    const { screenType } = useContext(UIContext);
    const dispatch = useAppDispatch();
    const conversations = useAppSelector(selectConversations);
    const router = useRouter();

    useEffect(() => {
        if (!conversationId) return;
        if (!conversations) return;
        const conversation = conversations.find(x => x.conversationId === conversationId);
        if (!conversation) return;
        dispatch(setSelectedConversation(conversation.conversationId!));
    }, [conversationId, conversations]);

    return (
        <ChatContextProvider>
            {screenType === "desktop" ? <DesktopChatPage /> : <></>}
            {screenType === "mobile" ? <MobileChatPage /> : <></>}
        </ChatContextProvider>
    )
}

type ChatVariantType = "page" | "embedded";
type ChatPropsBaseType = { variant: ChatVariantType }
type ChatPagePropsType = ChatPropsBaseType & { conversationId: number | undefined }
type ChatEmbeddedPropsType = ChatPropsBaseType & { opponentId?: number | undefined };
type ChatPropsType = ChatPagePropsType | ChatEmbeddedPropsType;

const isEmbeddedType = (base: ChatPropsBaseType): base is ChatEmbeddedPropsType => base.variant === "embedded";
const isPageType = (base: ChatPropsBaseType): base is ChatPagePropsType => base.variant === "page";

const Chat = (props: ChatPropsType) => {
    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(setConversations());
    }, [dispatch]);

    return (
        <>
            {isEmbeddedType(props) ? <EmbeddedChat opponentId={props.opponentId} /> : <></>}
            {isPageType(props) ? <ChatPage conversationId={props.conversationId} /> : <></>}
        </>
    )
}

export default Chat;
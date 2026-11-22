import Chat from "components/chat/Chat";
import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";

type QueryType = { conversationId: string | undefined };
const ChatPage = () => {
    const router = useRouter();
    const [conversationId, setConversationId] = useState<number | undefined>();
    const [loaded, setLoaded] = useState(false);

    useEffect(() => {
        if (!router.isReady) return;
        const query = router.query as QueryType;
        const conversationId = query?.conversationId ? parseInt(query.conversationId) : undefined;
        setConversationId(conversationId);
        setLoaded(true);

    }, [router.isReady]);

    return loaded ? <Chat variant="page" conversationId={conversationId} /> : <></>
}

export default withAppClientSideProvider(ChatPage);
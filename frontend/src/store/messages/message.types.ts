import { ChatPagingQuery } from "../chats/chat.types";

export interface Message extends MessageCreate {
    messageUID: string;
}

export interface MessageCreate {
    messageText: string;
    chatUID: string;
    tenantUID: string;
}

export interface MessageQuery {
    messageUID: string;
    tenantUID: string;
    chatUID: string;
}

export interface MessagePagingQuery extends ChatPagingQuery {
    messageUID: string;
}
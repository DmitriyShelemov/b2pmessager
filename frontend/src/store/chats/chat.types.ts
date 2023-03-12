import { Paging } from "../shared.types";

export interface Chat extends ChatCreate {
    chatUID: string;
}

export interface ChatCreate {
    name: string;
    tenantUID: string;
}

export interface ChatQuery {
    tenantUID: string;
    chatUID: string;
}

export interface ChatPagingQuery extends Paging {
    tenantUID: string;
    chatUID: string;
}
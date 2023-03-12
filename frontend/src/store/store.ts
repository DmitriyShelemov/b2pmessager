import { configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import { selectedChatReducer } from "./chats/selectedChat.slice";
import { selectedTenantReducer } from "./tenants/selectedTenant.slice";
import { tenantApi } from "./tenants/tenant.api";
import { chatApi } from "./chats/chat.api";
import { messageApi } from "./messages/message.api";

export const store = configureStore({
    reducer: { 
        [tenantApi.reducerPath]: tenantApi.reducer,
        selectedTenant: selectedTenantReducer, 
        [chatApi.reducerPath]: chatApi.reducer, 
        selectedChat: selectedChatReducer, 
        [messageApi.reducerPath]: messageApi.reducer
    },
    middleware: getDefaultMiddleware => getDefaultMiddleware()
                                        .concat(tenantApi.middleware)
                                        .concat(chatApi.middleware)
                                        .concat(messageApi.middleware),
})

export type TypeRootState = ReturnType<typeof store.getState>
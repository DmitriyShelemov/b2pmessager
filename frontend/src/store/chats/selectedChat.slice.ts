import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Chat } from "./chat.types";

export interface SelectedChat {
    chat: Chat;
}

const initialState:SelectedChat = {} as SelectedChat

export const selectedChatSlice = createSlice({
    name: 'selectedChat',
    initialState,
    reducers: {
        setChat: (state, action:PayloadAction<Chat>) => {
            state.chat = action.payload 
        }
    }
})

export const selectedChatReducer = selectedChatSlice.reducer
export const selectedChatActions = selectedChatSlice.actions

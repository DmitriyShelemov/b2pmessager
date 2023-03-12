import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { Chat, ChatCreate, ChatPagingQuery, ChatQuery } from './chat.types'

export const chatApi = createApi({
    reducerPath: 'api/chat',
    baseQuery: fetchBaseQuery({
        baseUrl: `${process.env.REACT_APP_API_URL}/tenant/`
    }),
    endpoints: build => ({
      getChats: build.query<Chat[], Partial<ChatPagingQuery>>({query: (query) => `${query.tenantUID}/chat?take=${query.take ?? 20}&skip=${query.skip ?? 0}`}),
      getChat: build.query<Chat, ChatQuery>({query: (args) => `${args.tenantUID}/chat/${args.chatUID}`}),
      createChat: build.mutation<Chat, ChatCreate>({
        query: body => ({
          url: `${body.tenantUID}/chat`,
          method: 'POST',
          body: body
        })
      }),
      updateChat: build.mutation<unknown, { uid: string, data: ChatCreate }>({
        query: body => ({
          url: `${body.data.tenantUID}/chat/${body.uid}`,
          method: 'PUT',
          body: body.data
        })
      }),
      deleteChat: build.mutation<unknown, Chat>({
        query: args => ({
          url: `${args.tenantUID}/chat/${args.chatUID}`,
          method: 'DELETE'
        })
      }) 
    })
})

export const { 
  useGetChatsQuery, 
  useGetChatQuery, 
  useLazyGetChatsQuery, 
  useLazyGetChatQuery, 
  useUpdateChatMutation, 
  useCreateChatMutation, 
  useDeleteChatMutation
 } = chatApi
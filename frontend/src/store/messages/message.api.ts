import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { Message, MessageCreate, MessagePagingQuery, MessageQuery } from './message.types'

export const messageApi = createApi({
    reducerPath: 'api/message',
    baseQuery: fetchBaseQuery({
        baseUrl: `${process.env.REACT_APP_API_URL}/tenant/`
    }),
    endpoints: build => ({
      getMessages: build.query<Message[], Partial<MessagePagingQuery>>({
        query: (query) => `${query.tenantUID}/chat/${query.chatUID}/message?take=${query.take ?? 20}&skip=${query.skip ?? 0}`
      }),
      getMessage: build.query<Message, MessageQuery>({
        query: (args) => `${args.tenantUID}/message/${args.messageUID}`
      }),
      createMessage: build.mutation<Message, MessageCreate>({
        query: body => ({
          url: `${body.tenantUID}/message`,
          method: 'POST',
          body: body
        })
      }),
      updateMessage: build.mutation<unknown, { uid: string, data: MessageCreate }>({
        query: body => ({
          url: `${body.data.tenantUID}/message/${body.uid}`,
          method: 'PUT',
          body: body.data
        })
      }),
      deleteMessage: build.mutation<unknown, Message>({
        query: args => ({
          url: `${args.tenantUID}/message/${args.messageUID}`,
          method: 'DELETE'
        })
      }) 
    })
})

export const { 
  useLazyGetMessagesQuery, 
  useLazyGetMessageQuery, 
  useUpdateMessageMutation, 
  useCreateMessageMutation, 
  useDeleteMessageMutation
 } = messageApi
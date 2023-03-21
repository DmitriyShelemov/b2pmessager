import * as React from "react"
import { FC, useRef } from "react"

import { useTypedSelector } from "../hooks/useTypedSelector";
import { useLazyGetMessagesQuery } from "../store/messages/message.api";
import Messager from "./messager";
import RteEditor from "./slate/rteditor";

const MessagesList: FC = () => {
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const {chat} = useTypedSelector(state => state.selectedChat)
    const bottomRef = useRef<HTMLDivElement | null>(null);
    const [trigger, { isLoading, isError, data, error }] = useLazyGetMessagesQuery()

    React.useEffect(() => {
        if (chat) {
            trigger({ tenantUID: tenant.tenantUID, chatUID: chat.chatUID })
        }
    }, [chat])

    React.useEffect(() => {
      if (data) {
        bottomRef.current?.scrollIntoView({behavior: 'smooth'});
      }
  }, [data])

    return (
    <>
    <main className='relative min-h-screen flex flex-col gap-4 py-5 px-4 pt-14 lg:pb-32 xl:pb-28 sm:px-6 lg:px-8 lg:flex-1 bg-zinc-800'>
        {data?.map((msg) => (                
            <div key={msg.messageUID} className="rounded-3xl p-3 bg-zinc-300 text-sm leading-6 shadow-lg ring-1 ring-gray-900/5">  
              <RteEditor value={JSON.parse(msg.messageText)} readonly={true} />
            </div>
        ))}
        <div ref={bottomRef} className="-mt-4" />
    </main>
    <Messager onAdded={() => trigger({ tenantUID: tenant.tenantUID, chatUID: chat.chatUID })} />
    </>
    );
  }
  
  export default MessagesList;
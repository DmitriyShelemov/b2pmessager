import * as React from "react"
import { FC } from "react"

import { useTypedSelector } from "../hooks/useTypedSelector";
import { useLazyGetMessagesQuery } from "../store/messages/message.api";
import Messager from "./messager";
import RteEditor from "./slate/rteditor";

const MessagesList: FC = () => {
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const {chat} = useTypedSelector(state => state.selectedChat)
    const [trigger, { isLoading, isError, data, error }] = useLazyGetMessagesQuery()

    React.useEffect(() => {
        if (chat) {
            trigger({ tenantUID: tenant.tenantUID, chatUID: chat.chatUID })
        }
    }, [chat])

    return (
        <div className='relative px-4 pt-14 sm:px-6 lg:px-8 lg:flex lg:flex-col lg:flex-1 bg-zinc-800'>
          <main className='py-5 lg:flex-1 flex flex-col gap-4'>
            {data?.map((msg) => (                
                <div key={msg.messageUID} className="flex-auto rounded-3xl p-3 bg-zinc-300 text-sm leading-6 shadow-lg ring-1 ring-gray-900/5">  
                  <RteEditor value={JSON.parse(msg.messageText)} readonly={true} />
                </div>
            ))}
          </main>
          <footer className='mx-auto max-w-2xl pb-5 lg:max-w-7xl flex'>
            <Messager onAdded={() => trigger({ tenantUID: tenant.tenantUID, chatUID: chat.chatUID })} />
          </footer>
        </div>

    );
  }
  
  export default MessagesList;
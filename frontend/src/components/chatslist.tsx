import { PlusCircleIcon, RectangleGroupIcon } from "@heroicons/react/24/outline";
import * as React from "react"
import { FC } from "react"

import { useActions } from "../hooks/useActions";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { useCreateChatMutation, useLazyGetChatsQuery } from "../store/chats/chat.api";

const ChatsList: FC = () => {
    const {setChat} = useActions()
    const {chat} = useTypedSelector(state => state.selectedChat)
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const [trigger, { isLoading, isError, data, error }] = useLazyGetChatsQuery()
    const [createChat, result] = useCreateChatMutation()
    const [chatName, setChatName] = React.useState<string>('')

    React.useEffect(() => {
        if (tenant) {
            trigger({ tenantUID: tenant.tenantUID })
        }
    }, [tenant])

    React.useEffect(() => {
        if (data && data.length) {
            setChat(data[0])
        }
    }, [data])

    return (
        <nav className='hidden lg:mt-10 lg:block'>
            <ul role="list" className="mt-4 flex justify-center gap-10 text-base font-medium leading-7 text-stone-700 sm:gap-8 lg:flex-col lg:gap-4">
                <li className="flex gap-2">
                    <div className="flex-1">
                        <input
                            type="text"
                            name="chat-name"
                            id="chat-name"
                            autoComplete="off"
                            value={chatName}
                            onChange={(event) => setChatName(event.target.value)}
                            className="block w-full rounded-md border-0 py-0.5 px-1.5 bg-zinc-600 text-stone-100 shadow-sm ring-1 ring-inset ring-zinc-600 placeholder:text-zinc-400 focus:ring-2 focus:ring-inset focus:ring-zinc-400 focus:outline-none  sm:text-sm sm:leading-6"
                        />
                    </div>
                    <button
                        onClick={() => {
                            if (!chatName) return

                            createChat({
                                name: chatName,
                                tenantUID: tenant.tenantUID
                            })
                            .then(() => {
                                setChatName('')
                                trigger({ tenantUID: tenant.tenantUID })
                            })
                        }}
                        disabled={chatName === ''}
                        className="text-stone-400 disabled:text-stone-700">
                        <PlusCircleIcon  className="h-7 w-7" />
                    </button>
                </li>
            
            {data?.map((chatFromList) => (
                (chatFromList.chatUID !== chat?.chatUID) ?
                <li 
                onClick={() => setChat(chatFromList)}
                className="flex px-2 rounded-xl text-stone-400 hover:bg-zinc-600 hover:text-stone-300 cursor-pointer truncate" 
                key={chatFromList.chatUID} >
                    <a className="group flex items-center" aria-label={chatFromList.name}>
                        <RectangleGroupIcon className="h-5 w-5" />
                        <span className="hidden sm:ml-3 sm:block">{chatFromList.name}</span>
                    </a>
                </li> :
                <li 
                className="flex px-2 rounded-xl bg-zinc-800 text-stone-300 cursor-pointer truncate" 
                key={chatFromList.chatUID} >
                <a className="group flex items-center" aria-label={chatFromList.name} onClick={() => {}}>
                    <RectangleGroupIcon className="h-5 w-5" />
                    <span className="hidden sm:ml-3 sm:block">{chatFromList.name}</span>
                </a>
            </li>
            ))}
            </ul>
        </nav>
    );
  }
  
  export default ChatsList;
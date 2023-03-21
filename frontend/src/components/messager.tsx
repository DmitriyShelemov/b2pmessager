import { PaperAirplaneIcon } from "@heroicons/react/20/solid";
import * as React from "react"
import { FC } from "react"
import { Descendant } from "slate"

import { useTypedSelector } from "../hooks/useTypedSelector";
import { useCreateMessageMutation, useLazyGetMessagesQuery } from "../store/messages/message.api";
import RteEditor from "./slate/rteditor";
  
const defaultValue: Descendant[] = [
  {
    type: 'paragraph',
    children: [
      { text: '' }
    ],
  }
]

const Messager: FC<{ onAdded?: (() => void) | undefined }> = ({ onAdded }) => {
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const {chat} = useTypedSelector(state => state.selectedChat)
    const [createMessage, result] = useCreateMessageMutation()
    const [text, setText] = React.useState<Descendant[]>(JSON.parse(JSON.stringify(defaultValue)) as typeof defaultValue)
    const [messagesSent, setMessagesSent] = React.useState<number>(0)
    const [isdefault, setIsdefault] = React.useState<boolean>(true)

    return !chat ? <></> : (
      <footer className='pt-4 pb-4 fixed bottom-0 left-80 right-0 flex flex-row backdrop-blur-sm bg-zinc-800/10'>
        <div className="flex flex-row gap-4 w-full px-4 sm:px-6 lg:px-8">
          <div className="flex flex-col w-full">
            <RteEditor
              value={text}
              resetCounter={messagesSent}
              onChange={ (value) => {
                if (JSON.stringify(value) === JSON.stringify(text)) return

                  setText?.(value) 
                  setIsdefault(JSON.stringify(value) === JSON.stringify(defaultValue))
                } } />
          </div>
          <button
            className="text-zinc-400 disabled:text-zinc-700"
            disabled={isdefault}
            onClick={() => {
              if (!text)
                return

              createMessage({
                tenantUID: tenant.tenantUID,
                chatUID: chat.chatUID,
                messageText: JSON.stringify(text)
              }).then(() => {
                setText(JSON.parse(JSON.stringify(defaultValue)) as typeof defaultValue)
                setMessagesSent(messagesSent + 1)
                setIsdefault(true)
                onAdded?.()
              })         

              }}>
            <PaperAirplaneIcon className="h-8 w-8" />
          </button>
        </div>
      </footer>
    );
  }
  
export default Messager;
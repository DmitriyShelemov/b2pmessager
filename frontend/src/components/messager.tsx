import { PaperAirplaneIcon } from "@heroicons/react/20/solid";
import * as React from "react"
import { FC } from "react"
import { Descendant } from "slate"

import { useTypedSelector } from "../hooks/useTypedSelector";
import { useCreateMessageMutation, useLazyGetMessagesQuery } from "../store/messages/message.api";
import RteEditor from "./slate/rteditor";
  
const initialValue: Descendant[] = [
  {
    type: 'paragraph',
    children: [
      { text: 'Type your message here' }
    ],
  }
]

const Messager: FC<{ onAdded?: (() => void) | undefined }> = ({ onAdded }) => {
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const {chat} = useTypedSelector(state => state.selectedChat)
    const [createMessage, result] = useCreateMessageMutation()
    const [text, setText] = React.useState<Descendant[]>()
    const [isdefault, setIsdefault] = React.useState<boolean>(true)

    return !chat ? <></> : (
      <div className="flex-1 flex flex-row gap-4">
        <div className="flex flex-col w-full">
          <RteEditor
            value={text ?? initialValue}
            onChange={ (value) => {
              setText?.(value) 
              setIsdefault(false)
              } } />
        </div>
        <button
          className="text-zinc-400 disabled:text-zinc-900"
          disabled={isdefault}
          onClick={() => {
            if (!text)
              return

            createMessage({
              tenantUID: tenant.tenantUID,
              chatUID: chat.chatUID,
              messageText: JSON.stringify(text)
            }).then(() => {
              setText(undefined)
              setIsdefault(true)
              onAdded?.()
            })         

            }}>
          <PaperAirplaneIcon className="h-8 w-8" />
        </button>
      </div>
    );
  }
  
export default Messager;
import { bindActionCreators } from "@reduxjs/toolkit"
import { useDispatch } from "react-redux"
import { selectedTenantActions } from "../store/tenants/selectedTenant.slice"
import { selectedChatActions } from "../store/chats/selectedChat.slice"

const allActions = {
    ...selectedTenantActions,
    ...selectedChatActions
}

export const useActions = () => {
    const dispatch = useDispatch()
    return bindActionCreators(allActions, dispatch)
}
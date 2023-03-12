import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Tenant } from "./tenant.types";

export interface SelectedTenant {
    tenant: Tenant;
}

const initialState:SelectedTenant = {} as SelectedTenant

export const selectedTenantSlice = createSlice({
    name: 'selectedTenant',
    initialState,
    reducers: {
        setTenant: (state, action:PayloadAction<Tenant>) => {
            state.tenant = action.payload 
        }
    }
})

export const selectedTenantReducer = selectedTenantSlice.reducer
export const selectedTenantActions = selectedTenantSlice.actions

import {createApi, fetchBaseQuery} from '@reduxjs/toolkit/query/react'
import { Tenant, TenantCreate, } from './tenant.types'
import { Paging } from './../shared.types'

export const tenantApi = createApi({
    reducerPath: 'api/tenant',
    baseQuery: fetchBaseQuery({
        baseUrl: `${process.env.REACT_APP_API_URL}/tenant`
    }),
    endpoints: build => ({
      getTenants: build.query<Tenant[], Partial<Paging> | null>({query: (paging = null) => `?take=${paging?.take ?? 20}&skip=${paging?.skip ?? 0}`}),
      getTenant: build.query<Tenant, string>({query: (uid) => `/${uid}`}),
      createTenant: build.mutation<Tenant, TenantCreate>({
        query: body => ({
          url: '',
          method: 'POST',
          body 
        })
      }),
      updateTenant: build.mutation<unknown, { uid: string, data: TenantCreate }>({
        query: body => ({
          url: `/${body.uid}`,
          method: 'PUT',
          body: body.data
        })
      }),
      deleteTenant: build.mutation<unknown, string>({
        query: uid => ({
          url: `/${uid}`,
          method: 'DELETE'
        })
      }) 
    })
})

export const { 
  useGetTenantsQuery, 
  useGetTenantQuery, 
  useLazyGetTenantsQuery, 
  useLazyGetTenantQuery, 
  useUpdateTenantMutation, 
  useCreateTenantMutation, 
  useDeleteTenantMutation
 } = tenantApi
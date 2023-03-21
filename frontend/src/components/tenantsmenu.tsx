import * as React from "react"
import { FC, useState } from "react"

import { useActions } from "../hooks/useActions";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { Fragment } from 'react'
import { Popover, Transition } from '@headlessui/react'
import { ChevronDownIcon, UserGroupIcon } from '@heroicons/react/20/solid'
import { PlusCircleIcon } from '@heroicons/react/24/outline'
import { useCreateTenantMutation, useGetTenantsQuery } from "../store/tenants/tenant.api";

const TenantsMenu: FC = () => {
    const {setTenant} = useActions()
    const {tenant} = useTypedSelector(state => state.selectedTenant)
    const [createTenant, { data: createdTenant }] = useCreateTenantMutation()
    const [name, setName] = React.useState<string>('')
    const {data, isLoading, error, refetch } = useGetTenantsQuery(null)

    React.useEffect(() => {
        if (data && data.length) {
            setTenant(data[0])
        }
    }, [data])

    return (
      <div className="hidden min-[416px]:contents">
        <Popover className="relative">
            <Popover.Button className="inline-flex gap-0.5 justify-center overflow-hidden text-sm font-medium transition rounded-full bg-stone-900 py-1 px-3 text-zinc-300 hover:bg-stone-700">
                <span>Tenants</span>
                <ChevronDownIcon className="h-5 w-5" aria-hidden="true" />
            </Popover.Button>

            <Transition
                as={Fragment}
                enter="transition ease-out duration-200"
                enterFrom="opacity-0 translate-y-1"
                enterTo="opacity-100 translate-y-0"
                leave="transition ease-in duration-150"
                leaveFrom="opacity-100 translate-y-0"
                leaveTo="opacity-0 translate-y-1"
            >
                <Popover.Panel className="absolute right-0.5 z-10 mt-2 flex w-xl max-w-xl -mr-5 px-4">
                <div className="w-xl p-3 max-w-xl min-w-xl flex-auto overflow-hidden rounded-3xl bg-stone-700/95 backdrop-blur-sm text-sm leading-6 shadow-lg ring-1 ring-gray-900/5">
                    <ul role="list" data-projection-id="4" className="flex flex-col gap-2">
                        <li className="flex gap-2">
                            <div className="flex-1">
                                <input
                                    type="text"
                                    name="tenant-name"
                                    id="tenant-name"
                                    autoComplete="off"
                                    value={name}
                                    onChange={(event) => setName(event.target.value)}
                                    className="block w-full min-w-[150px] rounded-md border-0 py-0.5 px-1.5 bg-stone-500 text-zinc-100 shadow-sm ring-1 ring-inset ring-stone-600 placeholder:text-stone-400 focus:ring-2 focus:ring-inset focus:ring-stone-400 focus:outline-none sm:text-sm sm:leading-6"
                                />
                            </div>
                            <button
                                onClick={() => {
                                    if (!name) return

                                    createTenant({
                                        name: name,
                                        email: 'test@mail.com'
                                    })
                                    .then(() => {
                                        setName('')
                                        if (createdTenant) {
                                            //debugger
                                            //setTenant(createdTenant);
                                            refetch()
                                        }
                                    })
                                }}
                                disabled={name === ''}
                                className="text-zinc-400 disabled:text-zinc-600">
                                <PlusCircleIcon  className="h-7 w-7" />
                            </button>
                        </li>
                    {data?.map((tenantFromList) => (
                        (tenantFromList.tenantUID !== tenant?.tenantUID) ?
                        <li 
                        key={tenantFromList.tenantUID}
                        className="w-full px-2 rounded-xl text-zinc-400 hover:bg-stone-500 hover:text-zinc-300 cursor-pointer">
                            <a  onClick={() => setTenant(tenantFromList)}
                                className="flex max-w-xl truncate gap-2 py-1 text-sm transition">
                                <UserGroupIcon  className="h-4 w-4" />
                                <span>{tenantFromList.name}</span>
                            </a>
                        </li> :
                        <li 
                        key={tenantFromList.tenantUID}
                        className="w-full px-2 rounded-xl bg-stone-800 text-zinc-300 hover:bg-stone-700 cursor-pointer">
                            <a  className="flex max-w-xl truncate gap-2 py-1 text-sm transition">
                                <UserGroupIcon  className="h-4 w-4" />
                                <span>{tenantFromList.name}</span>
                            </a>
                        </li>
                    ))}
                    </ul>
                </div>
                </Popover.Panel>
            </Transition>
        </Popover>
      </div>
    );
  }
  
  export default TenantsMenu;
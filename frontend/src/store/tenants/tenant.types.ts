export interface Tenant extends TenantCreate {
    tenantUID: string;
}

export interface TenantCreate {
    name: string;
    email: string;
}
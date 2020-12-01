export interface IHealthcareWorkers {
    id?: string;
    name: string;
    branch: Date;
    facility: IHealthFacilities;
    title: string;
}

export interface IHealthFacilities {
    id?: string;
    address: string;
    email?: string;
    municipality: string;
    name: string;
    phone?: string;
    type?: string;
}

export interface IMedicine {
    id?: string;
    name: string;
    strength: string;
    form: string;
    wayOfIssuing: string;
    manufacturer: string;
    price: number;
    packaging: string;
}

export interface IPandemic {
    id?: string;
    name?: string;
    totalMK?: number;
    activeMK?: number;
    deathsMK?: number;
    newMK?: number;
    totalGlobal?: number;
    deathsGlobal?: number;
    activeGlobal?: number;
}

export interface IPharmacy {
    id?: string;
    name: string;
    location: string;
    address: string;
    workAllTime?: boolean;
}

export interface IPharmacyHead {
    id?: string;
    PharmacyMedicines?: IMedicine[];
    Pharmacy?: IPharmacy[];
    Email: string;
    Name: string;
    Passwd: string;
    originalUserName?: string;
    Role?: string;
} 
export interface IPharmacyHeadRequest {
    id?: string;
    PharmacyHead?: IPharmacyHead;
    Pharmacy?: IPharmacy;
}  
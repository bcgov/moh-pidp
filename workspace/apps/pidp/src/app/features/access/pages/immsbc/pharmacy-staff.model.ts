export enum PharmacyRole {
  Clinician = 1,
  Clerk = 2,
  Admin = 3,
  Unknown = 99
}

export interface PharmacyProfile {
  isPharmacyAdmin: boolean;
  associations: {
    pharmacyId: number;
    pharmacyName: string;
    role: number;
  }[];
}

export interface Pharmacy {
  id: number;
  name: string;
  healthAuthority: string;
  address1: string;
  address2: string;
  city: string;
  province: string;
  postalCode: string;
  managerName: string;
  email: string;
  phone: string;
  fax: string;
  pharmaCareCode: string;
}

export interface IStaff {
  partyId: number;
  fullName: string;
  role: PharmacyRole;
  effectiveStartDate: string | null; // Assuming ISO date string from backend
  effectiveEndDate: string | null;
}
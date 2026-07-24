export enum PharmacyRole {
  Admin = 1,
  Clinician = 2,
  Clerk = 3,
  None = 99, // As per backend logic for removal
}

export interface PharmacyProfile {
  isPharmacyAdmin: boolean;
  associations: {
    pharmacyId: number;
    pharmacyName: string;
    role: string;
  }[];
}

export interface Pharmacy {
  id: number;
  name: string;
  address: string;
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
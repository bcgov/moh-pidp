export interface MsTeamsClinicInfo {
  clinicName: string;
  address: ClinicAddress;
  clinicMembers: ClinicMember[];
}

export interface ClinicAddress {
  countryCode: string;
  provinceCode: string;
  street: string;
  city: string;
  postal: string;
}

export interface ClinicMember {
  name: string;
  email: string;
  jobTitle: string;
  phone: string;
}

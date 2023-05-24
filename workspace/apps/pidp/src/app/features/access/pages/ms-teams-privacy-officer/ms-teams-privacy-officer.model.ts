export interface MsTeamsClinicInfo {
  clinicName: string;
  clinicAddress: ClinicAddress;
}

export interface ClinicAddress {
  countryCode: string;
  provinceCode: string;
  street: string;
  city: string;
  postal: string;
}

export interface PrivacyOfficer {
  fullName: string;
  clinicName: string;
  clinicId: number;
  clinicAddress: {
    street: string;
    provinceCode: string;
    city: string;
    postal: string;
    countryCode: string;
  };
}

export interface ClinicId {
  id: number;
}

export interface PrivacyOfficer {
  name: string;
  clinicName: string;
  clinicId: number;
  address: {
    street: string;
    provinceCode: string;
    city: string;
    postalCode: string;
    country: string;
  };
}

export interface ClinicId {
  id: number;
}

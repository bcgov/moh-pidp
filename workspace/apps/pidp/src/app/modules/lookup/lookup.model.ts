export interface LookupConfig {
  colleges: Lookup[];
  countries: CountryLookup[];
  provinces: ProvinceLookup[];
}

export interface Lookup {
  code: number;
  name: string;
}

export interface CountryLookup extends Lookup {
  isoCode: string;
}

export interface ProvinceLookup extends Lookup {
  isoCode: string;
  countryCode: number;
}

export interface LookupConfig {
  colleges: Lookup[];
  countries: Lookup<string>[];
  provinces: ProvinceLookup[];
}

export interface Lookup<T extends number | string = number> {
  code: T;
  name: string;
}

export interface ProvinceLookup extends Lookup<string> {
  countryCode: string;
}

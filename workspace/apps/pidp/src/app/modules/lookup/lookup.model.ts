export interface LookupConfig {
  colleges: CollegeLookup[];
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

export interface CollegeLookup extends Lookup<number> {
  acronym: string;
}

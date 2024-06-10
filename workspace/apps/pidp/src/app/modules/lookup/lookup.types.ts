export const lookupConfigKeys = [
  'colleges',
  'countries',
  'provinces',
  'organizations',
  'healthAuthorities',
] as const;

export type LookupConfigKey = (typeof lookupConfigKeys)[number];

export type ILookupConfig = {
  [K in LookupConfigKey]: Lookup<string | number>[];
};

export interface LookupConfig extends ILookupConfig {
  accessTypes: Lookup[];
  colleges: CollegeLookup[];
  countries: Lookup<string>[];
  provinces: ProvinceLookup[];
  organizations: Lookup[];
  healthAuthorities: Lookup[];
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

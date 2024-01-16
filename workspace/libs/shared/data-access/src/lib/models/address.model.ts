export type AddressLine = Exclude<keyof Address, 'id'>;
export type AddressLineMap<T> = { [key in AddressLine]: T };

export type AddressType =
  | 'verifiedAddress'
  | 'physicalAddress'
  | 'mailingAddress';

export const addressTypes: AddressType[] = [
  'verifiedAddress',
  'mailingAddress',
  'physicalAddress',
];

export type AddressMap<T> = { [key in keyof Address]: T };
export const optionalAddressLineItems: (keyof Address)[] = [];

export class Address {
  public constructor(
    public countryCode: string | null = null,
    public provinceCode: string | null = null,
    public street: string | null = null,
    public city: string | null = null,
    public postal: string | null = null,
  ) {
    this.street = street;
    this.city = city;
    this.provinceCode = provinceCode;
    this.countryCode = countryCode;
    this.postal = postal;
  }

  /**
   * @description
   * Create an new instance of an Address.
   *
   * NOTE: Useful for converting object literals (or data
   * transfer objects) into an instance.
   */
  public static instanceOf(address: Address): Address {
    const {
      street = null,
      city = null,
      provinceCode = null,
      countryCode = null,
      postal = null,
    } = address;
    return new Address(countryCode, provinceCode, street, city, postal);
  }

  /**
   * @description
   * Check for an empty address.
   */
  public static isEmpty(
    address: Address,
    omitList: (keyof Address)[] = optionalAddressLineItems,
  ): boolean {
    return address
      ? (Object.keys(address) as AddressLine[])
          .filter((key: keyof Address) => !omitList.includes(key))
          .every((k) => !address[k])
      : true;
  }

  /**
   * @description
   * Checks for a partial address.
   */
  public static isNotEmpty(
    address: Address,
    omitList: (keyof Address)[] = optionalAddressLineItems,
  ): boolean {
    return address ? !this.isEmpty(address, omitList) : false;
  }
}

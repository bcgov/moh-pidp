import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { distinctUntilChanged, pairwise, startWith } from 'rxjs';

import { Address, AddressLine, Country } from '@bcgov/shared/data-access';

import { LookupService } from '@app/modules/lookup/lookup.service';
import { Lookup, ProvinceLookup } from '@app/modules/lookup/lookup.types';

import { FormUtilsService } from '@core/services/form-utils.service';

@Component({
  selector: 'app-address-form',
  templateUrl: './address-form.component.html',
  styleUrls: ['./address-form.component.scss'],
})
export class AddressFormComponent implements OnInit {
  /**
   * @description
   * Address line form.
   */
  @Input() public form!: FormGroup;
  /**
   * @description
   * List of address line controls that should be
   * displayed.
   *
   * NOTE: Country must be included, but does not
   * have to be displayed.
   */
  @Input() public formControlNames: AddressLine[];
  /**
   * @description
   * Whether BC addresses can only be selected using
   * autocomplete.
   */
  @Input() public inBc: boolean;
  /**
   * @description
   * Whether to show the manual address.
   */
  @Input() public showManualButton: boolean;
  /**
   * @description
   * Whether to show the address line fields.
   */
  @Input() public showAddressLineFields: boolean;

  public countries: Lookup<string>[];
  // Includes provinces and states
  public provinces: ProvinceLookup[];
  // Filtered based on country
  public filteredProvinces: ProvinceLookup[];
  public provinceLabel!: string;
  public postalLabel!: string;
  public postalMask!: string;

  public constructor(
    private lookupService: LookupService,
    private formUtilsService: FormUtilsService
  ) {
    this.formControlNames = [
      'countryCode',
      'provinceCode',
      'street',
      'city',
      'postal',
    ];
    this.inBc = false;
    this.showManualButton = true;
    this.showAddressLineFields = false;
    this.countries = this.lookupService.countries;
    this.provinces = this.lookupService.provinces;
    this.filteredProvinces = [];
    this.setAddressLabels();
  }

  public get countryCode(): FormControl {
    return this.form.get('countryCode') as FormControl;
  }

  public get provinceCode(): FormControl {
    return this.form.get('provinceCode') as FormControl;
  }

  public get postal(): FormControl {
    return this.form.get('postal') as FormControl;
  }

  public onAutocomplete({ countryCode, ...address }: Address): void {
    // Populate the associated list of associated provinces/states
    this.countryCode.patchValue(countryCode);
    // Patch the remaining address fields, which includes the province/state
    this.form.patchValue(address);
    this.showManualAddress();
  }

  public showManualAddress(): void {
    this.showAddressLineFields = true;
  }

  public showFormControl(formControlName: AddressLine): boolean {
    return this.formControlNames.includes(formControlName);
  }

  public getFormControlOrder(formControlName: AddressLine): string {
    const index = this.formControlNames.indexOf(formControlName) + 1;
    return `order-${index}`;
  }

  public isRequired(addressLine: AddressLine): boolean {
    return this.formUtilsService.isRequired(this.form, addressLine);
  }

  public ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.setAddress(this.countryCode.value);
    this.countryCode.valueChanges
      .pipe(startWith(Country.CANADA), pairwise(), distinctUntilChanged())
      .subscribe(([prevCountry, nextCountry]: [string, string]) => {
        if (prevCountry !== nextCountry) {
          this.provinceCode.reset();
          this.postal.reset();
        }
        this.setAddress(nextCountry);
      });
    if (Address.isNotEmpty(this.form.value)) {
      // Only every set to true when the address is not empty, otherwise
      // leave control in the hands of the component or input bindings
      this.showAddressLineFields = true;
    }
  }

  private setAddress(countryCode: string): void {
    this.filteredProvinces = this.provinces.filter(
      (p) => p.countryCode === this.countryCode.value
    );
    this.setAddressLabels(countryCode);
  }

  private setAddressLabels(countryCode: string = Country.CANADA): void {
    const { province, postal } = this.addressConfig(countryCode);
    this.provinceLabel = province;
    this.postalLabel = postal.label;
    this.postalMask = postal.mask;
  }

  private addressConfig(countryCode: string): {
    province: string;
    postal: { label: string; mask: string };
  } {
    switch (countryCode) {
      case Country.UNITED_STATES:
        return {
          province: 'State',
          postal: { label: 'Zip Code', mask: '00000' },
        };
      case Country.CANADA:
      default:
        return {
          province: 'Province',
          postal: { label: 'Postal Code', mask: 'S0S 0S0' },
        };
    }
  }
}

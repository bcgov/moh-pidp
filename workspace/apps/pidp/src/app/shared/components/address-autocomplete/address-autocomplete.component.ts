import { NgFor } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { EMPTY, debounceTime, switchMap } from 'rxjs';

import { Address, Province } from '@bcgov/shared/data-access';
import {
  AnchorDirective,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import { AddressAutocompleteResource } from '@core/resources/address-autocomplete-resource.service';
import { ToastService } from '@core/services/toast.service';

import { AddressAutocompleteFindResponse } from './address-autocomplete-find-response.model';
import { AddressAutocompleteRetrieveResponse } from './address-autocomplete-retrieve-response.model';

@Component({
    selector: 'app-address-autocomplete',
    templateUrl: './address-autocomplete.component.html',
    styleUrls: ['./address-autocomplete.component.scss'],
    imports: [
        AnchorDirective,
        InjectViewportCssClassDirective,
        MatAutocompleteModule,
        MatFormFieldModule,
        MatInputModule,
        MatOptionModule,
        NgFor,
        ReactiveFormsModule,
    ]
})
export class AddressAutocompleteComponent implements OnInit {
  /**
   * @description
   * Whether BC addresses can only be selected using
   * autocomplete.
   */
  @Input() public inBc: boolean;
  /**
   * @description
   * Address autocomplete emitter.
   */
  @Output() public autocompleteAddress: EventEmitter<Address>;

  public form!: FormGroup;
  public addressAutocompleteFields: AddressAutocompleteFindResponse[];
  public canadaPostUrl: string;

  public constructor(
    private fb: FormBuilder,
    private toastService: ToastService,
    private addressAutocompleteResource: AddressAutocompleteResource,
  ) {
    this.inBc = false;
    this.autocompleteAddress = new EventEmitter<Address>();
    this.addressAutocompleteFields = [];
    this.canadaPostUrl = 'https://www.canadapost.ca/pca';
  }

  public get autocomplete(): FormControl {
    return this.form.get('autocomplete') as FormControl;
  }

  public onAutocomplete(id: string): void {
    this.addressAutocompleteResource
      .retrieve(id)
      .subscribe((results: AddressAutocompleteRetrieveResponse[]) => {
        const addressRetrieved =
          results.find((result) => result.language === 'ENG') ?? null;

        if (addressRetrieved) {
          const address = new Address(
            addressRetrieved.countryIso2,
            addressRetrieved.provinceCode,
            addressRetrieved.line1,
            addressRetrieved.city,
            addressRetrieved.postalCode,
          );

          !this.inBc || address.provinceCode === Province.BRITISH_COLUMBIA
            ? this.autocompleteAddress.emit(address)
            : this.toastService.openErrorToast('Address must be located in BC');
        } else {
          this.toastService.openErrorToast('Address could not be retrieved');
        }
      });
  }

  public ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({ autocomplete: [''] });

    this.autocomplete.valueChanges
      .pipe(
        debounceTime(400),
        switchMap((value: string) => {
          this.addressAutocompleteFields = [];
          return value ? this.addressAutocompleteResource.find(value) : EMPTY;
        }),
      )
      .subscribe(
        (response: AddressAutocompleteFindResponse[]) =>
          (this.addressAutocompleteFields = response),
      );
  }
}

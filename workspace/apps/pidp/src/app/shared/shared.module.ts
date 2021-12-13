import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { LookupModule } from '@app/modules/lookup/lookup.module';
import { SharedUiModule } from '@bcgov/shared/ui';

import { AddressAutocompleteComponent } from './components/address-autocomplete/address-autocomplete.component';
import { AddressFormComponent } from './components/address-form/address-form.component';
import { AddressInfoComponent } from './components/address-info/address-info.component';

@NgModule({
  declarations: [
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
  ],
  imports: [CommonModule, SharedUiModule, LookupModule],
  exports: [
    CommonModule,
    SharedUiModule,
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
  ],
})
export class SharedModule {}

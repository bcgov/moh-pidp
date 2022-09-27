import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

import { LookupModule } from '@app/modules/lookup/lookup.module';

import { AddressAutocompleteComponent } from './components/address-autocomplete/address-autocomplete.component';
import { AddressFormComponent } from './components/address-form/address-form.component';
import { AddressInfoComponent } from './components/address-info/address-info.component';
import { GetSupportComponent } from './components/get-support/get-support.component';

@NgModule({
  declarations: [
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
    GetSupportComponent,
  ],
  imports: [CommonModule, SharedUiModule, LookupModule.forChild()],
  exports: [
    CommonModule,
    SharedUiModule,
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
    GetSupportComponent,
  ],
})
export class SharedModule {}

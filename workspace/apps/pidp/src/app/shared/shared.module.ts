import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedUiModule } from '@bcgov/shared/ui';

import { LookupModule } from '@app/modules/lookup/lookup.module';

import { AddressAutocompleteComponent } from './components/address-autocomplete/address-autocomplete.component';
import { AddressFormComponent } from './components/address-form/address-form.component';
import { AddressInfoComponent } from './components/address-info/address-info.component';
import { GetSupportComponent } from './components/get-support/get-support.component';
import { NeedHelpComponent } from './components/need-help/need-help.component';
import { DialogBcproviderCreateComponent } from './components/success-dialog/components/dialog-bcprovider-create.component';
import { DialogBcproviderEditComponent } from './components/success-dialog/components/dialog-bcprovider-edit.component';
import { SuccessDialogComponent } from './components/success-dialog/success-dialog.component';
import { IsHighAssurancePipe } from './pipes/is-high-assurance.pipe';

@NgModule({
  declarations: [
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
    GetSupportComponent,
    IsHighAssurancePipe,
    NeedHelpComponent,
    SuccessDialogComponent,
    DialogBcproviderCreateComponent,
    DialogBcproviderEditComponent,
  ],
  imports: [CommonModule, LookupModule.forChild(), SharedUiModule],
  exports: [
    CommonModule,
    SharedUiModule,
    AddressAutocompleteComponent,
    AddressFormComponent,
    AddressInfoComponent,
    GetSupportComponent,
    IsHighAssurancePipe,
    NeedHelpComponent,
    SuccessDialogComponent,
    DialogBcproviderCreateComponent,
    DialogBcproviderEditComponent,
  ],
})
export class SharedModule {}
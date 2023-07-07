import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MfaSetupRoutingModule } from './mfa-setup-routing.module';
import { MfaSetupComponent } from './mfa-setup.component';


@NgModule({
  declarations: [
    MfaSetupComponent
  ],
  imports: [
    CommonModule,
    MfaSetupRoutingModule
  ]
})
export class MfaSetupModule { }

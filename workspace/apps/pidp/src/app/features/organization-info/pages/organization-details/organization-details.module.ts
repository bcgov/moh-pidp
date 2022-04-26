import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrganizationDetailsRoutingModule } from './organization-details-routing.module';
import { OrganizationDetailsComponent } from './organization-details.component';


@NgModule({
  declarations: [
    OrganizationDetailsComponent
  ],
  imports: [
    CommonModule,
    OrganizationDetailsRoutingModule
  ]
})
export class OrganizationDetailsModule { }

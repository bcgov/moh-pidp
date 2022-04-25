import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HcimwebEnrolmentRoutingModule } from './hcimweb-enrolment-routing.module';
import { HcimwebEnrolmentComponent } from './hcimweb-enrolment.component';


@NgModule({
  declarations: [
    HcimwebEnrolmentComponent
  ],
  imports: [
    CommonModule,
    HcimwebEnrolmentRoutingModule
  ]
})
export class HcimwebEnrolmentModule { }

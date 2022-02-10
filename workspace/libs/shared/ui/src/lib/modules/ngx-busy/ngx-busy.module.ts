import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { NgBusyModule } from 'ng-busy';

import { BusyLoadingComponent } from './busy-loading/busy-loading.component';
import { BusyOverlayMessageComponent } from './busy-overlay-message/busy-overlay-message.component';
import { BusyOverlayComponent } from './busy-overlay/busy-overlay.component';
import { busyConfig } from './busy.config';

@NgModule({
  imports: [CommonModule, NgBusyModule.forRoot(busyConfig)],
  declarations: [
    BusyLoadingComponent,
    BusyOverlayComponent,
    BusyOverlayMessageComponent,
  ],
  exports: [NgBusyModule, BusyLoadingComponent, BusyOverlayComponent],
})
export class NgxBusyModule {}

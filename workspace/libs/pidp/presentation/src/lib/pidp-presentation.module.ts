import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { PidpDataModelModule } from '@pidp/data-model';
import { register } from 'swiper/element/bundle';

import { PidpLoadingDialogComponent } from './services/loading-overlay.service';

register();

@NgModule({
  imports: [
    CommonModule,
    PidpDataModelModule,
    MatDialogModule,
    MatProgressSpinnerModule,
  ],
  declarations: [PidpLoadingDialogComponent],
})
export class PidpPresentationModule {}

import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { PidpDataModelModule } from '@pidp/data-model';
import { PidpPresentationModule } from '@pidp/presentation';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementCardComponent } from './components/endorsement-card/endorsement-card.component';
import { EndorsementsRoutingModule } from './endorsements-routing.module';
import { EndorsementsPage } from './endorsements.page';

@NgModule({
  declarations: [EndorsementsPage, EndorsementCardComponent],
  imports: [
    EndorsementsRoutingModule,
    SharedModule,
    FlexLayoutModule,
    PidpDataModelModule,
    PidpPresentationModule,
  ],
})
export class EndorsementsModule {}

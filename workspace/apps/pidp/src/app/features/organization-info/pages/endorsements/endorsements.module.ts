import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementCardComponent } from './components/endorsement-card/endorsement-card.component';
import { EndorsementsRoutingModule } from './endorsements-routing.module';
import { EndorsementsPage } from './endorsements.page';

@NgModule({
    imports: [EndorsementsRoutingModule, SharedModule, EndorsementsPage, EndorsementCardComponent],
})
export class EndorsementsModule {}

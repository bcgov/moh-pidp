import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { EndorsementsRoutingModule } from './endorsements-routing.module';
import { EndorsementsPage } from './endorsements.page';

@NgModule({
  declarations: [EndorsementsPage],
  imports: [EndorsementsRoutingModule, SharedModule],
})
export class EndorsementsModule {}

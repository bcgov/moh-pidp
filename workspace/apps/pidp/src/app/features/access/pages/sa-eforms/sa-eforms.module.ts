import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { AccessModule } from '../../access.module';
import { SaEformsRoutingModule } from './sa-eforms-routing.module';
import { SaEformsPage } from './sa-eforms.page';

@NgModule({
    imports: [SaEformsRoutingModule, SharedModule, AccessModule, SaEformsPage],
})
export class SaEformsModule {}

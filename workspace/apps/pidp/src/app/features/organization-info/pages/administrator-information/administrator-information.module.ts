import { NgModule } from '@angular/core';

import { SharedModule } from '@app/shared/shared.module';

import { AdministratorInformationRoutingModule } from './administrator-information-routing.module';
import { AdministratorInformationPage } from './administrator-information.page';

@NgModule({
    imports: [AdministratorInformationRoutingModule, SharedModule, AdministratorInformationPage],
})
export class AdministratorInformationModule {}

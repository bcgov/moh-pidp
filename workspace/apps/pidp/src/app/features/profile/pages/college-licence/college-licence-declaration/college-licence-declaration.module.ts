import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { CollegeLicenceDeclarationRoutingModule } from './college-licence-declaration-routing.module';
import { CollegeLicenceDeclarationPage } from './college-licence-declaration.page';

@NgModule({
  declarations: [CollegeLicenceDeclarationPage],
  imports: [CollegeLicenceDeclarationRoutingModule, SharedModule],
})
export class CollegeLicenceDeclarationModule {}

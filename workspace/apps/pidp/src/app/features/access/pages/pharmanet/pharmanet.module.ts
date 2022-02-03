import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { NextStepsPage } from './pages/next-steps/next-steps.page';
import { SelfDeclarationPage } from './pages/self-declaration/self-declaration.page';
import { TermsOfAccessPage } from './pages/terms-of-access/terms-of-access.page';
import { PharmanetRoutingModule } from './pharmanet-routing.module';
import { InfoGraphicComponent } from './shared/components/info-graphic/info-graphic.component';

@NgModule({
  declarations: [
    SelfDeclarationPage,
    TermsOfAccessPage,
    NextStepsPage,
    InfoGraphicComponent,
  ],
  imports: [PharmanetRoutingModule, SharedModule],
})
export class PharmanetModule {}

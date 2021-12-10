import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { NextStepsComponent } from './pages/next-steps/next-steps.component';
import { SelfDeclarationComponent } from './pages/self-declaration/self-declaration.component';
import { TermsOfAccessComponent } from './pages/terms-of-access/terms-of-access.component';
import { PharmanetRoutingModule } from './pharmanet-routing.module';

@NgModule({
  declarations: [
    SelfDeclarationComponent,
    TermsOfAccessComponent,
    NextStepsComponent,
  ],
  imports: [PharmanetRoutingModule, SharedModule],
})
export class PharmanetModule {}

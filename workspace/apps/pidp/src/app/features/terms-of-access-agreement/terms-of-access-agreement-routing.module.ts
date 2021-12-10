import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TermsOfAccessAgreementComponent } from './terms-of-access-agreement.component';

const routes: Routes = [
  {
    path: '',
    component: TermsOfAccessAgreementComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TermsOfAccessAgreementRoutingModule {}

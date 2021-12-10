import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SitePrivacySecurityChecklistComponent } from './site-privacy-security-checklist.component';

const routes: Routes = [
  {
    path: '',
    component: SitePrivacySecurityChecklistComponent,
    data: {
      title: 'Provider Identity Portal',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SitePrivacySecurityChecklistRoutingModule {}

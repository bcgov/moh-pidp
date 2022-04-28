import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

const routes: Routes = [
  {
    path: '',
    component: SitePrivacySecurityChecklistPage,
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SitePrivacySecurityChecklistRoutingModule {}

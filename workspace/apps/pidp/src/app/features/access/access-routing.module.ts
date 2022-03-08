import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccessRoutes } from './access.routes';
import { HcimWebEnrolmentModule } from './pages/hcim-reenrolment/hcim-reenrolment.module';
import { PharmanetModule } from './pages/pharmanet/pharmanet.module';
import { SitePrivacySecurityChecklistModule } from './pages/site-privacy-security-checklist/site-privacy-security-checklist.module';
import { SpecialAuthorityEformsModule } from './pages/special-authority-eforms/special-authority-eforms.module';

const routes: Routes = [
  {
    path: AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE,
    loadChildren: (): Promise<SpecialAuthorityEformsModule> =>
      import(
        './pages/special-authority-eforms/special-authority-eforms-routing.module'
      ).then((m) => m.SpecialAuthorityEformsRoutingModule),
  },
  {
    path: AccessRoutes.PHARMANET_PAGE,
    loadChildren: (): Promise<PharmanetModule> =>
      import('./pages/pharmanet/pharmanet.module').then(
        (m) => m.PharmanetModule
      ),
  },
  {
    path: AccessRoutes.SITE_PRIVACY_SECURITY_CHECKLIST_PAGE,
    loadChildren: (): Promise<SitePrivacySecurityChecklistModule> =>
      import(
        './pages/site-privacy-security-checklist/site-privacy-security-checklist.module'
      ).then((m) => m.SitePrivacySecurityChecklistModule),
  },
  {
    path: AccessRoutes.HCIM_REENROLMENT,
    loadChildren: (): Promise<HcimWebEnrolmentModule> =>
      import('./pages/hcim-reenrolment/hcim-reenrolment.module').then(
        (m) => m.HcimWebEnrolmentModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessRoutingModule {}

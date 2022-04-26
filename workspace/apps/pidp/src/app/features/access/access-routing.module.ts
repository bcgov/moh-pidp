import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from './access.routes';
import { HcimAccountTransferModule } from './pages/hcim-account-transfer/hcim-account-transfer.module';
import { HcimEnrolmentModule } from './pages/hcim-enrolment/hcim-enrolment.module';
import { PharmanetModule } from './pages/pharmanet/pharmanet.module';
import { SaEformsModule } from './pages/sa-eforms/sa-eforms.module';
import { SitePrivacySecurityChecklistModule } from './pages/site-privacy-security-checklist/site-privacy-security-checklist.module';

const routes: Routes = [
  {
    path: AccessRoutes.SPECIAL_AUTH_EFORMS_PAGE,
    loadChildren: (): Promise<SaEformsModule> =>
      import('./pages/sa-eforms/sa-eforms-routing.module').then(
        (m) => m.SaEformsRoutingModule
      ),
  },
  {
    path: AccessRoutes.HCIM_ACCOUNT_TRANSFER_PAGE,
    loadChildren: (): Promise<HcimAccountTransferModule> =>
      import('./pages/hcim-account-transfer/hcim-account-transfer.module').then(
        (m) => m.HcimAccountTransferModule
      ),
  },
  {
    path: AccessRoutes.HCIM_ENROLMENT_PAGE,
    loadChildren: (): Promise<HcimEnrolmentModule> =>
      import('./pages/hcim-enrolment/hcim-enrolment.module').then(
        (m) => m.HcimEnrolmentModule
      ),
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
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO, Role.FEATURE_AMH_DEMO],
    },
    loadChildren: (): Promise<SitePrivacySecurityChecklistModule> =>
      import(
        './pages/site-privacy-security-checklist/site-privacy-security-checklist.module'
      ).then((m) => m.SitePrivacySecurityChecklistModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessRoutingModule {}

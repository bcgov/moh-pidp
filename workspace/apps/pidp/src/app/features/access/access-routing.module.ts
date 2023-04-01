import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SetDashboardTitleGuard } from '@pidp/presentation';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from './access.routes';
import { BcProviderApplicationComponent } from './pages/bc-provider-application/bc-provider-application.component';
import { BcProviderApplicationResolver } from './pages/bc-provider-application/bc-provider-application.resolver';
import { BcProviderEditComponent } from './pages/bc-provider-edit/bc-provider-edit.component';
import { DriverFitnessModule } from './pages/driver-fitness/driver-fitness.module';
import { HcimAccountTransferModule } from './pages/hcim-account-transfer/hcim-account-transfer.module';
import { HcimEnrolmentModule } from './pages/hcim-enrolment/hcim-enrolment.module';
import { MsTeamsClinicMemberModule } from './pages/ms-teams-clinic-member/ms-teams-clinic-member.module';
import { MsTeamsPrivacyOfficerModule } from './pages/ms-teams-privacy-officer/ms-teams-privacy-officer.module';
import { PharmanetModule } from './pages/pharmanet/pharmanet.module';
import { PrescriptionRefillEformsModule } from './pages/prescription-refill-eforms/prescription-refill-eforms.module';
import { ProviderReportingPortalModule } from './pages/provider-reporting-portal/provider-reporting-portal.module';
import { SaEformsModule } from './pages/sa-eforms/sa-eforms.module';
import { SitePrivacySecurityChecklistModule } from './pages/site-privacy-security-checklist/site-privacy-security-checklist.module';

const routes: Routes = [
  {
    path: AccessRoutes.SPECIAL_AUTH_EFORMS,
    loadChildren: (): Promise<SaEformsModule> =>
      import('./pages/sa-eforms/sa-eforms-routing.module').then(
        (m) => m.SaEformsRoutingModule
      ),
  },
  {
    path: AccessRoutes.PRESCRIPTION_REFILL_EFORMS,
    loadChildren: (): Promise<PrescriptionRefillEformsModule> =>
      import(
        './pages/prescription-refill-eforms/prescription-refill-eforms.module'
      ).then((m) => m.PrescriptionRefillEformsModule),
  },
  {
    path: AccessRoutes.BC_PROVIDER_APPLICATION,
    resolve: {
      bcProviderApplicationStatusCode: BcProviderApplicationResolver,
    },
    canActivate: [SetDashboardTitleGuard],
    component: BcProviderApplicationComponent,
    data: {
      setDashboardTitleGuard: {
        titleText: 'BC Provider Application',
        titleDescriptionText: '',
      },
      roles: [Role.FEATURE_PIDP_DEMO],
    },
  },
  {
    path: AccessRoutes.BC_PROVIDER_APPLICATION_CHANGE_PASSWORD,
    canActivate: [SetDashboardTitleGuard],
    component: BcProviderEditComponent,
    data: {
      setDashboardTitleGuard: {
        titleText: 'BC Provider Application',
        titleDescriptionText: '',
      },
      roles: [Role.FEATURE_PIDP_DEMO],
    },
  },
  {
    path: AccessRoutes.HCIM_ACCOUNT_TRANSFER,
    loadChildren: (): Promise<HcimAccountTransferModule> =>
      import('./pages/hcim-account-transfer/hcim-account-transfer.module').then(
        (m) => m.HcimAccountTransferModule
      ),
  },
  {
    path: AccessRoutes.HCIM_ENROLMENT,
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<HcimEnrolmentModule> =>
      import('./pages/hcim-enrolment/hcim-enrolment.module').then(
        (m) => m.HcimEnrolmentModule
      ),
  },
  {
    path: AccessRoutes.PHARMANET,
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<PharmanetModule> =>
      import('./pages/pharmanet/pharmanet.module').then(
        (m) => m.PharmanetModule
      ),
  },
  {
    path: AccessRoutes.SITE_PRIVACY_SECURITY_CHECKLIST,
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<SitePrivacySecurityChecklistModule> =>
      import(
        './pages/site-privacy-security-checklist/site-privacy-security-checklist.module'
      ).then((m) => m.SitePrivacySecurityChecklistModule),
  },
  {
    path: AccessRoutes.DRIVER_FITNESS,
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<DriverFitnessModule> =>
      import('./pages/driver-fitness/driver-fitness.module').then(
        (m) => m.DriverFitnessModule
      ),
  },
  {
    path: AccessRoutes.MS_TEAMS_PRIVACY_OFFICER,
    canActivate: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<MsTeamsPrivacyOfficerModule> =>
      import(
        './pages/ms-teams-privacy-officer/ms-teams-privacy-officer.module'
      ).then((m) => m.MsTeamsPrivacyOfficerModule),
  },
  {
    path: AccessRoutes.PROVIDER_REPORTING_PORTAL,
    loadChildren: (): Promise<ProviderReportingPortalModule> =>
      import(
        './pages/provider-reporting-portal/provider-reporting-portal.module'
      ).then((m) => m.ProviderReportingPortalModule),
  },
  {
    path: AccessRoutes.MS_TEAMS_CLINIC_MEMBER,
    loadChildren: (): Promise<MsTeamsClinicMemberModule> =>
      import(
        './pages/ms-teams-clinic-member/ms-teams-clinic-member.module'
      ).then((m) => m.MsTeamsClinicMemberModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessRoutingModule {}

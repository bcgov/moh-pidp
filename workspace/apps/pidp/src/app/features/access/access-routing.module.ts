import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { AccessRoutes } from './access.routes';
import { BcProviderApplicationModule } from './pages/bc-provider-application/bc-provider-application.module';
import { BcProviderEditModule } from './pages/bc-provider-edit/bc-provider-edit.module';
import { DriverFitnessModule } from './pages/driver-fitness/driver-fitness.module';
import { HcimAccountTransferModule } from './pages/hcim-account-transfer/hcim-account-transfer.module';
import { HcimEnrolmentModule } from './pages/hcim-enrolment/hcim-enrolment.module';
import { ImmsBCEformsModule } from './pages/immsbc-eforms/immsbc-eforms.module';
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
    loadChildren: (): Promise<Type<SaEformsModule>> =>
      import('./pages/sa-eforms/sa-eforms-routing.module').then(
        (m) => m.SaEformsRoutingModule
      ),
  },
  {
    path: AccessRoutes.PRESCRIPTION_REFILL_EFORMS,
    loadChildren: (): Promise<Type<PrescriptionRefillEformsModule>> =>
      import(
        './pages/prescription-refill-eforms/prescription-refill-eforms.module'
      ).then((m) => m.PrescriptionRefillEformsModule),
  },
  {
    path: AccessRoutes.BC_PROVIDER_APPLICATION,
    loadChildren: (): Promise<Type<BcProviderApplicationModule>> =>
      import(
        './pages/bc-provider-application/bc-provider-application.module'
      ).then((m) => m.BcProviderApplicationModule),
  },
  {
    path: AccessRoutes.BC_PROVIDER_EDIT,
    loadChildren: (): Promise<Type<BcProviderEditModule>> =>
      import('./pages/bc-provider-edit/bc-provider-edit.module').then(
        (m) => m.BcProviderEditModule
      ),
  },
  {
    path: AccessRoutes.HCIM_ACCOUNT_TRANSFER,
    loadChildren: (): Promise<Type<HcimAccountTransferModule>> =>
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
    loadChildren: (): Promise<Type<HcimEnrolmentModule>> =>
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
    loadChildren: (): Promise<Type<PharmanetModule>> =>
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
    loadChildren: (): Promise<Type<SitePrivacySecurityChecklistModule>> =>
      import(
        './pages/site-privacy-security-checklist/site-privacy-security-checklist.module'
      ).then((m) => m.SitePrivacySecurityChecklistModule),
  },
  {
    path: AccessRoutes.DRIVER_FITNESS,
    loadChildren: (): Promise<Type<DriverFitnessModule>> =>
      import('./pages/driver-fitness/driver-fitness.module').then(
        (m) => m.DriverFitnessModule
      ),
  },
  {
    path: AccessRoutes.MS_TEAMS_PRIVACY_OFFICER,
    loadChildren: (): Promise<Type<MsTeamsPrivacyOfficerModule>> =>
      import(
        './pages/ms-teams-privacy-officer/ms-teams-privacy-officer.module'
      ).then((m) => m.MsTeamsPrivacyOfficerModule),
  },
  {
    path: AccessRoutes.PROVIDER_REPORTING_PORTAL,
    loadChildren: (): Promise<Type<ProviderReportingPortalModule>> =>
      import(
        './pages/provider-reporting-portal/provider-reporting-portal.module'
      ).then((m) => m.ProviderReportingPortalModule),
  },
  {
    path: AccessRoutes.MS_TEAMS_CLINIC_MEMBER,
    loadChildren: (): Promise<Type<MsTeamsClinicMemberModule>> =>
      import(
        './pages/ms-teams-clinic-member/ms-teams-clinic-member.module'
      ).then((m) => m.MsTeamsClinicMemberModule),
  },
  {
    path: AccessRoutes.IMMSBC_EFORMS,
    loadChildren: (): Promise<Type<ImmsBCEformsModule>> =>
      import('./pages/immsbc-eforms/immsbc-eforms.module').then(
        (m) => m.ImmsBCEformsModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessRoutingModule {}

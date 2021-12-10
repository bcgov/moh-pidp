import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from '../../modules/dashboard/components/dashboard/dashboard.component';
import { CollegeLicenceInformationRoutes } from '../college-licence-information/college-licence-information.routes';
import { GisRoutes } from '../gis/gis.routes';
import { PersonalInformationRoutes } from '../personal-information/personal-information.routes';
import { PharmanetRoutes } from '../pharmanet/pharmanet.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { SitePrivacySecurityChecklistRoutes } from '../site-privacy-security-checklist/site-privacy-security-checklist.routes';
import { SpecialAuthorityEformsRoutes } from '../special-authority-eforms/special-authority-eforms.routes';
import { TermsOfAccessAgreementRoutes } from '../terms-of-access-agreement/terms-of-access-agreement.routes';
import { WorkAndRoleInformationRoutes } from '../work-and-role-information/work-and-role-information.routes';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [],
    canActivateChild: [],
    children: [
      {
        path: PortalRoutes.MODULE_PATH,
        loadChildren: () =>
          import('../portal/portal.module').then((m) => m.PortalModule),
      },
      {
        path: PersonalInformationRoutes.MODULE_PATH,
        loadChildren: () =>
          import('../personal-information/personal-information.module').then(
            (m) => m.PersonalInformationModule
          ),
      },
      {
        path: CollegeLicenceInformationRoutes.MODULE_PATH,
        loadChildren: () =>
          import(
            '../college-licence-information/college-licence-information.module'
          ).then((m) => m.CollegeLicenceInformationModule),
      },
      {
        path: WorkAndRoleInformationRoutes.MODULE_PATH,
        loadChildren: () =>
          import(
            '../work-and-role-information/work-and-role-information.module'
          ).then((m) => m.WorkAndRoleInformationModule),
      },
      {
        path: TermsOfAccessAgreementRoutes.MODULE_PATH,
        loadChildren: () =>
          import(
            '../terms-of-access-agreement/terms-of-access-agreement.module'
          ).then((m) => m.TermsOfAccessAgreementModule),
      },
      {
        path: GisRoutes.MODULE_PATH,
        loadChildren: () =>
          import('../gis/gis.module').then((m) => m.GisModule),
      },
      {
        path: SpecialAuthorityEformsRoutes.MODULE_PATH,
        loadChildren: () =>
          import(
            '../special-authority-eforms/special-authority-eforms.module'
          ).then((m) => m.SpecialAuthorityEformsModule),
      },
      {
        path: PharmanetRoutes.MODULE_PATH,
        loadChildren: () =>
          import('../pharmanet/pharmanet.module').then(
            (m) => m.PharmanetModule
          ),
      },
      {
        path: SitePrivacySecurityChecklistRoutes.MODULE_PATH,
        loadChildren: () =>
          import(
            '../site-privacy-security-checklist/site-privacy-security-checklist.module'
          ).then((m) => m.SitePrivacySecurityChecklistModule),
      },
      {
        path: 'training',
        loadChildren: () =>
          import('../training/training.module').then((m) => m.TrainingModule),
      },
      {
        path: 'your-profile',
        loadChildren: () =>
          import('../your-profile/your-profile.module').then(
            (m) => m.YourProfileModule
          ),
      },
      {
        path: '',
        redirectTo: PortalRoutes.MODULE_PATH,
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShellRoutingModule {}

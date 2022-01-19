import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FeatureFlagGuard } from '@app/modules/feature-flag/feature-flag.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { CollegeLicenceInformationModule } from './pages/college-licence-information/college-licence-information.module';
import { PersonalInformationModule } from './pages/personal-information/personal-information.module';
import { UserAccessAgreementModule } from './pages/user-access-agreement/user-access-agreement.module';
import { WorkAndRoleInformationModule } from './pages/work-and-role-information/work-and-role-information.module';
import { ProfileRoutes } from './profile.routes';

const routes: Routes = [
  {
    path: ProfileRoutes.PERSONAL_INFO_PAGE,
    loadChildren: (): Promise<PersonalInformationModule> =>
      import('./pages/personal-information/personal-information.module').then(
        (m) => m.PersonalInformationModule
      ),
  },
  {
    path: ProfileRoutes.COLLEGE_LICENCE_INFO_PAGE,
    loadChildren: (): Promise<CollegeLicenceInformationModule> =>
      import(
        './pages/college-licence-information/college-licence-information.module'
      ).then((m) => m.CollegeLicenceInformationModule),
  },
  {
    path: ProfileRoutes.WORK_AND_ROLE_INFO_PAGE,
    canLoad: [FeatureFlagGuard],
    data: {
      features: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<WorkAndRoleInformationModule> =>
      import(
        './pages/work-and-role-information/work-and-role-information.module'
      ).then((m) => m.WorkAndRoleInformationModule),
  },
  {
    path: ProfileRoutes.USER_ACCESS_AGREEMENT_PAGE,
    canLoad: [FeatureFlagGuard],
    data: {
      features: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<UserAccessAgreementModule> =>
      import('./pages/user-access-agreement/user-access-agreement.module').then(
        (m) => m.UserAccessAgreementModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule {}

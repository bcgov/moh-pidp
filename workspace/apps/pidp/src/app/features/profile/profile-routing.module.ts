import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PermissionsGuard } from '@app/modules/permissions/permissions.guard';
import { Role } from '@app/shared/enums/roles.enum';

import { CollegeLicenceDeclarationModule } from './pages/college-licence/college-licence-declaration/college-licence-declaration.module';
import { CollegeLicenceInformationModule } from './pages/college-licence/college-licence-information/college-licence-information.module';
import { PersonalInformationModule } from './pages/personal-information/personal-information.module';
import { UserAccessAgreementModule } from './pages/user-access-agreement/user-access-agreement.module';
import { WorkAndRoleInformationModule } from './pages/work-and-role-information/work-and-role-information.module';
import { ProfileRoutes } from './profile.routes';

const routes: Routes = [
  {
    path: ProfileRoutes.PERSONAL_INFO,
    loadChildren: (): Promise<PersonalInformationModule> =>
      import('./pages/personal-information/personal-information.module').then(
        (m) => m.PersonalInformationModule
      ),
  },
  {
    path: ProfileRoutes.COLLEGE_LICENCE_DECLARATION,
    loadChildren: (): Promise<CollegeLicenceDeclarationModule> =>
      import(
        './pages/college-licence/college-licence-declaration/college-licence-declaration.module'
      ).then((m) => m.CollegeLicenceDeclarationModule),
  },
  {
    path: ProfileRoutes.WORK_AND_ROLE_INFO,
    canLoad: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<WorkAndRoleInformationModule> =>
      import(
        './pages/work-and-role-information/work-and-role-information.module'
      ).then((m) => m.WorkAndRoleInformationModule),
  },
  {
    path: ProfileRoutes.USER_ACCESS_AGREEMENT,
    canLoad: [PermissionsGuard],
    data: {
      roles: [Role.FEATURE_PIDP_DEMO],
    },
    loadChildren: (): Promise<UserAccessAgreementModule> =>
      import('./pages/user-access-agreement/user-access-agreement.module').then(
        (m) => m.UserAccessAgreementModule
      ),
  },
  {
    path: ProfileRoutes.COLLEGE_LICENCE_INFO,
    loadChildren: (): Promise<CollegeLicenceInformationModule> =>
      import(
        './pages/college-licence/college-licence-information/college-licence-information.module'
      ).then((m) => m.CollegeLicenceInformationModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule {}

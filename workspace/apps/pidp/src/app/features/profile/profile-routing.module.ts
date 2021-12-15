import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CollegeLicenceInformationModule } from './pages/college-licence-information/college-licence-information.module';
import { PersonalInformationModule } from './pages/personal-information/personal-information.module';
import { TermsOfAccessAgreementModule } from './pages/terms-of-access-agreement/terms-of-access-agreement.module';
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
    loadChildren: (): Promise<WorkAndRoleInformationModule> =>
      import(
        './pages/work-and-role-information/work-and-role-information.module'
      ).then((m) => m.WorkAndRoleInformationModule),
  },
  {
    path: ProfileRoutes.TERMS_OF_ACCESS_AGREEMENT_PAGE,
    loadChildren: (): Promise<TermsOfAccessAgreementModule> =>
      import(
        './pages/terms-of-access-agreement/terms-of-access-agreement.module'
      ).then((m) => m.TermsOfAccessAgreementModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule {}

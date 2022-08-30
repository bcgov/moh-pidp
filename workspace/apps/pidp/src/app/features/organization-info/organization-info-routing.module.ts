import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';
import { AdministratorInformationModule } from './pages/administrator-information/administrator-information.module';
import { FacilityDetailsModule } from './pages/facility-details/facility-details.module';
import { OrganizationDetailsModule } from './pages/organization-details/organization-details.module';

const routes: Routes = [
  {
    path: OrganizationInfoRoutes.ORGANIZATION_DETAILS,
    loadChildren: (): Promise<OrganizationDetailsModule> =>
      import('./pages/organization-details/organization-details.module').then(
        (m) => m.OrganizationDetailsModule
      ),
  },
  {
    path: OrganizationInfoRoutes.FACILITY_DETAILS,
    loadChildren: (): Promise<FacilityDetailsModule> =>
      import('./pages/facility-details/facility-details.module').then(
        (m) => m.FacilityDetailsModule
      ),
  },
  {
    path: OrganizationInfoRoutes.ADMINISTRATOR_INFO,
    loadChildren: (): Promise<AdministratorInformationModule> =>
      import(
        './pages/administrator-information/administrator-information.module'
      ).then((m) => m.AdministratorInformationModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationInfoRoutingModule {}

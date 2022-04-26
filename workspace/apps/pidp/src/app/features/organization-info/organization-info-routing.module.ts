import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';
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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationInfoRoutingModule {}

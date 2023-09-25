import { NgModule, Type } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';
import { AdministratorInformationModule } from './pages/administrator-information/administrator-information.module';
import { EndorsementsModule } from './pages/endorsements/endorsements.module';
import { FacilityDetailsModule } from './pages/facility-details/facility-details.module';
import { OrganizationDetailsModule } from './pages/organization-details/organization-details.module';

const routes: Routes = [
  {
    path: OrganizationInfoRoutes.ORGANIZATION_DETAILS,
    loadChildren: (): Promise<Type<OrganizationDetailsModule>> =>
      import('./pages/organization-details/organization-details.module').then(
        (m) => m.OrganizationDetailsModule
      ),
  },
  {
    path: OrganizationInfoRoutes.FACILITY_DETAILS,
    loadChildren: (): Promise<Type<FacilityDetailsModule>> =>
      import('./pages/facility-details/facility-details.module').then(
        (m) => m.FacilityDetailsModule
      ),
  },
  {
    path: OrganizationInfoRoutes.ADMINISTRATOR_INFO,
    loadChildren: (): Promise<Type<AdministratorInformationModule>> =>
      import(
        './pages/administrator-information/administrator-information.module'
      ).then((m) => m.AdministratorInformationModule),
  },
  {
    path: OrganizationInfoRoutes.ENDORSEMENTS,
    loadChildren: (): Promise<Type<EndorsementsModule>> =>
      import('./pages/endorsements/endorsements.module').then(
        (m) => m.EndorsementsModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationInfoRoutingModule {}

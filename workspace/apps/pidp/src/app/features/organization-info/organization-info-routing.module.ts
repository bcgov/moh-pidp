import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { OrganizationInfoRoutes } from './organization-info.routes';
import { AdministratorInformationModule } from './pages/administrator-information/administrator-information.module';
import { EndorsementRequestModule } from './pages/endorsement/pages/endorsement-request/endorsement-request.module';
import { EndorsementRequestsReceivedModule } from './pages/endorsement/pages/endorsement-requests-received/endorsement-requests-received.module';
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
  {
    path: OrganizationInfoRoutes.ENDORSEMENT_REQUEST,
    loadChildren: (): Promise<EndorsementRequestModule> =>
      import(
        './pages/endorsement/pages/endorsement-request/endorsement-request.module'
      ).then((m) => m.EndorsementRequestModule),
  },
  {
    path: OrganizationInfoRoutes.ENDORSEMENT_REQUESTS_RECEIVED,
    loadChildren: (): Promise<EndorsementRequestsReceivedModule> =>
      import(
        './pages/endorsement/pages/endorsement-requests-received/endorsement-requests-received.module'
      ).then((m) => m.EndorsementRequestsReceivedModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OrganizationInfoRoutingModule {}

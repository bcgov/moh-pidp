import { Routes } from '@angular/router';

import { ImmsbcPage } from './immsbc.page';
import { ImmsbcCreatePharmacyPage } from './immsbc-create-pharmacy.page';
import { ImmsbcManagePharmacyPage } from './immsbc-manage-pharmacy.page';
import { ImmsbcPharmacyEnrolmentPage } from './immsbc-pharmacy-enrolment.page';
import { ImmsbcClaimPharmacyPage } from './immsbc-claim-pharmacy.page';
import { ImmsbcRegisterPharmacyPage } from './immsbc-register-pharmacy.page';

import { immsbcResolver } from './immsbc.resolver';

export const routes: Routes = [
  {
    path: '',
    component: ImmsbcPage,
    resolve: {
      immsBCStatusCode: immsbcResolver,
    },
  },
  {
    path: 'create-pharmacy',
    component: ImmsbcCreatePharmacyPage,
  },
  {
    path: 'manage-pharmacy',
    component: ImmsbcManagePharmacyPage,
  },
  {
    path: 'pharmacy-enrol/:token',
    component: ImmsbcPharmacyEnrolmentPage,
  },
  {
    path: 'claim-pharmacy',
    component: ImmsbcClaimPharmacyPage,
  },
  {
    path: 'register-pharmacy',
    component: ImmsbcRegisterPharmacyPage,
  }
];

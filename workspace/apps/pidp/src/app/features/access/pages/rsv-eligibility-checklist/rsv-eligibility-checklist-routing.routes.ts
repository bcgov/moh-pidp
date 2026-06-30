import { Routes } from '@angular/router';

import { RsvEligibilityChecklistPage } from './rsv-eligibility-checklist.page';
import { rsvEligibilityChecklistResolver } from './rsv-eligibility-checklist.resolver';

export const routes: Routes = [
  {
    path: '',
    component: RsvEligibilityChecklistPage,
    resolve: {
      rsvEligibilityChecklistStatusCode: rsvEligibilityChecklistResolver,
    },
  },
];

import { Routes } from '@angular/router';

import { NpdpExceptionalCoveragePage } from './npdp-exceptional-coverage.page';
import { npdpExceptionalCoverageResolver } from './npdp-exceptional-coverage.resolver';

export const routes: Routes = [
  {
    path: '',
    component: NpdpExceptionalCoveragePage,
    resolve: {
      npdpExceptionalCoverageStatusCode: npdpExceptionalCoverageResolver,
    },
  },
];

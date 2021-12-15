import { PortalModule } from '@angular/cdk/portal';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from '../../modules/dashboard/components/dashboard/dashboard.component';
import { AccessModule } from '../access/access.module';
import { AccessRoutes } from '../access/access.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { ProfileModule } from '../profile/profile.module';
import { ProfileRoutes } from '../profile/profile.routes';
import { TrainingModule } from '../training/training.module';
import { TrainingRoutes } from '../training/training.routes';
import { YourProfileModule } from '../your-profile/your-profile.module';
import { YourProfileRoutes } from '../your-profile/your-profile.routes';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [],
    canActivateChild: [],
    children: [
      {
        path: PortalRoutes.MODULE_PATH,
        loadChildren: (): Promise<PortalModule> =>
          import('../portal/portal.module').then((m) => m.PortalModule),
      },
      {
        path: ProfileRoutes.MODULE_PATH,
        loadChildren: (): Promise<ProfileModule> =>
          import('../profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: AccessRoutes.MODULE_PATH,
        loadChildren: (): Promise<AccessModule> =>
          import('../access/access.module').then((m) => m.AccessModule),
      },
      {
        path: TrainingRoutes.MODULE_PATH,
        loadChildren: (): Promise<TrainingModule> =>
          import('../training/training.module').then((m) => m.TrainingModule),
      },
      {
        path: YourProfileRoutes.MODULE_PATH,
        loadChildren: (): Promise<YourProfileModule> =>
          import('../your-profile/your-profile.module').then(
            (m) => m.YourProfileModule
          ),
      },
      {
        path: '',
        redirectTo: PortalRoutes.MODULE_PATH,
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ShellRoutingModule {}

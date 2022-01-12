import { PortalModule } from '@angular/cdk/portal';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from '@bcgov/shared/ui';

import { AccessModule } from '../access/access.module';
import { AccessRoutes } from '../access/access.routes';
import { AdminModule } from '../admin/admin.module';
import { AdminRoutes } from '../admin/admin.routes';
import { AuthModule } from '../auth/auth.module';
import { AuthRoutes } from '../auth/auth.routes';
import { PortalRoutes } from '../portal/portal.routes';
import { ProfileModule } from '../profile/profile.module';
import { ProfileRoutes } from '../profile/profile.routes';
import { TrainingModule } from '../training/training.module';
import { TrainingRoutes } from '../training/training.routes';
import { YourProfileModule } from '../your-profile/your-profile.module';
import { YourProfileRoutes } from '../your-profile/your-profile.routes';

const routes: Routes = [
  {
    path: AuthRoutes.MODULE_PATH,
    loadChildren: (): Promise<AuthModule> =>
      import('../auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: AdminRoutes.MODULE_PATH,
    loadChildren: (): Promise<AdminModule> =>
      import('../admin/admin.module').then((m) => m.AdminModule),
  },
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

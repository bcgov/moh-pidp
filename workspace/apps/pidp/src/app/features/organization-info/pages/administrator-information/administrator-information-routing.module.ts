import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdministratorInformationComponent } from './administrator-information.component';

const routes: Routes = [
  {
    path: '',
    component: AdministratorInformationComponent,
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministratorInformationRoutingModule {}

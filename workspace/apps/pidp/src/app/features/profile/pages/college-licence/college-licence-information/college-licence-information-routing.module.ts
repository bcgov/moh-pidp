import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CollegeLicenceInformationV2Page } from './college-licence-information-v2.page';
import { CollegeLicenceInformationPage } from './college-licence-information.page';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceInformationPage,
    data: {
      title: 'Provider Identity Portal',
      routes: {
        root: '../../',
      },
    },
  },
  {
    path: 'v2',
    component: CollegeLicenceInformationV2Page,
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
export class CollegeLicenceInformationRoutingModule {}

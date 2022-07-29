import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanDeactivateFormGuard } from '@app/core/guards/can-deactivate-form.guard';

import { CollegeLicenceDeclarationPage } from './college-licence-declaration.page';

const routes: Routes = [
  {
    path: '',
    component: CollegeLicenceDeclarationPage,
    canDeactivate: [CanDeactivateFormGuard],
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
export class CollegeLicenceDeclarationRoutingModule {}

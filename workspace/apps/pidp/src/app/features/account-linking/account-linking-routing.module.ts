import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccountLinkingHomePage } from './pages/home/home.page';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: AccountLinkingHomePage,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountLinkingRoutingModule {}

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrganizationDetailsComponent } from './organization-details.component';

const routes: Routes = [{ path: '', component: OrganizationDetailsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrganizationDetailsRoutingModule { }

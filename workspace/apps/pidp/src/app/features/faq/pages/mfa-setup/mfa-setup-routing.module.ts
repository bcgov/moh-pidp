import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MfaSetupComponent } from './mfa-setup.component';

const routes: Routes = [{ path: '', component: MfaSetupComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MfaSetupRoutingModule { }

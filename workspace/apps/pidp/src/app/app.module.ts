import { NgModule } from '@angular/core';

import { RootRoutingModule } from '@bcgov/shared/ui';

import { AppRoutingModule } from '@app/app-routing.module';
import { AppComponent } from '@app/app.component';

import { CoreModule } from '@core/core.module';

import { LookupModule } from './modules/lookup/lookup.module';

@NgModule({
  declarations: [AppComponent],
  imports: [CoreModule, LookupModule, AppRoutingModule, RootRoutingModule],
  bootstrap: [AppComponent],
})
export class AppModule {}

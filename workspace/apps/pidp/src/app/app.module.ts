import { NgModule } from '@angular/core';

import { RootRoutingModule } from '@bcgov/shared/ui';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { ConfigModule } from './modules/lookup/lookup.module';

@NgModule({
  declarations: [AppComponent],
  imports: [CoreModule, ConfigModule, AppRoutingModule, RootRoutingModule],
  bootstrap: [AppComponent],
})
export class AppModule {}

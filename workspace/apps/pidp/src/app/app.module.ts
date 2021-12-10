import { NgModule } from '@angular/core';

import { RootRoutingModule } from '@bcgov/shared/ui';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';

@NgModule({
  declarations: [AppComponent],
  imports: [CoreModule, AppRoutingModule, RootRoutingModule],
  bootstrap: [AppComponent],
})
export class AppModule {}

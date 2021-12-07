import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { RootRoutingModule } from './modules/root-routing/root-routing.module';

@NgModule({
  declarations: [AppComponent],
  imports: [CoreModule, AppRoutingModule, RootRoutingModule],
  bootstrap: [AppComponent],
})
export class AppModule {}

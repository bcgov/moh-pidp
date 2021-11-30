import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { SharedModule } from './shared/shared.module';
import { UtilsModule } from './libs/utils/utils.module';
import { UiModule } from './libs/ui/ui.module';
import { DataAccessModule } from './libs/data-access/data-access.module';
import { ConfigModule } from './libs/config/config.module';
import { CoreModule } from './core/core.module';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppConfigModule } from './app-config.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    DataAccessModule,
    UtilsModule,
    UiModule,
    ConfigModule,
    CoreModule,
    SharedModule,
    AppRoutingModule,
    AppConfigModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}

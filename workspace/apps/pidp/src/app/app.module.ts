import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { DataAccessModule } from './libs/data-access/data-access.module';
import { UiModule } from './libs/ui/ui.module';
import { UtilsModule } from './libs/utils/utils.module';
import { ConfigModule } from './libs/config/config.module';

import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';

import { AppComponent } from './app.component';
import { AppConfigModule } from './app-config.module';
import { AppRoutingModule } from './app-routing.module';

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

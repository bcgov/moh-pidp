import { NgModule } from '@angular/core';

import { PidpDataModelModule } from '@pidp/data-model';
import { PidpPresentationModule } from '@pidp/presentation';

import { AppRoutingModule } from '@app/app-routing.module';
import { AppComponent } from '@app/app.component';

import { CoreModule } from '@core/core.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    AppRoutingModule,
    CoreModule,
    // Import PIDP modules in AppModule so that singleton services are injected at the top of the module hierarchy.
    PidpDataModelModule,
    PidpPresentationModule,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

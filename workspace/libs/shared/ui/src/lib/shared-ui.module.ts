import { NgModule } from '@angular/core';
import { MaterialModule } from './material/material.module';

import { AlertComponent } from './components/alert/alert.component';
import { PageComponent } from './components/page/page.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageSectionComponent } from './components/page-section/page-section.component';
import { PageSubheaderComponent } from './components/page-subheader/page-subheader.component';

@NgModule({
  declarations: [
    AlertComponent,
    PageComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
  ],
  imports: [MaterialModule],
  exports: [
    MaterialModule,
    AlertComponent,
    PageComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
  ],
})
export class SharedUiModule {}

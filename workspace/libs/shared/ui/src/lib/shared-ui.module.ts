import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from './material/material.module';

import { AlertComponent } from './components/alert/alert.component';
import { PageComponent } from './components/page/page.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageSectionComponent } from './components/page-section/page-section.component';
import { PageSubheaderComponent } from './components/page-subheader/page-subheader.component';
import { CardSummaryComponent } from './components/card-summary/card-summary.component';
import { CollectionNoticeComponent } from './components/collection-notice/collection-notice.component';

@NgModule({
  declarations: [
    AlertComponent,
    PageComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
    CardSummaryComponent,
    CollectionNoticeComponent,
  ],
  imports: [CommonModule, MaterialModule],
  exports: [
    MaterialModule,
    AlertComponent,
    PageComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
    CardSummaryComponent,
    CollectionNoticeComponent,
  ],
})
export class SharedUiModule {}

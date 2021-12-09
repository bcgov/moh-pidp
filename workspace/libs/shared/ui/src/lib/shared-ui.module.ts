import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from './material/material.module';

import { AlertComponent } from './components/alert/alert.component';
import { CardSummaryComponent } from './components/card-summary/card-summary.component';
import { CollectionNoticeComponent } from './components/collection-notice/collection-notice.component';
import { IconComponent } from './components/icon/icon.component';
import { PageComponent } from './components/page/page.component';
import { PageFooterComponent } from './components/page-footer/page-footer.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageSectionComponent } from './components/page-section/page-section.component';
import { PageSubheaderComponent } from './components/page-subheader/page-subheader.component';

@NgModule({
  declarations: [
    AlertComponent,
    CardSummaryComponent,
    CollectionNoticeComponent,
    IconComponent,
    PageComponent,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
  ],
  imports: [CommonModule, MaterialModule],
  exports: [
    MaterialModule,
    AlertComponent,
    CardSummaryComponent,
    CollectionNoticeComponent,
    IconComponent,
    PageComponent,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSubheaderComponent,
  ],
})
export class SharedUiModule {}

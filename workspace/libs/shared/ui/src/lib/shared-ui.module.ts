import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material/material.module';

import { NgxMaskModule } from 'ngx-mask';

import { AlertComponent } from './components/alert/alert.component';
import { AlertContentDirective } from './components/alert/alert-content.directive';
import { AlertActionsDirective } from './components/alert/alert-actions.directive';
import { CardComponent } from './components/card/card.component';
import { CardSummaryComponent } from './components/card-summary/card-summary.component';
import { CollectionNoticeComponent } from './components/collection-notice/collection-notice.component';
import { ContactFormComponent } from './components/contact-info-form/contact-info-form.component';
import { CardHintDirective } from './components/card/card-hint.directive';
import { CardContentDirective } from './components/card/card-content.directive';
import { CardActionsDirective } from './components/card/card-actions.directive';
import { FormSectionComponent } from './components/form-section/form-section.component';
import { IconComponent } from './components/icon/icon.component';
import { KeyValueInfoComponent } from './components/key-value-info/key-value-info.component';
import { PageComponent } from './components/page/page.component';
import { PageFooterComponent } from './components/page-footer/page-footer.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageSectionComponent } from './components/page-section/page-section.component';
import { PageSectionSubheaderComponent } from './components/page-section-subheader/page-section-subheader.component';
import { PageSectionSubheaderDescDirective } from './components/page-section-subheader/page-section-subheader-desc.directive';
import { PageSectionSubheaderHintDirective } from './components/page-section-subheader/page-section-subheader-hint.directive';
import { PageSubheaderComponent } from './components/page-subheader/page-subheader.component';
import { PreferredNameFormComponent } from './components/preferred-name-form/preferred-name-form.component';
import { ToggleContentComponent } from './components/toggle-content/toggle-content.component';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { YesNoContentComponent } from './components/yes-no-content/yes-no-content.component';
import { ContextHelpModule } from './modules/context-help/context-help.module';
import { DefaultPipe } from './pipes/default.pipe';
import { FormatDatePipe } from './pipes/format-date.pipe';
import { FullnamePipe } from './pipes/fullname.pipe';
import { PostalPipe } from './pipes/postal.pipe';

@NgModule({
  declarations: [
    AlertComponent,
    AlertContentDirective,
    AlertActionsDirective,
    CardComponent,
    CardHintDirective,
    CardContentDirective,
    CardActionsDirective,
    CardSummaryComponent,
    CollectionNoticeComponent,
    ContactFormComponent,
    FormSectionComponent,
    KeyValueInfoComponent,
    IconComponent,
    PageComponent,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    PageSectionSubheaderHintDirective,
    PageSubheaderComponent,
    PreferredNameFormComponent,
    ToggleContentComponent,
    UserInfoComponent,
    YesNoContentComponent,
    DefaultPipe,
    FormatDatePipe,
    FullnamePipe,
    PostalPipe,
  ],
  imports: [
    CommonModule,
    ContextHelpModule,
    MaterialModule,
    NgxMaskModule.forRoot(),
    ReactiveFormsModule,
  ],
  exports: [
    ContextHelpModule,
    MaterialModule,
    NgxMaskModule,
    ReactiveFormsModule,
    AlertComponent,
    AlertContentDirective,
    AlertActionsDirective,
    CardComponent,
    CardHintDirective,
    CardContentDirective,
    CardActionsDirective,
    CardSummaryComponent,
    CollectionNoticeComponent,
    ContactFormComponent,
    FormSectionComponent,
    KeyValueInfoComponent,
    IconComponent,
    PageComponent,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    PageSectionSubheaderHintDirective,
    PageSubheaderComponent,
    PreferredNameFormComponent,
    ToggleContentComponent,
    UserInfoComponent,
    YesNoContentComponent,
    DefaultPipe,
    FormatDatePipe,
    FullnamePipe,
    PostalPipe,
  ],
})
export class SharedUiModule {}

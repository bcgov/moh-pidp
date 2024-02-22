import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterLink } from '@angular/router';

import {
  AlertComponent,
  AlertContentDirective,
  ContextHelpComponent,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
} from '@bcgov/shared/ui';

import { InfoGraphicComponent } from '../../shared/components/info-graphic/info-graphic.component';

@Component({
  selector: 'app-next-steps',
  templateUrl: './next-steps.page.html',
  styleUrls: ['./next-steps.page.scss'],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    ContextHelpComponent,
    InfoGraphicComponent,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    RouterLink,
  ],
})
export class NextStepsPage {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    return;
  }
}

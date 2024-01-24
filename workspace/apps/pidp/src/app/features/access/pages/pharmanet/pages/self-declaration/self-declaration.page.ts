import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, RouterLink } from '@angular/router';

import {
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  YesNoContentComponent,
} from '@bcgov/shared/ui';

import { selfDeclarationQuestions } from './self-declaration-questions';
import { SelfDeclarationType } from './self-declaration.enum';

@Component({
  selector: 'app-self-declaration',
  templateUrl: './self-declaration.page.html',
  styleUrls: ['./self-declaration.page.scss'],
  standalone: true,
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    RouterLink,
    YesNoContentComponent,
  ],
})
export class SelfDeclarationPage {
  public title: string;

  public SelfDeclarationType = SelfDeclarationType;
  public selfDeclarationQuestions = selfDeclarationQuestions;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }
}

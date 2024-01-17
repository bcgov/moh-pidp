import { NgIf } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  QueryList,
} from '@angular/core';

import { IconType } from '../icon/icon.component';
import { PageSubheaderComponent } from '../page-subheader/page-subheader.component';
import { PageSectionSubheaderDescDirective } from './page-section-subheader-desc.directive';
import { PageSectionSubheaderHintDirective } from './page-section-subheader-hint.directive';

@Component({
  selector: 'ui-page-section-subheader',
  templateUrl: './page-section-subheader.component.html',
  styleUrls: ['./page-section-subheader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [PageSubheaderComponent, NgIf],
})
export class PageSectionSubheaderComponent {
  @Input() public icon?: string;
  @Input() public iconType?: IconType;
  @Input() public heading!: string;

  @ContentChildren(PageSectionSubheaderDescDirective, { descendants: true })
  public descriptions: QueryList<PageSectionSubheaderDescDirective>;

  @ContentChildren(PageSectionSubheaderHintDirective, { descendants: true })
  public hints: QueryList<PageSectionSubheaderHintDirective>;

  public constructor() {
    this.descriptions = new QueryList();
    this.hints = new QueryList();
  }
}

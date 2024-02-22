import {
  ChangeDetectionStrategy,
  Component,
  ContentChildren,
  Input,
  QueryList,
} from '@angular/core';

import { PageFooterActionDirective } from './page-footer-action.directive';

@Component({
  selector: 'ui-page-footer',
  templateUrl: './page-footer.component.html',
  styleUrls: ['./page-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
})
export class PageFooterComponent {
  @Input() public mode: 'normal' | 'reverse';

  @ContentChildren(PageFooterActionDirective, { descendants: true })
  public actions: QueryList<PageFooterActionDirective>;

  public constructor() {
    this.mode = 'normal';
    this.actions = new QueryList();
  }
}

import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'ui-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageComponent {
  /**
   * @description
   * Contraints applied to the maximum width of the
   * view content container to improve reability.
   *
   * "Page" provides readability for a single column of text within
   * a view, and enough room for 2 column forms. While the other sizes
   * provide a bit of flexibility in width when needed for views that
   * are not typical pages with forms found within a specific workflow.
   */
  @Input() public mode: 'page' | 'medium' | 'large' | 'full';

  public constructor() {
    this.mode = 'page';
  }
}

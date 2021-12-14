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
   */
  @Input() public mode: 'page' | 'moderate' | 'full';

  public constructor() {
    this.mode = 'page';
  }
}

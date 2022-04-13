import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ui-anchor',
  template: `
    <a #link uiAnchor [scheme]="scheme" [attr.href]="href">
      <ng-content></ng-content>
    </a>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnchorComponent {
  /**
   * @description
   * Type of URL scheme.
   * @default url
   */
  @Input() public scheme: 'url' | 'mailto' | 'tel' | 'scroll';
  /**
   * @description
   * URL scheme that the hyperlink points.
   * @default content when not projected
   */
  @Input() public href!: string;

  public constructor() {
    this.scheme = 'url';
  }
}

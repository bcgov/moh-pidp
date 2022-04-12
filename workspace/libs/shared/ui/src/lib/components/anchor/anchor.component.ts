import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';

@Component({
  selector: 'ui-anchor',
  template: `
    <a #link uiAnchor [scheme]="scheme" [attr.href]="hrefPrefix + href">
      <ng-content></ng-content>
    </a>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnchorComponent implements OnInit {
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

  public hrefPrefix: string;

  public constructor() {
    this.scheme = 'url';
    this.hrefPrefix = '';
  }

  public ngOnInit(): void {
    if (this.scheme === 'scroll') {
      this.hrefPrefix = '#';
    } else if (this.scheme !== 'url') {
      const suffix = this.scheme === 'tel' ? '+1' : '';
      this.hrefPrefix = `${this.scheme}:${suffix}`;
    }
  }
}

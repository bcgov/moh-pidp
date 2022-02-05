import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'ui-anchor',
  template: `
    <a #link [attr.href]="hrefPrefix + href">
      <ng-content></ng-content>
    </a>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AnchorComponent implements OnInit, AfterViewInit {
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
  /**
   * @description
   * Anchor URL target.
   * @default _blank
   */
  @Input() public target: '_blank' | '_self';

  @ViewChild('link') public link!: ElementRef;

  public hrefPrefix: string;

  public constructor() {
    this.scheme = 'url';
    this.hrefPrefix = '';
    this.target = '_blank';
  }

  public ngOnInit(): void {
    if (this.scheme === 'scroll') {
      this.hrefPrefix = '#';
    } else if (this.scheme !== 'url') {
      this.hrefPrefix = `${this.scheme}:`;
    }
  }

  public ngAfterViewInit(): void {
    if (this.scheme === 'url') {
      this.link.nativeElement.setAttribute('target', this.target);
      this.link.nativeElement.setAttribute('rel', 'noopener noreferrer');
    }

    if (!this.link.nativeElement.innerText.trim().length) {
      this.link.nativeElement.innerText = this.href;
    }
  }
}

import { Directive, ElementRef, Input, OnInit, inject } from '@angular/core';

import { PhonePipe } from '../../pipes';

@Directive({
  selector: '[uiAnchor]',
  providers: [PhonePipe],
  standalone: true,
})
export class AnchorDirective implements OnInit {
  private readonly el = inject<ElementRef<HTMLAnchorElement>>(ElementRef);
  private readonly phonePipe = inject(PhonePipe);

  /**
   * @description
   * Type of URL scheme.
   * @default url
   */
  @Input() public scheme: 'url' | 'mailto' | 'tel' | 'scroll';

  private readonly countryCode: number;

  public constructor() {
    this.scheme = 'url';
    this.countryCode = 1;
  }

  public ngOnInit(): void {
    const nativeElement = this.el.nativeElement;
    const href = nativeElement.getAttribute('href');

    if (!href) {
      return;
    }

    let value = href;

    switch (this.scheme) {
      case 'url':
        nativeElement.setAttribute('target', '_blank');
        nativeElement.setAttribute('rel', 'noopener noreferrer');
        break;
      case 'tel':
        nativeElement.setAttribute('href', `tel:+${this.countryCode}${href}`);
        value = this.phonePipe.transform(href);
        break;
      case 'mailto':
        nativeElement.setAttribute('href', `mailto:${href}`);
        break;
      case 'scroll':
        nativeElement.setAttribute('href', `#${href}`);
        break;
    }

    // Provide a display value when none exists
    if (!nativeElement.innerText?.trim()?.length && this.scheme !== 'scroll') {
      nativeElement.innerText = value;
    }
  }
}

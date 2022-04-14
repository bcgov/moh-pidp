import { Directive, ElementRef, Input, OnInit } from '@angular/core';

import { PhonePipe } from '../../pipes';

@Directive({
  selector: '[uiAnchor]',
  providers: [PhonePipe],
})
export class AnchorDirective implements OnInit {
  /**
   * @description
   * Type of URL scheme.
   * @default url
   */
  @Input() public scheme!: 'url' | 'mailto' | 'tel' | 'scroll';

  public constructor(private el: ElementRef, private phonePipe: PhonePipe) {}

  public ngOnInit(): void {
    const nativeElement = this.el.nativeElement as HTMLAnchorElement;
    const href = nativeElement.getAttribute('href') as string;
    let value = href;

    switch (this.scheme) {
      case 'url':
        nativeElement.setAttribute('target', '_blank');
        nativeElement.setAttribute('rel', 'noopener noreferrer');
        break;
      case 'tel':
        nativeElement.setAttribute('href', `tel:+1${href}`);
        value = this.phonePipe.transform(href);
        break;
      case 'scroll':
        nativeElement.setAttribute('href', `#${href}`);
        break;
    }

    // Provide a display value when none exists
    if (!nativeElement.innerText.trim().length && this.scheme !== 'scroll') {
      nativeElement.innerText = value;
    }
  }
}

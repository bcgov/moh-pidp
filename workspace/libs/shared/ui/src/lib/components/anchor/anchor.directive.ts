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
    const nativeElement = this.el.nativeElement;

    if (this.scheme === 'url') {
      nativeElement.setAttribute('target', '_blank');
      nativeElement.setAttribute('rel', 'noopener noreferrer');
    } else if (
      this.scheme === 'tel' &&
      !nativeElement.innerText.trim().length
    ) {
      const href = nativeElement.getAttribute('href');
      nativeElement.innerText = this.phonePipe.transform(href);
    }
  }
}

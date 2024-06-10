import { Directive, ElementRef, OnInit } from '@angular/core';

@Directive({
  selector: '[uiBreadcrumbButton]',
  standalone: true,
})
export class BreadcrumbButtonDirective implements OnInit {
  public constructor(private el: ElementRef<HTMLButtonElement>) {}

  public ngOnInit(): void {
    const nativeElement = this.el.nativeElement;
    const scss = `
        background: none;
        border: none;
        padding: 0;
        text-decoration: none;
        color: #1a5a96;
        font-size: 12px;
        font-weight: 400;
        line-height: 27px;
        word-wrap: break-word;
        `;
    nativeElement.setAttribute('style', scss);
  }
}

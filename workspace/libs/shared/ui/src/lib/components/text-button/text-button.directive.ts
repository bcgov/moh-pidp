import { Directive, ElementRef, OnInit, inject } from '@angular/core';

@Directive({
  selector: '[uiTextButton]',
  standalone: true,
})
export class TextButtonDirective implements OnInit {
  private readonly el = inject<ElementRef<HTMLButtonElement>>(ElementRef);


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

import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[appViewDocument]',
  standalone: true,
})
export class ViewDocumentDirective {
  public readonly viewContainerRef = inject(ViewContainerRef);

}

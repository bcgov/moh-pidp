import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({
  selector: '[appViewDocument]',
  standalone: true,
})
export class ViewDocumentDirective {
  private viewContainerRef = inject(ViewContainerRef);

}

import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appViewDocument]',
  standalone: true,
})
export class ViewDocumentDirective {
  public constructor(public viewContainerRef: ViewContainerRef) {}
}

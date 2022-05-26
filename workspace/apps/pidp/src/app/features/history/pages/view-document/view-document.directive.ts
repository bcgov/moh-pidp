import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appViewDocument]',
})
export class ViewDocumentDirective {
  public constructor(public viewContainerRef: ViewContainerRef) {}
}

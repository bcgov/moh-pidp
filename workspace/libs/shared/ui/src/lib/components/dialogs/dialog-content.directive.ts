import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appDialogContent]',
})
export class DialogContentDirective {
  public constructor(public viewContainerRef: ViewContainerRef) {}
}

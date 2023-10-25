import { Directive, ElementRef, HostListener, Optional } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[uiTrim]',
  exportAs: 'uiTrim',
})
export class TrimDirective {
  public constructor(
    private el: ElementRef<HTMLInputElement>,
    @Optional() private ngControl: NgControl,
  ) {}

  @HostListener('blur') public onBlur(): void {
    if (this.ngControl?.control) {
      this.ngControl.control.setValue(this.el.nativeElement.value.trim());
    } else {
      this.el.nativeElement.value = this.el.nativeElement.value.trim();
    }
  }
}

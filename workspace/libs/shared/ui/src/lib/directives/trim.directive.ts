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
    let value: string | null = this.el.nativeElement.value.trim();
    if (this.ngControl?.control) {
      value = value === '' ? null : value;
      this.ngControl.control.setValue(value);
    } else {
      this.el.nativeElement.value = value;
    }
  }
}

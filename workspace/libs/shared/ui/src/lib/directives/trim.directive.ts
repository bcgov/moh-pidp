import { Directive, ElementRef, HostListener, Optional } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[uiTrim]',
  exportAs: 'uiTrim',
  standalone: true,
})
export class TrimDirective {
  public constructor(
    private readonly el: ElementRef<HTMLInputElement>,
    @Optional() private readonly ngControl: NgControl,
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

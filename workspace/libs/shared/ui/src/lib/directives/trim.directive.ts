import { Directive, ElementRef, HostListener, inject } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: 'input[uiTrim]',
  exportAs: 'uiTrim',
  standalone: true,
})
export class TrimDirective {
  private readonly el = inject<ElementRef<HTMLInputElement>>(ElementRef);
  private readonly ngControl = inject(NgControl, { optional: true });


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

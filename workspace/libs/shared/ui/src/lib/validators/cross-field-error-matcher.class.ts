import { AbstractControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

export class CrossFieldErrorMatcher implements ErrorStateMatcher {
  public isErrorState(
    control: AbstractControl | null,
    form: FormGroupDirective | NgForm | null,
  ): boolean {
    return (
      ((control?.dirty && control.invalid) ||
        (control?.dirty && form?.hasError('nomatch'))) ??
      false
    );
  }
}

import {
  AbstractControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

export class FormGroupValidators {
  /**
   * @description
   * Checks two form control values are equal within a form group.
   */
  public static match(inputKey: string, confirmInputKey: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      if (!(group instanceof FormGroup)) {
        throw Error('Invalid AbstractControl expect FormGroup');
      }

      const input = group.controls[inputKey];
      const confirmInput = group.controls[confirmInputKey];
      if (!input || !confirmInput) {
        return null;
      }
      const valid = input.value === confirmInput.value;
      return valid ? null : { nomatch: true };
    };
  }

  /**
   * @description
   * Checks that at least one field has been chosen within a form group.
   */
  public static atLeastOne(
    validator: ValidatorFn = Validators.required,
    allowlist: string[] = [],
  ): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      if (!(group instanceof FormGroup)) {
        throw Error('Invalid AbstractControl expect FormGroup');
      }

      const atLeastOne =
        group &&
        group.controls &&
        Object.keys(group.controls)
          .filter((key) => allowlist.indexOf(key) !== -1)
          .some((key) => validator(group.controls[key]) === null);
      return atLeastOne ? null : { atleastone: true };
    };
  }

  /**
   * @description
   * Checks that the start key value is less than end key value.
   */
  public static lessThan(startKey: string, endKey: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      if (!(group instanceof FormGroup)) {
        throw Error('Invalid AbstractControl expect FormGroup');
      }

      const start = +group.controls[startKey].value;
      const end = +group.controls[endKey].value;
      if (!start || !end) {
        return null;
      }
      const valid = start < end;
      return valid ? null : { lessthan: true };
    };
  }
}

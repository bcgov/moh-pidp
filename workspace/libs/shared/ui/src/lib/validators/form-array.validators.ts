import {
  AbstractControl,
  FormArray,
  ValidationErrors,
  ValidatorFn,
} from '@angular/forms';

export class FormArrayValidators {
  /**
   * @description
   * Checks that at least # of abstract control(s) in a form array exist
   * based on a predicate. For example, useful for a list of checkbox form
   * controls, but can be extended using a custom predicate.
   */
  public static atLeast(
    minNumber: number,
    predicate: (control: AbstractControl) => boolean = (
      control: AbstractControl,
    ): boolean => !!control,
  ): ValidatorFn {
    return (array: AbstractControl): ValidationErrors | null => {
      if (!(array instanceof FormArray)) {
        throw Error('Invalid AbstractControl expect FormArray');
      }

      const atLeast =
        array &&
        array.controls?.length &&
        array.controls.filter(predicate).length >= minNumber;
      return atLeast ? null : { atleast: true };
    };
  }
}

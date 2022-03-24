import { FormGroup } from '@angular/forms';

/**
 * @description
 * Class is used to encapsulate form functionality, and
 * adaptation between the business model and form model.
 */
export abstract class AbstractFormState<T> {
  protected formInstance!: FormGroup;

  /**
   * @description
   * Get the reactive form instance.
   */
  public get form(): FormGroup {
    return this.formInstance;
  }

  /**
   * @description
   * Get the reactive form as JSON.
   */
  public abstract get json(): T | undefined;

  /**
   * @description
   * Patch the reactive form with data.
   *
   * NOTE: "options" is provided to allow for additional
   * information to be passed into the method, but should
   * only be used when absolutely necessary.
   */
  public abstract patchValue(model: T, options?: unknown): void;

  /**
   * @description
   * Build the reactive form structure.
   */
  public abstract buildForm(): void;
}

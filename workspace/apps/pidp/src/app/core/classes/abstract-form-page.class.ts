import { FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UrlTree } from '@angular/router';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { AbstractFormState, ConfirmDialogComponent } from '@bcgov/shared/ui';

import { FormUtilsService } from '@core/services/form-utils.service';

export interface IFormPage {
  /**
   * @description
   * Instance of the form state which provides access to
   * an API for the form.
   */
  formState: AbstractFormState<unknown>;

  /**
   * @description
   * Handle submission of forms.
   */
  onSubmit(): void;

  /**
   * @description
   * Handle redirection from the view when the form is
   * dirty to prevent loss of form data.
   */
  canDeactivate(): Observable<boolean | UrlTree> | boolean;
}

/**
 * @description
 * Class is used to provide a set of submission hooks and
 * functionality to pages used in enrolments.
 *
 * For example, outside of the boilerplate add getters for
 * quickly accessing AbstractControls in controllers to
 * reduce methods required in each controller.
 *
 * WARNING: Always use UntilDestroy in the controller to
 * unsubscribe from valueChanges on getters when the component
 * is destroyed. Not doing this will result in memory leaks, as
 * well as, create issues that are difficult to trace.
 *
 * @example
 * @UntilDestroy()
 * @Component({
 *   selector: 'app-example-page',
 *   templateUrl: './example-page.component.html',
 *   styleUrls: ['./example-page.component.scss']
 * })
 * export class ExamplePageComponent {
 *   public initForm(): void {
 *     this.formState.controlName.valueChanges
 *       .pipe(
 *         untilDestroyed(this),
 *         ...
 *       ).subscribe();
 *   }
 * }
 */
export abstract class AbstractFormPage<
  T extends AbstractFormState<unknown> = AbstractFormState<unknown>,
  S = unknown
> implements IFormPage
{
  // /**
  //  * @description
  //  * Busy subscription for use when blocking content from
  //  * being interacted with in the template. For example,
  //  * during but not limited to HTTP requests.
  //  */
  // public busy: Subscription;
  /**
   * @description
   * Instance of the form state which provides access to
   * an API for the form.
   */
  public abstract formState: T;
  /**
   * @description
   * Indicator applied after an initial submission of
   * the form occurs.
   */
  public hasAttemptedSubmission: boolean;
  /**
   * @description
   * Whether routing should be allowed after any form
   * control's value has been changed.
   */
  protected allowRoutingWhenDirty: boolean;
  /**
   * @description
   * Allowlisted set of control names that can be dirty, but
   * still allow routing. Allows for targeted route gating
   * on specific controls.
   *
   * @example
   * Form control checkboxes used as indicators, but are
   * not user entered data that could be lost.
   *
   * NOTE: allowRoutingWhenDirty must be falsey, as only one
   * of the routing checks can be used at a time.
   */
  protected canDeactivateAllowlist: string[];

  protected constructor(
    protected dialog: MatDialog,
    // TODO replace dialog with dialogService
    // protected dialogService: DialogService,
    protected formUtilsService: FormUtilsService
  ) {
    this.hasAttemptedSubmission = false;
    this.allowRoutingWhenDirty = false;
    this.canDeactivateAllowlist = [];
  }

  /**
   * @description
   * Form submission event handler.
   */
  public onSubmit(): void {
    this.hasAttemptedSubmission = true;
    if (this.checkValidity(this.formState.form)) {
      this.onSubmitFormIsValid();
      // TODO add in busy or find alternative so not attached to subscribe
      // this.busy = this.performSubmission()
      this.performSubmission()
        .pipe(tap((_) => this.formState.form.markAsPristine()))
        .subscribe((response?: S) => this.afterSubmitIsSuccessful(response));
    } else {
      this.onSubmitFormIsInvalid();
    }
  }

  /**
   * @description
   * Handle routing back to previous view.
   */
  public onBack(): void {
    // Optional back route event handler, otherwise NOOP
  }

  /**
   * @description
   * Deactivation guard handler.
   */
  public canDeactivate(): Observable<boolean | UrlTree> | boolean {
    const data = 'unsaved';
    return this.formState.form.dirty && !this.checkDeactivationIsAllowed()
      ? this.dialog
          .open(ConfirmDialogComponent, { data })
          // TODO replace dialog with dialogService
          // this.dialogService
          //   .canDeactivateFormDialog()
          .afterClosed()
          .pipe(
            map((dialogResult: boolean) =>
              this.handleDeactivation(dialogResult)
            )
          )
      : true;
  }

  /**
   * @description
   * Setup form listeners.
   */
  protected initForm(): void {
    // Optional method for setting up form listeners, but
    // when no listeners are required is NOOP
  }

  /**
   * @description
   * Deactivation guard hook to allow for specific actions
   * to be performed based on user interaction.
   *
   * NOTE: Usage example would be replacing previous form
   * values on deactivation so updates are discarded.
   */
  protected handleDeactivation(dialogResult: boolean): boolean | UrlTree {
    return dialogResult;
  }

  /**
   * @description
   * Check the validity of the form, as well as, perform
   * additional validation.
   */
  protected checkValidity(form: FormGroup | FormArray): boolean {
    return (
      this.formUtilsService.checkValidity(form) &&
      this.additionalValidityChecks(form)
    );
  }

  /**
   * @description
   * Additional checks outside of the form validity that
   * should gate form submission.
   */
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  protected additionalValidityChecks(form: FormGroup | FormArray): boolean {
    return true;
  }

  /**
   * @description
   * Pre-submission hook for execution.
   */
  protected onSubmitFormIsValid(): void {
    // Optional submission hook, otherwise NOOP
  }

  /**
   * @description
   * Pre-submission hook for execution.
   */
  protected onSubmitFormIsInvalid(): void {
    // Optional submission hook, otherwise NOOP
  }

  /**
   * @description
   * Submission hook for execution.
   */
  protected abstract performSubmission(): Observable<S>;

  /**
   * @description
   * Post-submission hook for execution.
   */
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  protected afterSubmitIsSuccessful(response?: S): void {
    // Optional submission hook, otherwise NOOP
  }

  /**
   * @description
   * Check that deactivation of the view is allowed in general
   * or specifically gated on a set of allowed control names.
   */
  private checkDeactivationIsAllowed(): boolean {
    if (!this.allowRoutingWhenDirty && this.canDeactivateAllowlist?.length) {
      return Object.keys(this.formState.form.controls)
        .filter((key) => !this.canDeactivateAllowlist.includes(key))
        .every((key) => !this.formState.form.controls[key].dirty);
    }
    return this.allowRoutingWhenDirty;
  }
}

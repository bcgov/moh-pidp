import { Injectable } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UrlTree } from '@angular/router';

import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import { AbstractFormState, ConfirmDialogComponent } from '@bcgov/shared/ui';

import { FormUtilsService } from '@core/services/form-utils.service';

/**
 * Helper service so the AbstractForm constructor can accept new service injections
 * without having to update every derived class.
 */
@Injectable({ providedIn: 'root' })
export class AbstractFormDependenciesService {
  public constructor(
    public dialog: MatDialog,
    public formUtilsService: FormUtilsService,
    public loadingOverlayService: LoadingOverlayService,
  ) {}
}
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
 *   selector: 'app-example',
 *   templateUrl: './example.page.html',
 *   styleUrls: ['./example.page.scss']
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
  S = unknown,
> implements IFormPage
{
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
   * If true, the a overlay will be displayed when the form is submitted.
   * Declared as abstract so derived classes must explicitly opt in to the behaviour.
   * This is done so that it is clear to the person reading the code how to track down
   * where the overlay is implemented.
   */
  public abstract showOverlayOnSubmit: boolean;
  /**
   * @description
   * Text to be displayed in the loading overlay when showOverlayOnSubmit is true.
   */
  public loadingOverlayMessageText: string;
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
    protected dependencies: AbstractFormDependenciesService, // protected dialog: MatDialog, // protected formUtilsService: FormUtilsService, // protected loadingOverlayService: LoadingOverlayService
  ) {
    this.hasAttemptedSubmission = false;
    this.allowRoutingWhenDirty = false;
    this.canDeactivateAllowlist = [];
    this.loadingOverlayMessageText = LOADING_OVERLAY_DEFAULT_MESSAGE;
  }

  /**
   * @description
   * Form submission event handler.
   */
  public onSubmit(): void {
    this.hasAttemptedSubmission = true;
    if (this.checkValidity(this.formState.form)) {
      this.onSubmitFormIsValid();
      const showLoadingOverlay = this.showOverlayOnSubmit;
      if (showLoadingOverlay) {
        this.dependencies.loadingOverlayService.open(
          this.loadingOverlayMessageText,
        );
      }
      this.performSubmission()
        .pipe(
          tap((_) => {
            this.formState.form.markAsPristine();
            if (showLoadingOverlay) {
              this.dependencies.loadingOverlayService.close();
            }
          }),
          catchError((error) => {
            if (showLoadingOverlay) {
              this.dependencies.loadingOverlayService.close();
            }
            return throwError(() => error);
          }),
        )
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
      ? this.dependencies.dialog
          .open(ConfirmDialogComponent, { data })
          .afterClosed()
          .pipe(
            map((dialogResult: boolean) =>
              this.handleDeactivation(dialogResult),
            ),
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

  protected get formUtilsService(): FormUtilsService {
    return this.dependencies.formUtilsService;
  }

  protected get dialog(): MatDialog {
    return this.dependencies.dialog;
  }

  /**
   * @description
   * Check the validity of the form, as well as, perform
   * additional validation.
   */
  protected checkValidity(form: FormGroup | FormArray): boolean {
    return (
      this.dependencies.formUtilsService.checkValidity(form) &&
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

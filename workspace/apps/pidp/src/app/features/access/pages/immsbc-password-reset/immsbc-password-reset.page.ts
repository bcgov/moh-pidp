import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';

/** At least one of resetPassword or resetMfa must be true. */
const atLeastOneResetOptionValidator: ValidatorFn = (
  group: AbstractControl,
): ValidationErrors | null => {
  const password = group.get('resetPassword')?.value as boolean;
  const mfa = group.get('resetMfa')?.value as boolean;
  return password || mfa ? null : { atLeastOneResetRequired: true };
};

@Component({
  selector: 'app-immsbc-password-reset',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
  ],
  templateUrl: './immsbc-password-reset.page.html',
  styleUrl: './immsbc-password-reset.page.scss',
})
export class ImmsbcPasswordResetPage implements OnInit {
  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public form!: FormGroup;

  private readonly fb = inject(FormBuilder);

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      {
        title: 'Access',
        path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
      },
      {
        title: 'ImmsBC',
        path: AccessRoutes.routePath(AccessRoutes.IMMSBC),
      },
      { title: 'Password/Authenticator Reset', path: '' },
    ];

    this.form = this.fb.group(
      {
        fullName: ['', Validators.required],
        emailAddressOnFile: ['', [Validators.required, Validators.email]],
        bcProviderLogin: ['', Validators.required],
        primaryPharmacy: ['', Validators.required],
        collegeId: [''],
        mobilePhoneNumber: [''],
        resetPassword: [false],
        resetMfa: [false],
        hasLoggedInBefore: [null, Validators.required],
      },
      { validators: atLeastOneResetOptionValidator },
    );

    // When MFA is toggled on, make mobilePhoneNumber required.
    this.form.get('resetMfa')?.valueChanges.subscribe((mfaSelected: boolean) => {
      const phoneControl = this.form.get('mobilePhoneNumber');
      if (mfaSelected) {
        phoneControl?.addValidators(Validators.required);
      } else {
        phoneControl?.removeValidators(Validators.required);
      }
      phoneControl?.updateValueAndValidity();
    });
  }

  public toggleResetOption(option: 'resetPassword' | 'resetMfa'): void {
    const control = this.form.get(option);
    control?.setValue(!control.value);
    this.form.updateValueAndValidity();
  }

  public setHasLoggedInBefore(value: boolean): void {
    this.form.patchValue({ hasLoggedInBefore: value });
  }

  public get resetOptionsInvalid(): boolean {
    return (
      this.form.hasError('atLeastOneResetRequired') &&
      (this.form.get('resetPassword')?.touched === true ||
        this.form.get('resetMfa')?.touched === true ||
        this.form.touched)
    );
  }

  public onSubmit(): void {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      console.log(this.form.value);
    }
  }
}

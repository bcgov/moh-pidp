import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';

@Component({
  selector: 'app-immsbc-adding-accounts',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
  ],
  templateUrl: './immsbc-adding-accounts.page.html',
  styleUrl: './immsbc-adding-accounts.page.scss',
})
export class ImmsbcAddingAccountsPage implements OnInit {
  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public form!: FormGroup;

  public readonly occupations = [
    'Pharmacist RPH',
    'Pharmacy Student',
    'Pharmacy Technician RPHT',
    'Pharmacy Assistant',
    'Doctor MD',
    'Nurse NP, RN, LPN',
    'Paramedic',
    'Other',
  ];

  public readonly accessLevels = [
    {
      value: 'clinician',
      title: 'Clinician',
      description: 'Clerk plus administer vaccinations and view appointments.',
    },
    {
      value: 'clerk',
      title: 'Clerk',
      description: 'Basic access to check-in patients.',
    },
  ];

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
      { title: 'Adding Accounts', path: '' },
    ];

    this.form = this.fb.group({
      pharmacyManagerName: ['', Validators.required],
      pharmacyManagerEmail: ['', [Validators.required, Validators.email]],
      pharmacyName: ['', Validators.required],
      pharmacareCode: ['', Validators.required],
      streetAddress: ['', Validators.required],
      postalCode: ['', Validators.required],
      pharmacyPhone: ['', Validators.required],
      legalFirstName: ['', Validators.required],
      legalLastName: ['', Validators.required],
      bcphaEmail: ['', [Validators.required, Validators.email]],
      mobilePhone: ['', Validators.required],
      occupation: ['', Validators.required],
      accessLevel: ['', Validators.required],
      trainingConfirmed: [false, Validators.requiredTrue],
      ackIndividualAccounts: [false, Validators.requiredTrue],
      ackMfa: [false, Validators.requiredTrue],
      ackPrivacy: [false, Validators.requiredTrue],
      ackRemovalAccess: [false, Validators.requiredTrue],
    });
  }

  public selectOccupation(occupation: string): void {
    this.form.patchValue({ occupation });
  }

  public selectAccessLevel(level: string): void {
    this.form.patchValue({ accessLevel: level });
  }

  public toggleTrainingConfirmed(): void {
    const current = this.form.get('trainingConfirmed')?.value as boolean;
    this.form.patchValue({ trainingConfirmed: !current });
  }

  public onSubmit(): void {
    if (this.form.valid) {
      console.log(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }
}

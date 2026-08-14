import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { EMPTY, catchError } from 'rxjs';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';
import { PharmacyResource } from './pharmacy-resource.service';
import { PharmacyFormComponent } from './pharmacy-form.component';

@Component({
  selector: 'app-immsbc-create-pharmacy',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
    MatSnackBarModule,
    PharmacyFormComponent,
  ],
  templateUrl: './immsbc-create-pharmacy.page.html',
  styleUrl: './immsbc-create-pharmacy.page.scss',
})
export class ImmsbcCreatePharmacyPage implements OnInit {
  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public form!: FormGroup;

  private readonly fb = inject(FormBuilder);
  private readonly resource = inject(PharmacyResource);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

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
      { title: 'Add Pharmacy', path: '' },
    ];

    this.form = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      phone: ['', Validators.required],
      fax: ['', Validators.required],
      managerName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      pharmacareCode: ['', Validators.required],
      ackImmunizationScope: [false, Validators.requiredTrue],
      ackAccessToVaccines: [false, Validators.requiredTrue],
      ackPrivacy: [false, Validators.requiredTrue],
      ackRemovalAccess: [false, Validators.requiredTrue],
    });
  }

  public onSubmit(): void {
    if (this.form.invalid) {
      console.log('Create Pharmacy - Invalid State, Form Value:', this.form.value);
      this.form.markAllAsTouched();
      return;
    }

    this.resource
      .createPharmacy(this.form.value)
      .pipe(
        catchError(() => {
          this.snackBar.open('An error occurred while creating the pharmacy. Please try again.', 'Close', { duration: 10000 });
          return EMPTY;
        }),
      )
      .subscribe(() => {
        this.snackBar.open('Pharmacy created successfully!', 'Close', {
          duration: 3000,
        });
        this.router.navigate([AccessRoutes.routePath(AccessRoutes.IMMSBC)]);
      });
  }
}
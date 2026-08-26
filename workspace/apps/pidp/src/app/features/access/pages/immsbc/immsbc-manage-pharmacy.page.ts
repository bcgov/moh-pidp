import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EMPTY, Observable, catchError, tap } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { AccessRoutes } from '@app/features/access/access.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { PharmacyResource } from './pharmacy-resource.service';
import { PharmacyStaffManagementComponent } from './pharmacy-staff-management.component';
import { Pharmacy, PharmacyRole, PharmacyProfile } from './pharmacy-staff.model';
import { PharmacyFormComponent } from './pharmacy-form.component';

@Component({
  selector: 'app-immsbc-manage-pharmacy',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatTabsModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
    PharmacyStaffManagementComponent,
    PharmacyFormComponent,
  ],
  templateUrl: './immsbc-manage-pharmacy.page.html',
  styleUrl: './immsbc-manage-pharmacy.page.scss',
})
export class ImmsbcManagePharmacyPage implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly resource = inject(PharmacyResource);
  private readonly snackBar = inject(MatSnackBar);

  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public pharmacyProfile$!: Observable<PharmacyProfile>;
  public selectedPharmacy: PharmacyProfile['associations'][0] | null = null;
  public detailsForm!: FormGroup;

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      { title: 'Access', path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS) },
      { title: 'ImmsBC', path: AccessRoutes.routePath(AccessRoutes.IMMSBC) },
      { title: 'Manage Pharmacy', path: '' },
    ];

    this.pharmacyProfile$ = this.resource.getPharmacyAdminProfile();

    this.detailsForm = this.fb.group({
      name: ['', [Validators.required]],
      healthAuthority: ['', [Validators.required]],
      address1: ['', [Validators.required]],
      address2: [''],
      city: ['', [Validators.required]],
      province: ['', [Validators.required]],
      postalCode: ['', [Validators.required]],
      managerName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      fax: ['', [Validators.required]],
      pharmaCareCode: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]],
    });
  }

  public selectPharmacy(pharmacy: PharmacyProfile['associations'][0]): void {
    this.selectedPharmacy = pharmacy;
    this.resource
      .getPharmacyDetails(pharmacy.pharmacyId)
      .pipe(tap((details: Pharmacy | null) => this.detailsForm.patchValue(details ?? {})))
      .subscribe();
  }

  public deselectPharmacy(): void {
    this.selectedPharmacy = null;
    this.detailsForm.reset();
  }

  public onUpdateDetails(): void {
    // console.log("updating details:  " + this.detailsForm.valid + ", " + this.detailsForm.dirty + ", " + this.selectedPharmacy);
    if (this.selectedPharmacy) {
      this.resource
        .updatePharmacy(this.selectedPharmacy.pharmacyId, this.detailsForm.value)
        .pipe(
          catchError(() => {
            this.snackBar.open('An error occurred while saving. Please try again.', 'Close', { duration: 5000 });
            return EMPTY;
          })
        )
        .subscribe(() => {
          this.detailsForm.markAsPristine();
          this.snackBar.open('Pharmacy details saved successfully!', 'Close', {
            duration: 3000,
            panelClass: ['immsbc-manage-snackbar'],
          });
        });
    }
  }

  public getRole(role: number): string {
    return PharmacyRole[role];
  }
}
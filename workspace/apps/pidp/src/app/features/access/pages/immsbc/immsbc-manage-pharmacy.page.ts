import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTabsModule } from '@angular/material/tabs';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { AccessRoutes } from '@app/features/access/access.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { PillComponent } from '@app/shared/components/pill/pill.component';

import { PharmacyResource } from './pharmacy-resource.service';
import { PharmacyStaffManagementComponent } from './pharmacy-staff-management.component';
import { Pharmacy, PharmacyRole, PharmacyProfile } from './pharmacy-staff.model';

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
    MatTabsModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
    PharmacyStaffManagementComponent,
    PillComponent,
  ],
  templateUrl: './immsbc-manage-pharmacy.page.html',
  styleUrl: './immsbc-manage-pharmacy.page.scss',
})
export class ImmsbcManagePharmacyPage implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly resource = inject(PharmacyResource);

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
      address: ['', [Validators.required]],
      managerName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      fax: [''],
      pharmaCareCode: [''],
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
    if (this.detailsForm.valid && this.selectedPharmacy) {
      this.resource.updatePharmacy(this.selectedPharmacy.pharmacyId, this.detailsForm.value).subscribe(() => {
        this.detailsForm.markAsPristine();
      });
    }
  }

  public getRole(role: number): string {
    return PharmacyRole[role];
  }
}
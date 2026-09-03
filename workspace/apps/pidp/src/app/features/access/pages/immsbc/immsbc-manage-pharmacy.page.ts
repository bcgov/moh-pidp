import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, EMPTY, Observable, catchError, exhaustMap, filter, switchMap, tap } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { AccessRoutes } from '@app/features/access/access.routes';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { PillComponent } from '@app/shared/components/pill/pill.component';
import { ConfirmDialogComponent, DialogOptions } from '@bcgov/shared/ui';
import { NgxMaskDirective } from 'ngx-mask';
import { MatExpansionModule } from '@angular/material/expansion';
import { PharmacyResource } from './pharmacy-resource.service';
import { PharmacyRole, PharmacyProfile, IStaff, Pharmacy } from './pharmacy-staff.model';
import { QrCodeDialogComponent } from './qr-code-dialog.component';
import { EditStaffDialogComponent } from './edit-staff-dialog.component';
import { InviteByEmailDialogComponent } from './invite-by-email-dialog.component';

@Component({
  selector: 'app-immsbc-manage-pharmacy',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTableModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
    PillComponent,
    NgxMaskDirective,
    MatExpansionModule,
    InviteByEmailDialogComponent,
  ],
  templateUrl: './immsbc-manage-pharmacy.page.html',
  styleUrl: './immsbc-manage-pharmacy.page.scss',
})
export class ImmsbcManagePharmacyPage implements OnInit {
  private readonly resource = inject(PharmacyResource);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);

  public breadcrumbsData: Array<{ title: string; path: string }> = [];
  public pharmacyProfile$!: Observable<PharmacyProfile>;
  public selectedPharmacy: PharmacyProfile['associations'][0] | null = null;
  public pharmacyDetails: Pharmacy | null = null;
  
  public contactForm!: FormGroup;
  public hasEmail = false;
  public contactPanelExpanded = false;

  // Staff Management properties
  private readonly refresh$ = new BehaviorSubject<void>(undefined);
  public staff$!: Observable<IStaff[]>;
  public dataSource = new MatTableDataSource<IStaff>();
  public displayedColumns: string[] = ['name', 'role', 'effectiveStartDate', 'effectiveEndDate', 'status', 'actions'];
  public PharmacyRole = PharmacyRole;

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      { title: 'Access', path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS) },
      { title: 'ImmsBC', path: AccessRoutes.routePath(AccessRoutes.IMMSBC) },
      { title: 'Manage Pharmacy', path: '' },
    ];

    this.contactForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      fax: ['']
    });

    this.pharmacyProfile$ = this.resource.getPharmacyAdminProfile();
  }

  public selectPharmacy(pharmacy: PharmacyProfile['associations'][0]): void {
    this.selectedPharmacy = pharmacy;
    
    // Fetch full pharmacy details for contact info form
    this.resource.getPharmacyDetails(pharmacy.pharmacyId).subscribe({
      next: (response: any) => {
        // Handle array wrap if the backend is bizarrely returning an array
        const details = Array.isArray(response) ? response[0] : response;
        this.pharmacyDetails = details;
        if (details) {
          this.contactForm.patchValue({
            email: details.email || '',
            phone: details.phone || '',
            fax: details.fax || ''
          });
          this.hasEmail = !!details.email;
          this.contactPanelExpanded = !this.hasEmail;
        }
        this.cdr.detectChanges(); // Force UI update
      },
      error: (err) => {
        console.error('Error fetching pharmacy details:', err);
        this.snackBar.open('Error fetching pharmacy details. Please check the console.', 'Close', { duration: 5000 });
        this.pharmacyDetails = null;
      }
    });

    // Initialize staff stream
    this.staff$ = this.refresh$.pipe(
      switchMap(() => this.resource.getStaff(this.selectedPharmacy!.pharmacyId)),
      tap((staff) => (this.dataSource.data = staff)),
    );
  }

  public deselectPharmacy(): void {
    this.selectedPharmacy = null;
    this.pharmacyDetails = null;
    this.hasEmail = false;
    this.contactForm.reset();
    this.dataSource.data = [];
  }

  public onSubmitContactInfo(): void {
    if (this.contactForm.invalid || !this.selectedPharmacy || !this.pharmacyDetails) {
      this.contactForm.markAllAsTouched();
      return;
    }

    const payload = {
      ...this.pharmacyDetails,
      ...this.contactForm.value
    };

    this.resource.updatePharmacy(this.selectedPharmacy.pharmacyId, payload)
      .pipe(
        catchError(() => {
          this.snackBar.open('An error occurred while updating contact information.', 'Close', { duration: 5000 });
          return EMPTY;
        })
      )
      .subscribe(() => {
        this.snackBar.open('Contact information updated successfully.', 'Close', { duration: 3000 });
        this.pharmacyDetails = payload;
        this.hasEmail = !!payload.email;
        this.contactPanelExpanded = false;
        this.contactForm.markAsPristine();
      });
  }

  // Staff Management methods
  public inviteByEmail(role: PharmacyRole): void {
    if (!this.selectedPharmacy) return;
    const dialogRef = this.dialog.open(InviteByEmailDialogComponent, {
      data: {
        pharmacyName: this.selectedPharmacy.pharmacyName,
        role: PharmacyRole[role],
      },
      width: '500px'
    });

    dialogRef.afterClosed().subscribe((emails: string[]) => {
      if (emails && emails.length > 0) {
        this.resource.inviteStaff(this.selectedPharmacy!.pharmacyId, role, emails).subscribe({
          next: () => {
            this.snackBar.open(`Successfully sent ${emails.length} invitation(s).`, 'Close', { duration: 3000 });
          },
          error: () => {
            this.snackBar.open('Failed to send some or all invitations.', 'Close', { duration: 5000 });
          }
        });
      }
    });
  }

  public generateLink(role: PharmacyRole): void {
    if (!this.selectedPharmacy) return;
    this.resource.generateEnrolmentToken(this.selectedPharmacy.pharmacyId, role).subscribe((token: string) => {
      const enrolmentLink = `${window.location.origin}/access/immsbc/pharmacy-enrol/${token}`;
      this.dialog.open(QrCodeDialogComponent, {
        data: {
          link: enrolmentLink,
          pharmacyName: this.selectedPharmacy!.pharmacyName,
          role: PharmacyRole[role],
        },
      });
    });
  }

  public getRole(role: PharmacyRole): string {
    return PharmacyRole[role];
  }

  public getStatus(staff: IStaff): string {
    if (staff.effectiveEndDate && new Date(staff.effectiveEndDate) < new Date()) {
      return 'Inactive';
    }
    return 'Active';
  }

  public onEdit(staff: IStaff): void {
    if (!this.selectedPharmacy) return;
    this.resource
      .getStaffDetails(this.selectedPharmacy.pharmacyId, staff.partyId)
      .pipe(
        exhaustMap((staffDetails) =>
          this.dialog.open(EditStaffDialogComponent, {
            data: {
              staff: staffDetails,
              pharmacyId: this.selectedPharmacy!.pharmacyId,
            },
            width: '500px',
          })
            .afterClosed()),
        filter((result) => !!result),
      )
      .subscribe({
        complete: () => this.refresh$.next()
      });
  }

  public onDelete(staff: IStaff): void {
    if (!this.selectedPharmacy) return;
    const confirmData: DialogOptions = {
      title: `Delete Staff Member?`,
      message: `Are you sure you want to delete this ${staff.fullName}?`,
      actionType: 'warn',
      actionText: `Delete ${staff.fullName}`
    };
    this.dialog
      .open(ConfirmDialogComponent, { data: confirmData })
      .afterClosed()
      .pipe(
        filter((confirmed: boolean) => confirmed),
        exhaustMap(() =>
          this.resource.deleteStaff(this.selectedPharmacy!.pharmacyId, staff.partyId).pipe(
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            catchError((error: any) => {
              const firstLine = (error.error as string).split('\n')[0];
              const errorData: DialogOptions = {
                title: 'Error Deleting Staff',
                message: firstLine.substring(firstLine.indexOf(': ') + 2).trim() ?? 'An unexpected error occurred.',
                actionText: 'OK',
                cancelHide: true,
              };
              this.dialog.open(ConfirmDialogComponent, { data: errorData });
              return EMPTY;
            })
          )
        )
      )
      .subscribe({
        complete: () => {
          this.dataSource.data = this.dataSource.data.filter(s => s.partyId !== staff.partyId);
          this.refresh$.next();
        }
      });
  }

  public onRenew(staff: IStaff): void {
    if (!this.selectedPharmacy) return;
    const today = new Date();
    const currentYear = today.getFullYear();
    const nextAugustFirst = new Date(currentYear, 7, 1);

    if (today >= nextAugustFirst) {
      nextAugustFirst.setFullYear(currentYear + 1);
    }

    const payload = {
      role: staff.role,
      effectiveStartDate: today.toISOString().split('T')[0],
      effectiveEndDate: nextAugustFirst.toISOString().split('T')[0]
    };

    const confirmData: DialogOptions = {
      title: `Renew Staff Member?`,
      message: `Are you sure you want to renew ${staff.fullName} until ${nextAugustFirst.toDateString()}?`,
      actionType: 'primary',
      actionText: `Renew ${staff.fullName}`
    };

    this.dialog
      .open(ConfirmDialogComponent, { data: confirmData })
      .afterClosed()
      .pipe(
        filter((confirmed: boolean) => confirmed),
        exhaustMap(() =>
          this.resource.updateStaff(this.selectedPharmacy!.pharmacyId, staff.partyId, payload).pipe(
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            catchError((error: any) => {
              const errorMsg = error.error ? error.error.toString().split('\n')[0] : 'An unexpected error occurred.';
              const errorData: DialogOptions = {
                title: 'Error Renewing Staff',
                message: errorMsg,
                actionText: 'OK',
                cancelHide: true,
              };
              this.dialog.open(ConfirmDialogComponent, { data: errorData });
              return EMPTY;
            })
          )
        )
      )
      .subscribe({
        complete: () => this.refresh$.next()
      });
  }
}
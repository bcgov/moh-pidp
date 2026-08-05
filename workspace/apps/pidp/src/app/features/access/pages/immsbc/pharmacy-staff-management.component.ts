import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { BehaviorSubject, EMPTY, Observable, catchError, exhaustMap, filter, switchMap, tap } from 'rxjs';
import { PillComponent } from '@app/shared/components/pill/pill.component';
import { PharmacyResource } from './pharmacy-resource.service';
import { IStaff, PharmacyRole } from './pharmacy-staff.model';
import { QrCodeDialogComponent } from './qr-code-dialog.component';
import { ConfirmDialogComponent, DialogOptions } from '@bcgov/shared/ui';
import { EditStaffDialogComponent } from './edit-staff-dialog.component';

@Component({
  selector: 'app-pharmacy-staff-management',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatProgressBarModule,
    MatTableModule,
    PillComponent,
    EditStaffDialogComponent,
  ],
  templateUrl: './pharmacy-staff-management.component.html',
})
export class PharmacyStaffManagementComponent implements OnInit {
  @Input() public pharmacyId!: number;
  @Input() public pharmacyName!: string;

  private readonly resource = inject(PharmacyResource);
  private readonly dialog = inject(MatDialog);
  private readonly refresh$ = new BehaviorSubject<void>(undefined);

  public title = 'Staff Management';
  public staff$!: Observable<IStaff[]>;
  public dataSource = new MatTableDataSource<IStaff>();
  public displayedColumns: string[] = ['name', 'role', 'status', 'actions'];
  public PharmacyRole = PharmacyRole;

  public ngOnInit(): void {
    this.staff$ = this.refresh$.pipe(
      switchMap(() => this.resource.getStaff(this.pharmacyId)),
      tap((staff) => (this.dataSource.data = staff)),
    );
  }

  public generateLink(role: PharmacyRole): void {
    this.resource.generateEnrolmentToken(this.pharmacyId, role).subscribe((token: string) => {
      const enrolmentLink = `${window.location.origin}/access/immsbc/pharmacy-enrol/${token}`;
      this.dialog.open(QrCodeDialogComponent, {
        data: {
          link: enrolmentLink,
          pharmacyName: this.pharmacyName,
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
    this.resource
      .getStaffDetails(this.pharmacyId, staff.partyId)
      .pipe(
        exhaustMap((staffDetails) =>
          this.dialog.open(EditStaffDialogComponent, {
            data: {
              staff: staffDetails,
              pharmacyId: this.pharmacyId,
            },
            width: '500px',
          })
        .afterClosed()),
        filter((result) => !!result),
      )
      .subscribe(() => this.refresh$.next());
  }

  public onDelete(staff: IStaff): void {
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
          this.resource.deleteStaff(this.pharmacyId, staff.partyId).pipe(
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
      .subscribe(() => this.refresh$.next());
  }
}
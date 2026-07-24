import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { PillComponent } from '@app/shared/components/pill/pill.component';
import { PharmacyResource } from './pharmacy-resource.service';
import { IStaff, PharmacyRole } from './pharmacy-staff.model';
import { QrCodeDialogComponent } from './qr-code-dialog.component';

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
    PillComponent
  ],
  templateUrl: './pharmacy-staff-management.component.html',
})
export class PharmacyStaffManagementComponent implements OnInit {
  @Input() public pharmacyId!: number;

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
      const enrolmentLink = `${window.location.origin}/enrol/${token}`;
      this.dialog.open(QrCodeDialogComponent, {
        data: {
          link: enrolmentLink,
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
    console.log('Editing staff:', staff);
  }

  public onDelete(staff: IStaff): void {
    this.resource
      .deleteStaff(this.pharmacyId, staff.partyId)
      .subscribe(() => this.refresh$.next());
  }
}
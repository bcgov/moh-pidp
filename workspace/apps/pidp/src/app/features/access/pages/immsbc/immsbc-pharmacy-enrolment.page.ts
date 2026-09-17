import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ConfirmDialogComponent,
  DialogOptions,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { PartyService } from '@app/core/party/party.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';
import { AccessRoutes } from '@app/features/access/access.routes';
import Keycloak from 'keycloak-js';
import { catchError, throwError } from 'rxjs';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PharmacyResource } from './pharmacy-resource.service';

@Component({
  selector: 'app-immsbc-pharmacy-enrolment',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatProgressBarModule,
    ReactiveFormsModule,
    MatButtonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
  ],
  templateUrl: './immsbc-pharmacy-enrolment.page.html',
})
export class ImmsbcPharmacyEnrolmentPage implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly partyService = inject(PartyService);
  private readonly resource = inject(PharmacyResource);
  private readonly keycloak = inject(Keycloak);
  private readonly dialog = inject(MatDialog);
  private readonly fb = inject(FormBuilder);
  private readonly portalResource = inject(PortalResource);
  private readonly cdr = inject(ChangeDetectorRef);

  public form!: FormGroup;
  public token: string | null = null;
  public breadcrumbsData: Array<{ title: string; path: string }> = [];

  public title = 'Enrolment';
  public message = 'Processing your enrolment...';
  public isError = false;

  public ngOnInit(): void {
    this.breadcrumbsData = [
      { title: 'Home', path: '' },
      { title: 'Access', path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS) },
      { title: 'ImmsBC', path: AccessRoutes.routePath(AccessRoutes.IMMSBC) },
      { title: 'Enrolment', path: '' },
    ];
    this.token = this.route.snapshot.paramMap.get('token');
    console.log('[ImmsbcPharmacyEnrolment] ngOnInit started. Token:', this.token);

    if (!this.token) {
      this.handleError(
        'No enrolment token provided. The link may be invalid or expired.',
      );
      console.warn('[ImmsbcPharmacyEnrolment] No token provided, exiting ngOnInit.');
      return;
    }

    console.log('[ImmsbcPharmacyEnrolment] Fetching profile status for partyId:', this.partyService.partyId);
    this.portalResource.getProfileStatus(this.partyService.partyId).subscribe({
      next: (profileStatus) => {
        console.log('[ImmsbcPharmacyEnrolment] Received profileStatus:', profileStatus);
        console.log('[ImmsbcPharmacyEnrolment] bcProvider statusCode:', profileStatus?.status?.bcProvider?.statusCode);
        
        if (profileStatus?.status?.bcProvider?.statusCode !== StatusCode.COMPLETED) {
          console.log('[ImmsbcPharmacyEnrolment] BC Provider not completed, handling missing bc provider error');
          this.handleMissingBcProviderError('You must link or create a BC Provider account before you can enrol in a pharmacy.');
        } else {
          console.log('[ImmsbcPharmacyEnrolment] BC Provider is completed, proceeding with form setup');
          // User is authenticated, proceed with form setup
          this.form = this.fb.group({
            privacyTrainingAcknowledged: [false, Validators.requiredTrue]
          });
          console.log('[ImmsbcPharmacyEnrolment] Form initialized:', this.form);

          this.message = 'Please acknowledge the privacy and security training to proceed.';
          this.cdr.detectChanges();
        }
      },
      error: (error) => {
        console.error('[ImmsbcPharmacyEnrolment] Error fetching profile status:', error);
      },
      complete: () => {
        console.log('[ImmsbcPharmacyEnrolment] getProfileStatus observable completed');
      }
    });
  }

  public onSubmit(): void {
    if (this.form.invalid || !this.token) {
      this.form.markAllAsTouched();
      return;
    }

    this.message = 'Processing your enrolment...';
    this.isError = false;

    this.resource
      .enrolStaff(this.token, { privacyTrainingAcknowledged: true })
      .pipe(
        catchError((error) => {
          let errorMessage = 'An unexpected error occurred during enrolment.';
          if (typeof error.error === 'string') {
            errorMessage = error.error;
          } else if (error.error?.detail) {
            errorMessage = error.error.detail;
          } else if (error.error?.title) {
            errorMessage = error.error.title;
          } else if (error.message) {
            errorMessage = error.message;
          }

          let parsedMessage = errorMessage;
          if (parsedMessage.includes(': ')) {
            parsedMessage = parsedMessage.substring(parsedMessage.indexOf(': ') + 2).trim();
          }
          const firstLine = parsedMessage.split('\n')[0] || 'An unexpected error occurred during enrolment.';

          if (firstLine.toLowerCase().includes('bc provider') || firstLine.includes('bc-provider-application')) {
            this.handleMissingBcProviderError(firstLine);
          } else {
            this.handleError(firstLine);
          }

          return throwError(() => error);
        }),
      )
      .subscribe({
        complete: () => {
          this.handleSuccess();
        }
      });
  }

  private handleSuccess(): void {
    localStorage.removeItem('pending_pharmacy_enrolment_token');
    const data: DialogOptions = {
      title: 'Enrolment Successful',
      message:
        'You have been successfully associated with the pharmacy. You can now request access to ImmsBC from the access request page.',
      actionText: 'OK',
      cancelHide: true,
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .subscribe(() => this.router.navigate(['/']));
  }

  private handleMissingBcProviderError(message: string): void {
    this.message = message;
    this.isError = true;
    const data: DialogOptions = {
      title: 'BC Provider Account Required',
      message: message,
      actionText: 'Link Account',
      cancelText: 'Cancel',
      cancelHide: false,
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          localStorage.setItem('pending_pharmacy_enrolment_token', this.token as string);
          this.router.navigate(['/account/bc-provider-application']);
        } else {
          localStorage.removeItem('pending_pharmacy_enrolment_token');
          this.router.navigate(['/']);
        }
      });
  }

  private handleError(message: string): void {
    localStorage.removeItem('pending_pharmacy_enrolment_token');
    this.message = message;
    this.isError = true;
    const data: DialogOptions = {
      title: 'Enrolment Failed',
      message: message,
      actionText: 'OK',
      cancelHide: true,
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .subscribe(() => this.router.navigate(['/']));
  }

  public onCancel(): void {
    localStorage.removeItem('pending_pharmacy_enrolment_token');
    this.router.navigate(['/']);
  }
}
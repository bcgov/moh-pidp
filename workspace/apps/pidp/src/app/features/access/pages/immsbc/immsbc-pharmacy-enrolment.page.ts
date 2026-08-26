import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ConfirmDialogComponent,
  DialogOptions,
  PageComponent,
  PageHeaderComponent,
} from '@bcgov/shared/ui';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { PartyService } from '@app/core/party/party.service';
import Keycloak from 'keycloak-js';
import { catchError, throwError } from 'rxjs';
import { PharmacyResource } from './pharmacy-resource.service';

@Component({
  selector: 'app-immsbc-pharmacy-enrolment',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatProgressBarModule,
    PageComponent,
    PageHeaderComponent,
    ReactiveFormsModule,
    MatButtonModule,
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

  public form!: FormGroup;
  public token: string | null = null;

  public title = 'Pharmacy Enrolment';
  public message = 'Processing your enrolment...';
  public isError = false;

  public ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token');

    if (!this.token) {
      this.handleError(
        'No enrolment token provided. The link may be invalid or expired.',
      );
      return;
    }

    // User is authenticated, proceed with form setup
    this.form = this.fb.group({
      privacyTrainingAcknowledged: [false, Validators.requiredTrue]
    });
    
    this.message = 'Please acknowledge the privacy and security training to proceed.';
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
          this.router.navigate(['/account/bc-provider-application']);
        } else {
          this.router.navigate(['/']);
        }
      });
  }

  private handleError(message: string): void {
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
}
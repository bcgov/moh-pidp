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
import { PartyService } from '@app/core/party/party.service';
import Keycloak from 'keycloak-js';
import { EMPTY, catchError } from 'rxjs';
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

  public title = 'Pharmacy Enrolment';
  public message = 'Processing your enrolment...';
  public isError = false;

  public ngOnInit(): void {
    const token = this.route.snapshot.paramMap.get('token');

    if (!token) {
      this.handleError(
        'No enrolment token provided. The link may be invalid or expired.',
      );
      return;
    }

    if (!this.partyService.partyId) {
      // Not authenticated, redirect to login and return to this page
      this.keycloak.login({
        redirectUri: window.location.href,
      });
      return; // Stop execution until user is logged in and redirected back
    }

    // User is authenticated, proceed with enrolment
    this.resource
      .enrolStaff(token)
      .pipe(
        catchError((error) => {
          const errorMessage = (error.error as string).split('\n')[0];
          this.handleError(
            errorMessage.substring(errorMessage.indexOf(': ') + 2).trim() ||
              'An unexpected error occurred during enrolment.',
          );
          return EMPTY;
        }),
      )
      .subscribe(() => {
        this.handleSuccess();
      });
  }

  private handleSuccess(): void {
    const data: DialogOptions = {
      title: 'Enrolment Successful',
      message:
        'You have been successfully associated with the pharmacy. You will now be redirected to the home page.',
      actionText: 'OK',
      cancelHide: true,
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .subscribe(() => this.router.navigate(['/']));
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
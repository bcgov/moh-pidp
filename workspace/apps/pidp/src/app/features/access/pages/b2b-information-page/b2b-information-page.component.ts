/* eslint-disable @typescript-eslint/no-explicit-any */
import { NgOptimizedImage } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

import { EMPTY, Observable, catchError, exhaustMap, noop, of, tap } from 'rxjs';

import {
  LOADING_OVERLAY_DEFAULT_MESSAGE,
  LoadingOverlayService,
} from '@pidp/presentation';

import { NoContent } from '@bcgov/shared/data-access';
import {
  ConfirmDialogComponent,
  DialogOptions,
  HtmlComponent,
  InjectViewportCssClassDirective,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { AccessRoutes } from '../../access.routes';
import { B2bInformationFormState } from './b2b-information-form-state';
import { B2bInvitationResource } from './b2b-invitation-resource.service';

@Component({
  selector: 'app-b2b-information-page',
  standalone: true,
  imports: [
    BreadcrumbComponent,
    InjectViewportCssClassDirective,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    NgOptimizedImage,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './b2b-information-page.component.html',
  styleUrl: './b2b-information-page.component.scss',
})
export class B2bInformationPageComponent extends AbstractFormPage<B2bInformationFormState> {
  public AccessRoutes = AccessRoutes;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    { title: 'B2B Information', path: '' },
  ];
  public formState: B2bInformationFormState;
  public showOverlayOnSubmit = false;

  public constructor(
    fb: FormBuilder,
    dependenciesService: AbstractFormDependenciesService,
    private router: Router,
    private b2bResource: B2bInvitationResource,
    private partyService: PartyService,
    private loadingOverlayService: LoadingOverlayService,
  ) {
    super(dependenciesService);
    this.formState = new B2bInformationFormState(fb);
  }

  public onContinue(): void {
    this.router.navigate([
      AccessRoutes.routePath(AccessRoutes.BC_PROVIDER_EDIT),
    ]);
  }

  protected performSubmission(): NoContent {
    this.loadingOverlayService.open(LOADING_OVERLAY_DEFAULT_MESSAGE);
    return this.b2bResource
      .inviteGuestAccount(
        this.partyService.partyId,
        this.formState.userPrincipalName.value,
      )
      .pipe(
        tap((_) => {
          this.loadingOverlayService.close();
          this.showSuccessDialog();
          console.info('User invited');
        }),
        catchError(() => {
          console.error('Failed to invite user');
          return of(noop());
        }),
      );
  }

  private showSuccessDialog(): void {
    const data: DialogOptions = {
      title: 'Invitation created',
      bottomBorder: false,
      titlePosition: 'center',
      bodyTextPosition: 'center',
      component: HtmlComponent,
      data: {
        content: `We have successfully invited your account <br /> <strong>${this.formState.userPrincipalName.value}</strong>. <br /> The entire process may take up to 5 minutes. You may continue to the Imms BC page.`,
      },
      imageSrc: '/assets/images/online-marketing-hIgeoQjS_iE-unsplash.jpg',
      imageType: 'banner',
      width: '31rem',
      height: '24rem',
      actionText: 'Continue',
      actionTypePosition: 'center',
    };
    this.dialog
      .open(ConfirmDialogComponent, { data })
      .afterClosed()
      .pipe(exhaustMap((result) => (result ? this.onNext() : EMPTY)))
      .subscribe();
  }

  private onNext(): Observable<void> {
    this.router.navigate([AccessRoutes.routePath(AccessRoutes.IMMSBC)]);
    return of(noop());
  }
}

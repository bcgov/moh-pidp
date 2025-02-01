import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { AccessRoutes } from '@app/features/access/access.routes';
import { AbstractFormDependenciesService, AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { BreadcrumbComponent } from '../../../../../shared/components/breadcrumb/breadcrumb.component';
import { ExternalGuestInvitationFormState } from './external-guest-invitation-form-state';
import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';
import { Observable } from 'rxjs';
import { ProfileRoutes } from '@app/features/profile/profile.routes';

@Component({
  selector: 'app-external-guest-invitation',
  templateUrl: './external-guest-invitation.page.html',
  styleUrl: './external-guest-invitation.page.scss',
  standalone: true,
  imports: [
    CommonModule,
    InjectViewportCssClassDirective,
    BreadcrumbComponent,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule
  ],
})
export class ExternalGuestInvitationComponent  extends AbstractFormPage<ExternalGuestInvitationFormState>{
  public formState: ExternalGuestInvitationFormState;
  public showOverlayOnSubmit: boolean = false;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    {
      title: 'Access',
      path: AccessRoutes.routePath(AccessRoutes.ACCESS_REQUESTS),
    },
    {
      title: 'Account Linking',
      path: ProfileRoutes.routePath(ProfileRoutes.ACCOUNT_LINKING),
    },
    { title: 'External Guest Invitation', path: '' },
  ];

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    fb: FormBuilder,
  ) {
    super(dependenciesService);
    this.formState = new ExternalGuestInvitationFormState(fb);
  }

  protected performSubmission(): Observable<unknown> {
    return new Observable();
  }

}

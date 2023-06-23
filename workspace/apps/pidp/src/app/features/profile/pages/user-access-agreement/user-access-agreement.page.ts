import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, map } from 'rxjs';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { UserAccessAgreementFormState } from './user-access-agreement-form-state';
import { UserAccessAgreementResource } from './user-access-agreement-resource.service';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
})
export class UserAccessAgreementPage extends AbstractFormPage<any> {
  public title: string;
  public username: Observable<string>;
  public completed: boolean | null;
  public formState: any;

  // ui-page is handling this.
  public showOverlayOnSubmit = false;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: UserAccessAgreementResource,
    accessTokenService: AccessTokenService,
    fb: FormBuilder
  ) {
    super(dependenciesService);

    this.title = this.route.snapshot.data.title;
    this.formState = new UserAccessAgreementFormState(fb);
    this.username = accessTokenService
      .decodeToken()
      .pipe(map((token) => token?.name ?? ''));

    const routeData = this.route.snapshot.data;
    this.completed = routeData.userAccessAgreementCode === StatusCode.COMPLETED;
  }

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.partyId;

    return partyId ? this.resource.acceptAgreement(partyId) : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

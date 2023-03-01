import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, catchError, of, tap } from 'rxjs';

import { NoContent, OrganizationCode } from '@bcgov/shared/data-access';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { Lookup } from '@app/modules/lookup/lookup.types';

import { OrganizationDetailsFormState } from './organization-details-form-state';
import { OrganizationDetailsResource } from './organization-details-resource.service';
import { OrganizationDetails } from './organization-details.model';

@Component({
  selector: 'app-organization-details',
  templateUrl: './organization-details.page.html',
  styleUrls: ['./organization-details.page.scss'],
})
export class OrganizationDetailsPage
  extends AbstractFormPage<OrganizationDetailsFormState>
  implements OnInit
{
  public title: string;
  public formState: OrganizationDetailsFormState;
  public organizations: (Lookup & { disabled: boolean })[];
  public healthAuthorities: Lookup[];

  // ui-page is handling this.
  public showOverlayOnSubmit = false;

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: OrganizationDetailsResource,
    private logger: LoggerService,
    private lookupService: LookupService,
    fb: FormBuilder
  ) {
    super(dependenciesService);

    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.formState = new OrganizationDetailsFormState(fb);
    this.organizations = this.lookupService.organizations.map(
      (organization) => ({
        ...organization,
        disabled: organization.code !== OrganizationCode.HealthAuthority,
      })
    );
    this.healthAuthorities = this.lookupService.healthAuthorities;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    this.resource
      .get(partyId)
      .pipe(
        tap((model: OrganizationDetails | null) =>
          this.formState.patchValue(model)
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        })
      )
      .subscribe();
  }

  protected performSubmission(): NoContent {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

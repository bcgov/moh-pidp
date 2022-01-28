import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';

import { EMPTY, Observable, catchError, of, tap } from 'rxjs';

import { AbstractFormPage } from '@app/core/classes/abstract-form-page.class';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { PartyService } from '@app/core/services/party.service';
import { CollegeLookup } from '@app/modules/lookup/lookup.model';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { CollegeLicenceInformationFormState } from './college-licence-information-form-state';
import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformationModel } from './college-licence-information.model';

@Component({
  selector: 'app-college-licence-information',
  templateUrl: './college-licence-information.component.html',
  styleUrls: ['./college-licence-information.component.scss'],
  viewProviders: [CollegeLicenceInformationResource],
})
export class CollegeLicenceInformationComponent
  extends AbstractFormPage<CollegeLicenceInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: CollegeLicenceInformationFormState;
  public colleges: CollegeLookup[];
  public inGoodStanding: boolean;

  public constructor(
    protected dialog: MatDialog,
    // TODO replace dialog with dialogService
    // protected dialogService: DialogService,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceInformationResource,
    private logger: LoggerService,
    lookupService: LookupService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new CollegeLicenceInformationFormState(fb);
    this.colleges = lookupService.colleges;
    this.inGoodStanding = false;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    const partyId = this.partyService.profileStatus?.id;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    this.resource
      .get(partyId)
      .pipe(
        tap((model: CollegeLicenceInformationModel | null) =>
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

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.profileStatus?.id;

    return partyId && this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    const queryParams = {
      completedProfile: !this.partyService.completedProfile,
    };
    this.navigateToRoot({ queryParams });
  }

  private navigateToRoot(navigationExtras?: NavigationExtras): void {
    this.router.navigate(
      [this.route.snapshot.data.routes.root],
      navigationExtras
    );
  }
}

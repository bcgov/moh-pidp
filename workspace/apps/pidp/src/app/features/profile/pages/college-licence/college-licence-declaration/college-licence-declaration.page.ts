import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, of, tap } from 'rxjs';

import { DashboardStateModel, PidpStateName } from '@pidp/data-model';
import { RegisteredCollege } from '@pidp/data-model';
import { AppStateService } from '@pidp/presentation';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { CollegeLookup } from '@app/modules/lookup/lookup.types';

import { CollegeLicenceDeclarationFormState } from './college-licence-declaration-form-state';
import { CollegeLicenceDeclarationResource } from './college-licence-declaration-resource.service';
import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

@Component({
  selector: 'app-college-licence-declaration',
  templateUrl: './college-licence-declaration.page.html',
  styleUrls: ['./college-licence-declaration.page.scss'],
  viewProviders: [CollegeLicenceDeclarationResource],
})
export class CollegeLicenceDeclarationPage
  extends AbstractFormPage<CollegeLicenceDeclarationFormState>
  implements OnInit
{
  public title: string;
  public formState: CollegeLicenceDeclarationFormState;
  public colleges: CollegeLookup[];
  public showOverlayOnSubmit = true;

  public get showNurseValidationInfo(): boolean {
    const isNurse =
      this.formState.collegeCode.value === RegisteredCollege.Bccnm;
    return isNurse;
  }

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceDeclarationResource,
    private logger: LoggerService,
    private lookupService: LookupService,
    private stateService: AppStateService,
    fb: FormBuilder
  ) {
    super(dependenciesService);

    this.title = this.route.snapshot.data.title;
    this.formState = new CollegeLicenceDeclarationFormState(fb);
    this.colleges = lookupService.colleges;
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
        tap((model: PartyLicenceDeclarationInformation | null) =>
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

  protected performSubmission(): Observable<string | null> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.updateDeclaration(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(cpn: string | null): void {
    // Set college on the left navigation.
    const collegeCode = this.formState.collegeCode.value as number;
    const college = this.lookupService.getCollege(collegeCode);
    const collegeName = college?.name ?? '';

    const oldState = this.stateService.getNamedState<DashboardStateModel>(
      PidpStateName.dashboard
    );
    const newState: DashboardStateModel = {
      ...oldState,
      userProfileCollegeNameText: collegeName,
    };
    this.stateService.setNamedState(PidpStateName.dashboard, newState);

    if (!cpn) {
      this.navigateToRoot();
    } else {
      this.router.navigate(
        [ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO)],
        { replaceUrl: true }
      );
    }
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

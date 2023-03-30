import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

import { EMPTY, Observable, debounceTime, switchMap, tap } from 'rxjs';

import {
  DashboardStateModel,
  PidpStateName,
  RegisteredCollege,
} from '@pidp/data-model';
import { AppStateService, NavigationService } from '@pidp/presentation';

import { ToggleContentChange } from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { LookupResource } from '@app/modules/lookup/lookup-resource.service';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { CollegeLookup } from '@app/modules/lookup/lookup.types';

import { MeFormState } from './me-form-state';

@Component({
  selector: 'app-me',
  templateUrl: './me.page.html',
  styleUrls: ['./me.page.scss'],
})
export class MePage extends AbstractFormPage<MeFormState> implements OnInit {
  public formState: MeFormState;

  public showOverlayOnSubmit = true;

  public identityProvider$: Observable<IdentityProvider>;
  public colleges: CollegeLookup[];
  public hasPreferredName: boolean;

  public get showNurseValidationInfo(): boolean {
    const isNurse =
      this.formState.collegeCode.value === RegisteredCollege.Bccnm;
    return isNurse;
  }
  public name = '';
  public dateOfBirthText = '';
  public warningMessage =
    'Our system is not familiar with this email address, please double check the spelling. If everything is correct hit save information, and our system will update.';

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private snackBar: MatSnackBar,
    private partyService: PartyService,
    private navigationService: NavigationService,
    private logger: LoggerService,
    private lookupService: LookupService,
    private lookupResource: LookupResource,
    private stateService: AppStateService,
    private authorizedUserService: AuthorizedUserService,
    fb: FormBuilder
  ) {
    super(dependenciesService);

    this.formState = new MeFormState(fb);

    this.hasPreferredName = false;
    this.colleges = lookupService.colleges;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    this.authorizedUserService.user$.subscribe((user) => {
      this.name = `${user.firstName} ${user.lastName}`;
      this.dateOfBirthText = user.birthdate ?? '';
    });

    this.formState.collegeCode.valueChanges.subscribe((x) => {
      if (x && this.formState.licenceNumber.disabled) {
        this.formState.licenceNumber.enable();
      } else if (!x && this.formState.licenceNumber.enabled) {
        this.formState.licenceNumber.disable();
      }
    });
    this.formState.email.valueChanges
      .pipe(
        tap((_) => this.snackBar.dismiss()),
        debounceTime(800),
        switchMap((email) => this.lookupResource.hasCommonEmailDomain(email)),
        tap((emailFound) => {
          if (!emailFound) {
            this.openSnackBar(this.warningMessage, 'Ok');
          }
        })
      )
      .subscribe();

    // TODO: Hook this up to the back end to fetch the data for edit.
    // this.resource
    //   .get(partyId)
    //   .pipe(
    //     tap((model: PartyLicenceDeclarationInformation | null) =>
    //       this.formState.patchValue(model)
    //     ),
    //     catchError((error: HttpErrorResponse) => {
    //       if (error.status === HttpStatusCode.NotFound) {
    //         this.navigateToRoot();
    //       }
    //       return of(null);
    //     })
    //   )
    //   .subscribe();
  }
  public onBack(): void {
    this.navigateToRoot();
  }
  public onPreferredNameToggle({ checked }: ToggleContentChange): void {
    this.handlePreferredNameChange(checked);
  }
  protected performSubmission(): Observable<void> {
    // TODO: Hook this up to the back end.

    // const partyId = this.partyService.partyId;
    // return partyId && this.formState.json
    //   ? this.resource.update(partyId, this.formState.json)
    //   : EMPTY;
    console.log('me', this.formState);

    return EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.setCollegeOnLeftNav();

    this.navigateToRoot();
  }
  private openSnackBar(message: string, action: string): void {
    this.snackBar.open(message, action);
  }

  private setCollegeOnLeftNav(): void {
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
  }
  private navigateToRoot(): void {
    this.navigationService.navigateToRoot();
  }
  private handlePreferredNameChange(checked: boolean): void {
    this.hasPreferredName = checked;
    this.formUtilsService.setOrResetValidators(
      checked,
      this.formState.preferredFirstName
    );
    this.formUtilsService.setOrResetValidators(
      checked,
      this.formState.preferredLastName
    );
    if (!checked) {
      this.formState.preferredMiddleName.reset();
    }
  }
}

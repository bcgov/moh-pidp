import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';

import {
  EMPTY,
  Observable,
  Subject,
  catchError,
  debounceTime,
  of,
  switchMap,
  tap,
} from 'rxjs';

import { ToggleContentChange } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { LookupResource } from '@app/modules/lookup/lookup-resource.service';

import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { FormUtilsService } from '@core/services/form-utils.service';

import { PersonalInformationFormState } from './personal-information-form-state';
import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformation } from './personal-information.model';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.page.html',
  styleUrls: ['./personal-information.page.scss'],
  viewProviders: [PersonalInformationResource],
})
export class PersonalInformationPage
  extends AbstractFormPage<PersonalInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: PersonalInformationFormState;
  public user$: Observable<User>;
  public identityProvider$: Observable<IdentityProvider>;
  public hasPreferredName: boolean;
  public warningMessage: string;
  public emailChanged: Subject<null>;
  public userInput: string;

  public IdentityProvider = IdentityProvider;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PersonalInformationResource,
    private authorizedUserService: AuthorizedUserService,
    private logger: LoggerService,
    private _snackBar: MatSnackBar,
    private lookupResource: LookupResource,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new PersonalInformationFormState(fb);
    this.user$ = this.authorizedUserService.user$;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
    this.hasPreferredName = false;
    this.warningMessage =
      'Double check the spelling of your email. Your email domain does not appear to be commonly used';
    this.emailChanged = new Subject<null>();
    this.userInput = '';
  }

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {
    this.handlePreferredNameChange(checked);
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public onEmailInputChange(emailInput: string): void {
    this.userInput = emailInput;
    this.emailChanged.next(null);
  }

  public openSnackBar(message: string, action: string): void {
    this._snackBar.open(message, action);
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    this.emailChanged
      .pipe(
        tap(() => this._snackBar.dismiss()),
        debounceTime(1000),
        switchMap(() =>
          this.lookupResource.hasCommonEmailDomain(this.userInput)
        )
      )
      .subscribe((emailFound) => {
        if (!emailFound) {
          this.openSnackBar(this.warningMessage, 'Ok');
        }
      });

    this.resource
      .get(partyId)
      .pipe(
        tap((model: PersonalInformation | null) =>
          this.formState.patchValue(model)
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        })
      )
      .subscribe((model: PersonalInformation | null) =>
        this.handlePreferredNameChange(!!model?.preferredFirstName)
      );
  }

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.partyId;

    return partyId && this.formState.json
      ? this.resource.update(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private handlePreferredNameChange(checked: boolean): void {
    this.hasPreferredName = checked;
    [
      this.formState.preferredFirstName,
      this.formState.preferredLastName,
    ].forEach((field: FormControl) => {
      this.formUtilsService.setOrResetValidators(checked, field);
    });
    if (!checked) {
      this.formState.preferredMiddleName.reset();
    }
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

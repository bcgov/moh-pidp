import { AsyncPipe, NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
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

import {
  ContactFormComponent,
  InjectViewportCssClassDirective,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
  PreferredNameFormComponent,
  ToggleContentChange,
  ToggleContentComponent,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { IdentityProvider } from '@app/features/auth/enums/identity-provider.enum';
import { User } from '@app/features/auth/models/user.model';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { LookupResource } from '@app/modules/lookup/lookup-resource.service';
import { IsHighAssurancePipe } from '@app/shared/pipes/is-high-assurance.pipe';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@core/classes/abstract-form-page.class';

import { UserInfoComponent } from './components/user-info/user-info.component';
import { PersonalInformationFormState } from './personal-information-form-state';
import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformation } from './personal-information.model';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.page.html',
  styleUrls: ['./personal-information.page.scss'],
  viewProviders: [PersonalInformationResource],
  standalone: true,
  imports: [
    AsyncPipe,
    BreadcrumbComponent,
    ContactFormComponent,
    InjectViewportCssClassDirective,
    IsHighAssurancePipe,
    MatButtonModule,
    NgIf,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
    PreferredNameFormComponent,
    ToggleContentComponent,
    UserInfoComponent,
  ],
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
  public userEmail: string;

  public IdentityProvider = IdentityProvider;

  // ui-page is handling this.
  public showOverlayOnSubmit = false;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'Personal Information', path: '' },
  ];

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PersonalInformationResource,
    private authorizedUserService: AuthorizedUserService,
    private logger: LoggerService,
    private _snackBar: MatSnackBar,
    private lookupResource: LookupResource,
    fb: FormBuilder,
  ) {
    super(dependenciesService);

    this.title = this.route.snapshot.data.title;
    this.formState = new PersonalInformationFormState(fb);
    this.user$ = this.authorizedUserService.user$;
    this.identityProvider$ = this.authorizedUserService.identityProvider$;
    this.hasPreferredName = false;
    this.warningMessage =
      'Our system is not familiar with this email address, please double check the spelling. If everything is correct hit save information, and our system will update.';
    this.emailChanged = new Subject<null>();
    this.userEmail = '';
  }

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {
    this.handlePreferredNameChange(checked);
  }

  public onBack(): void {
    this._snackBar.dismiss();
    this.navigateToRoot();
  }

  public onEmailInputChange(emailInput: string): void {
    this.userEmail = emailInput;
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
        debounceTime(800),
        switchMap(() =>
          this.lookupResource.hasCommonEmailDomain(this.userEmail),
        ),
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
          this.formState.patchValue(model),
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        }),
      )
      .subscribe((model: PersonalInformation | null) =>
        this.handlePreferredNameChange(!!model?.preferredFirstName),
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

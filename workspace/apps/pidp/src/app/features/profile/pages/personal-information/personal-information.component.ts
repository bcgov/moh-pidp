import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, of, tap } from 'rxjs';

import { ToggleContentChange } from '@bcgov/shared/ui';

import { AuthorizedUserService } from '@app/core/services/authorized-user.service';
import { LoggerService } from '@app/core/services/logger.service';
import { BcscUser } from '@app/features/auth/models/bcsc-user.model';

import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { FormUtilsService } from '@core/services/form-utils.service';
import { PartyService } from '@core/services/party.service';

import { PersonalInformationFormState } from './personal-information-form-state';
import { PersonalInformationResource } from './personal-information-resource.service';
import { PersonalInformationModel } from './personal-information.model';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.scss'],
  viewProviders: [PersonalInformationResource],
})
export class PersonalInformationComponent
  extends AbstractFormPage<PersonalInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: PersonalInformationFormState;
  public bcscUser: Observable<BcscUser | null>;
  public hasPreferredName: boolean;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: PersonalInformationResource,
    private authorizedUserService: AuthorizedUserService,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new PersonalInformationFormState(fb);
    this.bcscUser = this.authorizedUserService.user$;
    this.hasPreferredName = false;
  }

  public onPreferredNameToggle({ checked }: ToggleContentChange): void {
    this.handlePreferredNameChange(checked);
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
        tap((model: PersonalInformationModel | null) =>
          this.formState.patchValue(model)
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        })
      )
      .subscribe((model: PersonalInformationModel | null) =>
        this.handlePreferredNameChange(!!model?.preferredFirstName)
      );
  }

  protected performSubmission(): Observable<void> {
    const partyId = this.partyService.profileStatus?.id;

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

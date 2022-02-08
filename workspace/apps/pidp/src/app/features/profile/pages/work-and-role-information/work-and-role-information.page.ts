import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, of, tap } from 'rxjs';

import { LoggerService } from '@app/core/services/logger.service';

import { AbstractFormPage } from '@core/classes/abstract-form-page.class';
import { FormUtilsService } from '@core/services/form-utils.service';
import { PartyService } from '@core/services/party.service';

import { WorkAndRoleInformationFormState } from './work-and-role-information-form-state';
import { WorkAndRoleInformationResource } from './work-and-role-information-resource.service';
import { WorkAndRoleInformationModel } from './work-and-role-information.model';

@Component({
  selector: 'app-work-and-role-information',
  templateUrl: './work-and-role-information.page.html',
  styleUrls: ['./work-and-role-information.page.scss'],
  viewProviders: [WorkAndRoleInformationResource],
})
export class WorkAndRoleInformationPage
  extends AbstractFormPage<WorkAndRoleInformationFormState>
  implements OnInit
{
  public title: string;
  public formState: WorkAndRoleInformationFormState;
  public hasPhysicalAddress: boolean;

  public constructor(
    protected dialog: MatDialog,
    // TODO replace dialog with dialogService
    // protected dialogService: DialogService,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: WorkAndRoleInformationResource,
    private logger: LoggerService,
    fb: FormBuilder
  ) {
    super(dialog, formUtilsService);

    this.title = this.route.snapshot.data.title;
    this.formState = new WorkAndRoleInformationFormState(fb, formUtilsService);
    this.hasPhysicalAddress = false;
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
        tap((model: WorkAndRoleInformationModel | null) =>
          this.formState.patchValue(model)
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        })
      )
      .subscribe(
        (model: WorkAndRoleInformationModel | null) =>
          (this.hasPhysicalAddress = !!model?.physicalAddress)
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

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

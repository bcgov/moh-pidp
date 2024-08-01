import { NgFor, NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute, Router } from '@angular/router';

import { EMPTY, Observable, catchError, of, tap } from 'rxjs';

import { RegisteredCollege } from '@pidp/data-model';

import {
  AlertComponent,
  AlertContentDirective,
  InjectViewportCssClassDirective,
  PageFooterActionDirective,
} from '@bcgov/shared/ui';

import {
  AbstractFormDependenciesService,
  AbstractFormPage,
} from '@app/core/classes/abstract-form-page.class';
import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { ProfileRoutes } from '@app/features/profile/profile.routes';
import { LookupService } from '@app/modules/lookup/lookup.service';
import { CollegeLookup } from '@app/modules/lookup/lookup.types';
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { CollegeLicenceDeclarationFormState } from './college-licence-declaration-form-state';
import { CollegeLicenceDeclarationResource } from './college-licence-declaration-resource.service';
import { PartyLicenceDeclarationInformation } from './party-licence-declaration-information.model';

@Component({
  selector: 'app-college-licence-declaration',
  templateUrl: './college-licence-declaration.page.html',
  styleUrls: ['./college-licence-declaration.page.scss'],
  viewProviders: [CollegeLicenceDeclarationResource],
  standalone: true,
  imports: [
    AlertComponent,
    AlertContentDirective,
    BreadcrumbComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule,
    NgFor,
    NgIf,
    PageFooterActionDirective,
    ReactiveFormsModule,
  ],
})
export class CollegeLicenceDeclarationPage
  extends AbstractFormPage<CollegeLicenceDeclarationFormState>
  implements OnInit
{
  @Input() public disableCollegeCode: boolean = false;
  @Input() public disableCollegeLicenceNumber: boolean = false;

  public title: string;
  public formState: CollegeLicenceDeclarationFormState;
  public colleges: CollegeLookup[];
  public showOverlayOnSubmit = true;
  public licenceDeclarationFailed = false;
  public disableSearch = false;

  public get showNurseValidationInfo(): boolean {
    const isNurse =
      this.formState.collegeCode.value === RegisteredCollege.Bccnm;
    return isNurse;
  }
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'College Licence', path: '' },
  ];

  public constructor(
    dependenciesService: AbstractFormDependenciesService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceDeclarationResource,
    private logger: LoggerService,
    private lookupService: LookupService,
    fb: FormBuilder,
  ) {
    super(dependenciesService);

    this.title = this.route.snapshot.data.title;
    this.formState = new CollegeLicenceDeclarationFormState(fb);
    this.colleges = this.lookupService.colleges;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public ngOnInit(): void {
    if(this.disableCollegeCode && this.disableCollegeLicenceNumber) {
      this.formState.disableCollegeLicenseForm();
      this.disableSearch = true;
    }
    const partyId = this.partyService.partyId;
    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    this.resource
      .get(partyId)
      .pipe(
        tap((model: PartyLicenceDeclarationInformation | null) =>
          this.formState.patchValue(model),
        ),
        catchError((error: HttpErrorResponse) => {
          if (error.status === HttpStatusCode.NotFound) {
            this.navigateToRoot();
          }
          return of(null);
        }),
      )
      .subscribe();
  }

  protected performSubmission(): Observable<string | null> {
    const partyId = this.partyService.partyId;
    this.formState.disableCollegeLicenseForm();

    return partyId && this.formState.json
      ? this.resource.updateDeclaration(partyId, this.formState.json)
      : EMPTY;
  }

  protected afterSubmitIsSuccessful(cpn: string | null): void {
    if (cpn) {
            this.router.navigate(
        [ProfileRoutes.routePath(ProfileRoutes.COLLEGE_LICENCE_INFO), {showCollegeLicenceDeclarationPage: true}],
        { replaceUrl: true },
      );
    } else if (this.formState.collegeCode.value === 0) {
      this.navigateToRoot();
    } else {
      this.licenceDeclarationFailed = true;
    }
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

import { AsyncPipe, NgFor } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, catchError, map, of, tap } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faAngleRight, faStethoscope } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective, TextButtonDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { PortalAlertComponent } from '@app/features/portal/components/portal-alert/portal-alert.component';
import { ProfileStatusAlert } from '@app/features/portal/models/profile-status-alert.model';
import { ProfileStatus } from '@app/features/portal/models/profile-status.model';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';

import { CollegeCertification } from '../college-licence-declaration/college-certification.model';
import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformationDetailComponent } from './components/college-licence-information-detail.component';

@Component({
  selector: 'app-college-licence-information',
  templateUrl: './college-licence-information.page.html',
  styleUrls: ['./college-licence-information.page.scss'],
  standalone: true,
  imports: [
    AsyncPipe,
    CollegeLicenceInformationDetailComponent,
    FaIconComponent,
    InjectViewportCssClassDirective,
    MatButtonModule,
    NgFor,
    PortalAlertComponent,
    TextButtonDirective
  ],
})
export class CollegeLicenceInformationPage implements OnInit {
  public faStethoscope = faStethoscope;
  public faAngleRight = faAngleRight;

  public title: string;
  public collegeCertifications$!: Observable<CollegeCertification[]>;
  public alerts: ProfileStatusAlert[] = [];

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceInformationResource,
    private logger: LoggerService,
    private portalResource: PortalResource,
    private portalService: PortalService,
  ) {
    this.title = this.route.snapshot.data.title;
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

    this.collegeCertifications$ = this.resource.get(partyId).pipe(
      map((response: CollegeCertification[] | null) => response ?? []),
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.NotFound) {
          this.navigateToRoot();
        }
        return of([]);
      }),
    );

    const profileStatus$ = this.portalResource.getProfileStatus(
      this.partyService.partyId,
    );
    this.updateAlerts(profileStatus$);
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }

  private updateAlerts(profileStatus$: Observable<ProfileStatus | null>): void {
    profileStatus$
      .pipe(
        tap((profileStatus: ProfileStatus | null) => {
          this.portalService.updateState(profileStatus);
          this.alerts = this.portalService.licenceAlerts;
        }),
      )
      .subscribe();
  }
}

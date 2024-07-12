import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable, catchError, map, of } from 'rxjs';

import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faStethoscope } from '@fortawesome/free-solid-svg-icons';

import { InjectViewportCssClassDirective } from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';

import { CollegeCertification } from '../college-licence-declaration/college-certification.model';
import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { CollegeLicenceInformationDetailComponent } from './components/college-licence-information-detail.component';
import { CollegeLicenceDeclarationPage } from '../college-licence-declaration/college-licence-declaration.page';
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
    CollegeLicenceDeclarationPage,
    NgIf,
  ],
})
export class CollegeLicenceInformationPage implements OnInit {
  public faStethoscope = faStethoscope;

  public title: string;
  public collegeCertifications$!: Observable<CollegeCertification[]>;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: CollegeLicenceInformationResource,
    private logger: LoggerService,
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
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

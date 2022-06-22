import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';

import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { EndorsementRequestsReceivedResource } from './endorsement-requests-received-resource.service';

@Component({
  selector: 'app-endorsement-requests-received',
  templateUrl: './endorsement-requests-received.page.html',
  styleUrls: ['./endorsement-requests-received.page.scss'],
})
export class EndorsementRequestsReceivedPage implements OnInit {
  public title: string;
  public completed: boolean | null;

  public constructor(
    protected dialog: MatDialog,
    protected formUtilsService: FormUtilsService,
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: EndorsementRequestsReceivedResource,
    private logger: LoggerService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.completed =
      routeData.endorsementRequestStatusCode === StatusCode.COMPLETED;
  }

  public onBack(): void {
    this.navigateToRoot();
  }
  public onFakeIt(): void {
    console.log('Fakin it');
  }

  public ngOnInit(): void {
    const partyId = this.partyService.partyId;

    if (!partyId) {
      this.logger.error('No party ID was provided');
      return this.navigateToRoot();
    }

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }
  }

  protected afterSubmitIsSuccessful(): void {
    this.navigateToRoot();
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

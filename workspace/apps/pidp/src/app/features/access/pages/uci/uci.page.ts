import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { PartyService } from '@app/core/party/party.service';
import { DocumentService } from '@app/core/services/document.service';
import { LoggerService } from '@app/core/services/logger.service';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { UciResource } from './uci-resource.service';
import { uciUrl } from './uci.constants';

@Component({
  selector: 'app-uci',
  templateUrl: './uci.page.html',
  styleUrls: ['./uci.page.scss'],
})
export class UciPage implements OnInit {
  public title: string;
  public collectionNotice: string;
  public completed: boolean | null;
  public accessRequestFailed: boolean;
  public uciUrl: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partyService: PartyService,
    private resource: UciResource,
    private logger: LoggerService,
    documentService: DocumentService
  ) {
    const routeData = this.route.snapshot.data;
    this.title = routeData.title;
    this.collectionNotice = documentService.getUciCollectionNotice();
    this.completed = routeData.uciStatusCode === StatusCode.COMPLETED;
    this.accessRequestFailed = false;
    this.uciUrl = uciUrl;
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

    if (this.completed === null) {
      this.logger.error('No status code was provided');
      return this.navigateToRoot();
    }
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

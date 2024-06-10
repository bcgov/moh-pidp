import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs';

import {
  FormatDatePipe,
  PageComponent,
  PageFooterActionDirective,
  PageFooterComponent,
  PageHeaderComponent,
  PageSectionComponent,
  PageSectionSubheaderComponent,
  PageSectionSubheaderDescDirective,
} from '@bcgov/shared/ui';

import { PartyService } from '@app/core/party/party.service';
import { LoggerService } from '@app/core/services/logger.service';
import { LookupCodePipe } from '@app/modules/lookup/lookup-code.pipe';

import { Transaction } from './transaction.model';
import { TransactionsResource } from './transactions-resource.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.page.html',
  styleUrls: ['./transactions.page.scss'],
  standalone: true,
  imports: [
    AsyncPipe,
    FormatDatePipe,
    LookupCodePipe,
    MatButtonModule,
    NgIf,
    NgFor,
    PageComponent,
    PageFooterActionDirective,
    PageFooterComponent,
    PageHeaderComponent,
    PageSectionComponent,
    PageSectionSubheaderComponent,
    PageSectionSubheaderDescDirective,
  ],
})
export class TransactionsPage implements OnInit {
  public title: string;
  public transactions$!: Observable<Transaction[]>;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private resource: TransactionsResource,
    private partyService: PartyService,
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

    this.transactions$ = this.resource.transactions(partyId);
  }

  private navigateToRoot(): void {
    this.router.navigate([this.route.snapshot.data.routes.root]);
  }
}

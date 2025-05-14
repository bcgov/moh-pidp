import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

import { Observable } from 'rxjs';

import {
  FormatDatePipe,
  InjectViewportCssClassDirective,
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
import { BreadcrumbComponent } from '@app/shared/components/breadcrumb/breadcrumb.component';

import { Transaction } from './transaction.model';
import { TransactionsResource } from './transactions-resource.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.page.html',
  styleUrls: ['./transactions.page.scss'],
  standalone: true,
  imports: [
    AsyncPipe,
    BreadcrumbComponent,
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
    InjectViewportCssClassDirective,
  ],
})
export class TransactionsPage implements OnInit {
  public title: string;
  public transactions$!: Observable<Transaction[]>;
  public breadcrumbsData: Array<{ title: string; path: string }> = [
    { title: 'Home', path: '' },
    { title: 'History', path: '' },
  ];
  public constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly resource: TransactionsResource,
    private readonly partyService: PartyService,
    private readonly logger: LoggerService,
  ) {
    this.title = this.route.snapshot.data.title;
  }

  public onBack(): void {
    this.navigateToRoot();
  }

  public trackByTransactionId(index: number, transaction: Transaction): string {
    return transaction.requestedOn;
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

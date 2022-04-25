import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { DateTime } from 'luxon';

export interface Transaction {
  date: string;
  description: string;
}

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.page.html',
  styleUrls: ['./transactions.page.scss'],
})
export class TransactionsPage implements OnInit {
  public title: string;
  public transactions: Transaction[];

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
    this.transactions = [];
  }

  public ngOnInit(): void {
    this.transactions = [
      {
        date: DateTime.now().toString(),
        description: 'HCIMWeb Account Transfer submission',
      },
      {
        date: DateTime.now().toString(),
        description: 'Special Authority eForms enrolment submission',
      },
      {
        date: DateTime.now().toString(),
        description: 'Logon to system',
      },
    ];
  }
}

import { NgModule } from '@angular/core';

import { LookupModule } from '@app/modules/lookup/lookup.module';

import { SharedModule } from '@shared/shared.module';

import { TransactionsRoutingModule } from './transactions-routing.module';
import { TransactionsPage } from './transactions.page';

@NgModule({
  imports: [
    TransactionsRoutingModule,
    SharedModule,
    LookupModule.forChild(),
    TransactionsPage,
  ],
})
export class TransactionsModule {}

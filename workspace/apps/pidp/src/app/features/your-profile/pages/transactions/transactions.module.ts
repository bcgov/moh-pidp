import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { TransactionsRoutingModule } from './transactions-routing.module';
import { TransactionsPage } from './transactions.page';

@NgModule({
  declarations: [TransactionsPage],
  imports: [TransactionsRoutingModule, SharedModule],
})
export class TransactionsModule {}

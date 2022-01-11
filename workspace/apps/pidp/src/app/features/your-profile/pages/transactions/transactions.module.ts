import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { TransactionsRoutingModule } from './transactions-routing.module';
import { TransactionsComponent } from './transactions.component';

@NgModule({
  declarations: [TransactionsComponent],
  imports: [TransactionsRoutingModule, SharedModule],
})
export class TransactionsModule {}

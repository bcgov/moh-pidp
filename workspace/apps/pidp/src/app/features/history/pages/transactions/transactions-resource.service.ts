import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { Transaction } from './transaction.model';

@Injectable({
  providedIn: 'root',
})
export class TransactionsResource {
  public constructor(private apiResource: ApiHttpClient) {}

  public transactions(partyId: number): Observable<Transaction[]> {
    return this.apiResource.get<Transaction[]>(
      `parties/${partyId}/access-requests`,
    );
  }
}

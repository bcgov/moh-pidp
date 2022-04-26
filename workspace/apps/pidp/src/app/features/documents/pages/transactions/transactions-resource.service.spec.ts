import { TestBed } from '@angular/core/testing';

import { TransactionsResource } from './transactions-resource.service';

describe('TransactionsResource', () => {
  let service: TransactionsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TransactionsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

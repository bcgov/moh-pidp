import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { TransactionsResource } from './transactions-resource.service';

describe('TransactionsResource', () => {
  let service: TransactionsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        TransactionsResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    service = TestBed.inject(TransactionsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

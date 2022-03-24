import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { ApiHttpClient } from './api-http-client.service';

describe('ApiHttpClient', () => {
  let service: ApiHttpClient;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ApiHttpClient,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    service = TestBed.inject(ApiHttpClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

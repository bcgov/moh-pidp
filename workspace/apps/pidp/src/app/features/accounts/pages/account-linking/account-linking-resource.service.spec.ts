import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AccountLinkingResource } from './account-linking-resource.service';

describe('AccountLinkingResource', () => {
  let service: AccountLinkingResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });
    service = TestBed.inject(AccountLinkingResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

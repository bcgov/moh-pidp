import { TestBed } from '@angular/core/testing';

import { ExternalAccountsResourceService } from './external-accounts-resource.service';

describe('ExternalAccountsResourceService', () => {
  let service: ExternalAccountsResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExternalAccountsResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

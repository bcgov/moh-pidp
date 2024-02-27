import { TestBed } from '@angular/core/testing';

import { AccountLinkingResource } from './account-linking-resource.service';

describe('AccountLinkingResource', () => {
  let service: AccountLinkingResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountLinkingResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

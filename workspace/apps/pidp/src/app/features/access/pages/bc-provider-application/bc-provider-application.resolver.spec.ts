import { TestBed } from '@angular/core/testing';

import { BcProviderApplicationResolver } from './bc-provider-application.resolver';

describe('BcProviderApplicationResolver', () => {
  let resolver: BcProviderApplicationResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(BcProviderApplicationResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

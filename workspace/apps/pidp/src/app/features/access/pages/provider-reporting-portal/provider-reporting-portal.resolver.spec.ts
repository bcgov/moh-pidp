import { TestBed } from '@angular/core/testing';

import { ProviderReportingPortalResolver } from './provider-reporting-portal.resolver';

describe('ProviderReportingPortalResolver', () => {
  let resolver: ProviderReportingPortalResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(ProviderReportingPortalResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

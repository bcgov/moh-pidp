import { TestBed } from '@angular/core/testing';

import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';

describe('ProviderReportingPortalResource', () => {
  let service: ProviderReportingPortalResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProviderReportingPortalResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

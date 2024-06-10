import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';

describe('ProviderReportingPortalResource', () => {
  let service: ProviderReportingPortalResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(ProviderReportingPortalResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

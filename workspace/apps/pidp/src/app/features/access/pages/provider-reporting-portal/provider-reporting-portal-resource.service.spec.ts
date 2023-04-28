import { TestBed } from '@angular/core/testing';

import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { provideAutoSpy } from 'jest-auto-spies';

describe('ProviderReportingPortalResource', () => {
  let service: ProviderReportingPortalResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ]
    });
    service = TestBed.inject(ProviderReportingPortalResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

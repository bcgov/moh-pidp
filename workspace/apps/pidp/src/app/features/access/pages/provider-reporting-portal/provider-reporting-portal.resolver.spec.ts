import { TestBed } from '@angular/core/testing';

import { providerReportingPortalResolver } from './provider-reporting-portal.resolver';
import { provideAutoSpy } from 'jest-auto-spies';
import { PartyService } from '@app/core/party/party.service';
import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';

describe('ProviderReportingPortalResolver', () => {
  let resolver: ProviderReportingPortalResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(ProviderReportingPortalResource),
      ],
    });
    resolver = TestBed.inject(providerReportingPortalResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

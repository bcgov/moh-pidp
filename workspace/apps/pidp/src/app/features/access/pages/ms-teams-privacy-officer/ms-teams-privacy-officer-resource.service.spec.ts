import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';

describe('MsTeamsPrivacyOfficerResource', () => {
  let service: MsTeamsPrivacyOfficerResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(MsTeamsPrivacyOfficerResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

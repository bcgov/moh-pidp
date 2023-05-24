import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { MsTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';
import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';

describe('MsTeamsPrivacyOfficerResolver', () => {
  let resolver: MsTeamsPrivacyOfficerResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(MsTeamsPrivacyOfficerResource),
      ],
    });
    resolver = TestBed.inject(MsTeamsPrivacyOfficerResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

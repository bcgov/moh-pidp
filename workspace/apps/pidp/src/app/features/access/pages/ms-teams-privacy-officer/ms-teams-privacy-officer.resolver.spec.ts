import { TestBed } from '@angular/core/testing';

import { MsTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';

describe('MsTeamsPrivacyOfficerResolver', () => {
  let resolver: MsTeamsPrivacyOfficerResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(MsTeamsPrivacyOfficerResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

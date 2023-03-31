import { TestBed } from '@angular/core/testing';

import { MsTeamsPrivacyOfficerResource } from './ms-teams-privacy-officer-resource.service';

describe('MsTeamsPrivacyOfficerResource', () => {
  let service: MsTeamsPrivacyOfficerResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsTeamsPrivacyOfficerResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

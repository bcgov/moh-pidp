import { TestBed } from '@angular/core/testing';

import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';

describe('MsTeamsClinicMemberResource', () => {
  let service: MsTeamsClinicMemberResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsTeamsClinicMemberResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

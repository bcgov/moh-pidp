import { TestBed } from '@angular/core/testing';

import { MsTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';

describe('MsTeamsClinicMemberResolver', () => {
  let resolver: MsTeamsClinicMemberResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(MsTeamsClinicMemberResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

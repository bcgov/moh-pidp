import { TestBed } from '@angular/core/testing';

import { msTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';
import { provideAutoSpy } from 'jest-auto-spies';
import { MsTeamsClinicMemberResource } from './ms-teams-clinic-member-resource.service';
import { PartyService } from '@app/core/party/party.service';

describe('MsTeamsClinicMemberResolver', () => {
  let resolver: MsTeamsClinicMemberResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(PartyService),
        provideAutoSpy(MsTeamsClinicMemberResource),
      ],
    });
    resolver = TestBed.inject(msTeamsClinicMemberResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

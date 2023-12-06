import { TestBed } from '@angular/core/testing';

import { msTeamsClinicMemberResolver } from './ms-teams-clinic-member.resolver';
import { ResolveFn } from '@angular/router';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

describe('msTeamsClinicMemberResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      msTeamsClinicMemberResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

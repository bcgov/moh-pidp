import { TestBed } from '@angular/core/testing';

import { msTeamsPrivacyOfficerResolver } from './ms-teams-privacy-officer.resolver';
import { ResolveFn } from '@angular/router';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

describe('msTeamsPrivacyOfficerResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      msTeamsPrivacyOfficerResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

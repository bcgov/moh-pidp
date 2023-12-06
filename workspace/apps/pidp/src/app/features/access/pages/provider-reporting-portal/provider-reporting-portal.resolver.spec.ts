import { TestBed } from '@angular/core/testing';

import { providerReportingPortalResolver } from './provider-reporting-portal.resolver';
import { ResolveFn } from '@angular/router';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

describe('providerReportingPortalResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      providerReportingPortalResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { npdpExceptionalCoverageResolver } from './npdp-exceptional-coverage.resolver';

describe('npdpExceptionalCoverageResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      npdpExceptionalCoverageResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

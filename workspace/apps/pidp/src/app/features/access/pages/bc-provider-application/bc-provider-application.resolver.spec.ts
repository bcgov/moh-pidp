import { TestBed } from '@angular/core/testing';

import { bcProviderApplicationResolver } from './bc-provider-application.resolver';
import { ResolveFn } from '@angular/router';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

describe('bcProviderApplicationResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      bcProviderApplicationResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

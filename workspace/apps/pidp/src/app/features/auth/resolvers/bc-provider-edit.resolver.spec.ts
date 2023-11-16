import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { bcProviderEditResolver } from './bc-provider-edit.resolver';

describe('bcProviderEditResolver', () => {
  const executeResolver: ResolveFn<boolean | null | Promise<boolean>> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      bcProviderEditResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

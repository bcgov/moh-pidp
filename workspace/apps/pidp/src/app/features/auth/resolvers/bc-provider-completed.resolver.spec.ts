import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { bcProviderCompletedResolver } from './bc-provider-completed.resolver';

describe('bcProviderCompletedResolver', () => {
  const executeResolver: ResolveFn<boolean | null | Promise<boolean>> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      bcProviderCompletedResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

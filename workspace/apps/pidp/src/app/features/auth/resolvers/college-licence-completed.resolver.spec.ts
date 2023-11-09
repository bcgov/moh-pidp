import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { collegeLicenceCompletedResolver } from './college-licence-completed.resolver';

describe('collegeLicenceCompletedResolver', () => {
  const executeResolver: ResolveFn<boolean | null | Promise<boolean>> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      collegeLicenceCompletedResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { landingActionsResolver } from './landing-actions.resolver';

describe('landingActionsResolver', () => {
  const executeResolver: ResolveFn<null> = (...resolverParameters) =>
    TestBed.runInInjectionContext(() =>
      landingActionsResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

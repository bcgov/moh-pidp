import { TestBed } from '@angular/core/testing';

import { landingActionsResolver } from './landing-actions.resolver';
import { ResolveFn } from '@angular/router';

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

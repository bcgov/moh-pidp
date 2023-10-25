import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { authorizationRedirectGuard } from './authorization-redirect.guard';

describe('authorizationRedirectGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      authorizationRedirectGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

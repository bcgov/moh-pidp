import { TestBed } from '@angular/core/testing';

import { AuthorizationRedirectGuard } from './authorization-redirect.guard';

describe('AuthorizationRedirectGuard', () => {
  let guard: AuthorizationRedirectGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthorizationRedirectGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { CanActivateChildFn, CanActivateFn, CanMatchFn } from '@angular/router';

import { AuthenticationGuard } from './authentication.guard';

describe('AuthenticationGuard canActivate', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      AuthenticationGuard.canActivate(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

describe('AuthenticationGuard canActivateChild', () => {
  const executeGuard: CanActivateChildFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      AuthenticationGuard.canActivateChild(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

describe('AuthenticationGuard CanMatchFn', () => {
  const executeGuard: CanMatchFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      AuthenticationGuard.canMatch(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { CanActivateChildFn, CanActivateFn, CanMatchFn } from '@angular/router';

import {
  canActivateChildPermissionsGuard,
  canActivatePermissionsGuard,
  canMatchPermissionsGuard,
} from './permissions.guard';

describe('canActivatePermissionsGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      canActivatePermissionsGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

describe('canActivateChildPermissionsGuard', () => {
  const executeGuard: CanActivateChildFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      canActivateChildPermissionsGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

describe('canMatchPermissionsGuard', () => {
  const executeGuard: CanMatchFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      canMatchPermissionsGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

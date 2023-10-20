import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { redirectOnFeatureFlagConfigGuard } from './redirect-on-feature-flag.guard';

describe('redirectOnFeatureFlagConfigGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      redirectOnFeatureFlagConfigGuard(...guardParameters)
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

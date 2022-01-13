import { TestBed } from '@angular/core/testing';

import { FeatureFlagCanLoadGuard } from './feature-flag-can-load.guard';

describe('FeatureFlagCanLoadGuard', () => {
  let guard: FeatureFlagCanLoadGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(FeatureFlagCanLoadGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

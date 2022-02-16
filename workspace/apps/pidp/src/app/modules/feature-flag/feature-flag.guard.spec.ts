import { TestBed } from '@angular/core/testing';

import { FeatureFlagGuard } from './feature-flag.guard';

describe('FeatureFlagGuard', () => {
  let guard: FeatureFlagGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(FeatureFlagGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { PermissionsGuard } from './permissions.guard';

describe('PermissionsGuard', () => {
  let guard: PermissionsGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PermissionsGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

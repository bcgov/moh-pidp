import { TestBed } from '@angular/core/testing';

import { SaEformsAccessRequestGuard } from './sa-eforms-access-request.guard';

describe('SaEformsAccessRequestGuard', () => {
  let guard: SaEformsAccessRequestGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(SaEformsAccessRequestGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

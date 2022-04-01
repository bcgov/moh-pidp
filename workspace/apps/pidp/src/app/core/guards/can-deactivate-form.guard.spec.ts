import { TestBed } from '@angular/core/testing';

import { CanDeactivateFormGuard } from './can-deactivate-form.guard';

describe('CanDeactivateFormGuard', () => {
  let guard: CanDeactivateFormGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CanDeactivateFormGuard],
    });

    guard = TestBed.inject(CanDeactivateFormGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

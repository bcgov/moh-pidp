import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { setDashboardTitleGuard } from './set-dashboard-title.guard';

describe('setDashboardTitleGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() =>
      setDashboardTitleGuard(...guardParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

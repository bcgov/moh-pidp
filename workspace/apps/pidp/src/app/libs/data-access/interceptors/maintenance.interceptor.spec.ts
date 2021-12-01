import { TestBed } from '@angular/core/testing';

import { MaintenanceInterceptor } from './maintenance.interceptor';

describe('MaintenanceInterceptor', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      providers: [MaintenanceInterceptor],
    })
  );

  it('should be created', () => {
    const interceptor: MaintenanceInterceptor = TestBed.inject(
      MaintenanceInterceptor
    );
    expect(interceptor).toBeTruthy();
  });
});

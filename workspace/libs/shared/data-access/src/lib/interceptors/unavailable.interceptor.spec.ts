import { TestBed } from '@angular/core/testing';

import { UnavailableInterceptor } from './unavailable.interceptor';

describe('UnavailableInterceptor', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      providers: [UnavailableInterceptor],
    })
  );

  it('should be created', () => {
    const interceptor: UnavailableInterceptor = TestBed.inject(
      UnavailableInterceptor
    );
    expect(interceptor).toBeTruthy();
  });
});

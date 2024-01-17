import { TestBed } from '@angular/core/testing';

import { ServerErrorInterceptor } from './server-error.interceptor';

describe('ServerErrorInterceptor', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      providers: [ServerErrorInterceptor],
    }),
  );

  it('should be created', () => {
    const interceptor: ServerErrorInterceptor = TestBed.inject(
      ServerErrorInterceptor,
    );
    expect(interceptor).toBeTruthy();
  });
});

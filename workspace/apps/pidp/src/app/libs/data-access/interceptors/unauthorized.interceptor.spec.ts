import { TestBed } from '@angular/core/testing';

import { UnauthorizedInterceptor } from './unauthorized.interceptor';

describe('UnauthorizedInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      UnauthorizedInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: UnauthorizedInterceptor = TestBed.inject(UnauthorizedInterceptor);
    expect(interceptor).toBeTruthy();
  });
});

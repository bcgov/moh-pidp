import { TestBed } from '@angular/core/testing';

import { UciResolver } from './uci.resolver';

describe('UciResolver', () => {
  let resolver: UciResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(UciResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

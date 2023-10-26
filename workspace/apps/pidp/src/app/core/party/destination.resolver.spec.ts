import { TestBed } from '@angular/core/testing';

import { DestinationResolver } from './destination.resolver';

describe('DestinationResolver', () => {
  let resolver: DestinationResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(DestinationResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

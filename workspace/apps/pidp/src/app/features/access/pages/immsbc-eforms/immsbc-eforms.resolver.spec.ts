import { TestBed } from '@angular/core/testing';

import { ImmsBCEformsResolver } from './immsbc-eforms.resolver';

describe('ImmsBCEformsResolver', () => {
  let resolver: ImmsBCEformsResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(ImmsBCEformsResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { haloResolver } from './halo.resolver';

describe('haloResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => haloResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

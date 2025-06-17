import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { ivfResolver } from './ivf.resolver';

describe('ivfResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => ivfResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

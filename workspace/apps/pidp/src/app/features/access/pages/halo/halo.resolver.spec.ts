import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { haloResolver } from './halo.resolver';

describe('haloResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) => TestBed.runInInjectionContext(() => haloResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

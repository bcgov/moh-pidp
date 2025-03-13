import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { ImmsbcResolver } from './immsbc.resolver';

describe('ImmsbcResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      ImmsbcResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

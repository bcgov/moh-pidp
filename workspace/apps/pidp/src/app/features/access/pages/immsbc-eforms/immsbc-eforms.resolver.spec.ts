import { TestBed } from '@angular/core/testing';

import { immsBCEformsResolver } from './immsbc-eforms.resolver';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';
import { ResolveFn } from '@angular/router';

describe('immsBCEformsResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      immsBCEformsResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

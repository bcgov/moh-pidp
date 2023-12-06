import { TestBed } from '@angular/core/testing';

import { prescriptionRefillEformsResolver } from './prescription-refill-eforms.resolver';
import { ResolveFn } from '@angular/router';
import { StatusCode } from '@app/features/portal/enums/status-code.enum';

describe('prescriptionRefillEformsResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      prescriptionRefillEformsResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

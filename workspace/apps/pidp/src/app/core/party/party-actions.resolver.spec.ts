import { TestBed } from '@angular/core/testing';

import { partyActionsResolver } from './party-actions.resolver';
import { ResolveFn } from '@angular/router';

describe('partyActionsResolver', () => {
  const executeResolver: ResolveFn<number | null> = (...resolverParameters) =>
    TestBed.runInInjectionContext(() =>
      partyActionsResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { partyActionsResolver } from './party-actions.resolver';

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

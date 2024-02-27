import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { accountLinkingResolver } from './account-linking.resolver';

describe('accountLinkingResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => accountLinkingResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

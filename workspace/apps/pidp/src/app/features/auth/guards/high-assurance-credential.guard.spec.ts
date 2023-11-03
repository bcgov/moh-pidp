import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { highAssuranceCredentialGuard } from './high-assurance-credential.guard';

describe('highAssuranceCredentialGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => highAssuranceCredentialGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

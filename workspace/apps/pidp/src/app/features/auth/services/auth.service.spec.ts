import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import Keycloak from 'keycloak-js';

import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService, provideAutoSpy(Keycloak)],
    });
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

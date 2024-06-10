import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { AuthService } from '../../services/auth.service';
import { AuthorizationRedirectGuardService } from './authorization-redirect-guard.service';

describe('AuthorizationRedirectGuardService', () => {
  let service: AuthorizationRedirectGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AuthorizationRedirectGuardService,
        provideAutoSpy(AuthService),
      ],
    });

    service = TestBed.inject(AuthorizationRedirectGuardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

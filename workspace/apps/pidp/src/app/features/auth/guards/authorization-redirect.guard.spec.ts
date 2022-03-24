import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { AuthService } from '../services/auth.service';
import { AuthorizationRedirectGuard } from './authorization-redirect.guard';

describe('AuthorizationRedirectGuard', () => {
  let guard: AuthorizationRedirectGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [AuthorizationRedirectGuard, provideAutoSpy(AuthService)],
    });

    guard = TestBed.inject(AuthorizationRedirectGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

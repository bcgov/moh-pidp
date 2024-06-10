import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { AuthService } from '../../services/auth.service';
import { AuthenticationGuardService } from './authentication-guard.service';

describe('AuthenticationGuardService', () => {
  let guard: AuthenticationGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [AuthenticationGuardService, provideAutoSpy(AuthService)],
    });
    guard = TestBed.inject(AuthenticationGuardService);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

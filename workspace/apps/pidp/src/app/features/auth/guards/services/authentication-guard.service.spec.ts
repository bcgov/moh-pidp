import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { AuthService } from '../../services/auth.service';
import { AuthenticationGuardService } from './authentication-guard.service';

describe('AuthenticationGuardService', () => {
  let guard: AuthenticationGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
      providers: [AuthenticationGuardService, provideAutoSpy(AuthService)],
    });
    guard = TestBed.inject(AuthenticationGuardService);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

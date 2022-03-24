import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { AccessTokenService } from './access-token.service';
import { AuthorizedUserService } from './authorized-user.service';

describe('AuthorizedUserService', () => {
  let service: AuthorizedUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthorizedUserService, provideAutoSpy(AccessTokenService)],
    });

    service = TestBed.inject(AuthorizedUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

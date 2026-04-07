import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { AccessTokenService } from '@app/features/auth/services/access-token.service';

import { PermissionsService } from './permissions.service';

describe('PermissionsService', () => {
  let service: PermissionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
      providers: [
        PermissionsService,
        provideAutoSpy(AccessTokenService),
        provideAutoSpy(PermissionsService),
      ],
    });

    service = TestBed.inject(PermissionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PermissionsService } from '@app/modules/permissions/permissions.service';

import { AuthorizedUserService } from '../auth/services/authorized-user.service';
import { PortalService } from './portal.service';

describe('PortalService', () => {
  let service: PortalService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        PortalService,
        provideAutoSpy(AuthorizedUserService),
        provideAutoSpy(PermissionsService),
      ],
    });

    service = TestBed.inject(PortalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

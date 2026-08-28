import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { provideAutoSpy, Spy } from 'jest-auto-spies';
import Keycloak from 'keycloak-js';
import { of } from 'rxjs';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PartyService } from '@app/core/party/party.service';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';
import { PortalService } from '@app/features/portal/portal.service';

import { AccessRequestsPage } from './access-requests.page';

describe('AccessRequestsPage', () => {
  let component: AccessRequestsPage;
  let portalResourceSpy: Spy<PortalResource>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [NoopAnimationsModule],
      providers: [
        AccessRequestsPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: AuthorizedUserService,
          useValue: {
            identityProvider$: of('BCSC'),
          },
        },
        {
          provide: PartyService,
          useValue: { partyId: 1 },
        },
        {
          provide: PortalService,
          useValue: {
            accessState$: of({ access: [] }),
            updateState: jest.fn(),
          },
        },
        provideAutoSpy(PortalResource),
        provideAutoSpy(HttpClient),
        provideAutoSpy(Keycloak),
      ],
    });

    portalResourceSpy = TestBed.inject<any>(PortalResource);
    portalResourceSpy.getProfileStatus.nextWith({} as any);

    component = TestBed.inject(AccessRequestsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

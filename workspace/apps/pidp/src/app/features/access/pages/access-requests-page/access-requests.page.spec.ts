/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AccessRequestsPage } from './access-requests.page';

describe('PortalCardComponent', () => {
  let component: AccessRequestsPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [NoopAnimationsModule],
      providers: [
        AccessRequestsPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(KeycloakService),
      ],
    });
    component = TestBed.inject(AccessRequestsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

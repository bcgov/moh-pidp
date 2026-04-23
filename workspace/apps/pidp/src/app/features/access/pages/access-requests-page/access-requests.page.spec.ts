/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { provideAutoSpy } from 'jest-auto-spies';
import Keycloak from 'keycloak-js';

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
        provideAutoSpy(HttpClient),
        provideAutoSpy(Keycloak),
      ],
    });
    component = TestBed.inject(AccessRequestsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

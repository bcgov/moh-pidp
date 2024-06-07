import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { LinkAccountConfirmResource } from './link-account-confirm-resource.service';

describe('LinkAccountConfirmResource', () => {
  let service: LinkAccountConfirmResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(KeycloakService),
      ],
    });
    service = TestBed.inject(LinkAccountConfirmResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

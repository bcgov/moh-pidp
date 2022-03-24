import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { KeycloakInitService } from './keycloak-init.service';

describe('KeycloakInitService', () => {
  let service: KeycloakInitService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(KeycloakService),
      ],
    });

    service = TestBed.inject(KeycloakInitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

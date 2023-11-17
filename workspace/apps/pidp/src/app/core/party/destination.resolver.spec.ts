import { TestBed } from '@angular/core/testing';

import { destinationResolver } from './destination.resolver';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { HttpClient } from '@angular/common/http';
import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

describe('destinationResolver', () => {
  let resolver: DestinationResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(KeycloakService),
      ],
    });
    resolver = TestBed.inject(destinationResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

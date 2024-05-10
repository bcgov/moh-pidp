/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { LinkAccountErrorPage } from './link-account-error.page';

describe('LinkAccountErrorPage', () => {
  let component: LinkAccountErrorPage;
  let mockActivatedRoute: { snapshot: any };

  beforeEach(async () => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        LinkAccountErrorPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Router),
        provideAutoSpy(KeycloakService),
      ],
    });
    component = TestBed.inject(LinkAccountErrorPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

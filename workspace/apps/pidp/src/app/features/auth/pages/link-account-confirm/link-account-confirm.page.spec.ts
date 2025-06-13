/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { of } from 'rxjs';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';
import Keycloak from 'keycloak-js';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AuthorizedUserService } from '../../services/authorized-user.service';
import { LinkAccountConfirmPage } from './link-account-confirm.page';

describe('LinkAccountConfirmPage', () => {
  let component: LinkAccountConfirmPage;
  let mockActivatedRoute: { snapshot: any; queryParams: any };

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
      queryParams: of({ param1: 'value1' }),
    };

    TestBed.configureTestingModule({
      imports: [LinkAccountConfirmPage],
      providers: [
        LinkAccountConfirmPage,
        provideAutoSpy(HttpClient),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(AuthorizedUserService),
        provideAutoSpy(Keycloak),
      ],
    });

    component = TestBed.inject(LinkAccountConfirmPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

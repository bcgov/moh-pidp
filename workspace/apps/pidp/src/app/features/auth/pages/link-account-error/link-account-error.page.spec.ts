/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { of } from 'rxjs';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { ShellRoutes } from '@app/features/shell/shell.routes';

import { LinkAccountErrorPage } from './link-account-error.page';

describe('LinkAccountErrorPage', () => {
  let component: LinkAccountErrorPage;
  let mockActivatedRoute: { snapshot: any; queryParams: any };
  let router: Router;

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
      queryParams: of({ param1: 'value1' }),
    };

    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
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
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('METHOD: onBack', () => {
    given('user wants to go back to the previous page', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then('router should navigate to root route', () => {
          expect(router.navigate).toHaveBeenCalledWith([ShellRoutes.BASE_PATH]);
        });
      });
    });
  });
});

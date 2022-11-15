import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { SupportErrorPage } from './support-error.page';

describe('SupportErrorPage', () => {
  let component: SupportErrorPage;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SupportErrorPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(Router),
      ],
    });
    router = TestBed.inject(Router);
    component = TestBed.inject(SupportErrorPage);
  });

  describe('METHOD: onBack', () => {
    given('user wants to be redirected back to the application', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then('router should navigate to the module root route', () => {
          expect(router.navigate).toHaveBeenCalledWith(['']);
        });
      });
    });
  });
});

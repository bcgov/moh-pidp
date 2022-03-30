import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { SupportErrorPage } from './support-error.page';

describe('SupportErrorPage', () => {
  let component: SupportErrorPage;
  let router: Router;

  const mockActivatedRoute = {
    shellRoutes: {
      MODULE_PATH: '',
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SupportErrorPage, provideAutoSpy(Router)],
    });
    router = TestBed.inject(Router);
    component = TestBed.inject(SupportErrorPage);
  });

  describe('METHOD: onBack', () => {
    given('user wants to navigate back to the PIDP', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then('router should navigate to the module root route', () => {
          const moduleRootRoute = mockActivatedRoute.shellRoutes.MODULE_PATH;
          expect(router.navigate).toHaveBeenCalledWith([moduleRootRoute]);
        });
      });
    });
  });
});

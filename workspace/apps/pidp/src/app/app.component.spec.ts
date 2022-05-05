/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, RouterEvent } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { Subject } from 'rxjs';

import { randTextRange } from '@ngneat/falso';
import { Spy, createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { AppComponent } from './app.component';
import { RouteStateService } from './core/services/route-state.service';
import { UtilsService } from './core/services/utils.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let titleServiceSpy: Spy<Title>;
  // let routeStateServiceSpy: Spy<RouteStateService>;
  const routerEventsSubject = new Subject<RouterEvent>();
  const navigationEnd = new NavigationEnd(1, '', '');
  // let router: Router;

  let mockActivatedRoute: { snapshot: any };

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
    };

    let titleServiceSpy = createSpyFromClass(Title, {
      methodsToSpyOn: ['getTitle', 'setTitle'],
    });

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AppComponent,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: Title,
          useValue: titleServiceSpy,
        },
        provideAutoSpy(RouteStateService),
        provideAutoSpy(UtilsService),
        // provideAutoSpy(Router),
      ],
    });

    component = TestBed.inject(AppComponent);
    titleServiceSpy = TestBed.inject<any>(Title);
    // routeStateServiceSpy = TestBed.inject<any>(RouteStateService);
    // router = TestBed.inject(Router);
  });

  describe('INIT', () => {
    given('component initializes', () => {
      component.ngOnInit();
      when('navigation ends and set title has been called', () => {
        // routeStateServiceSpy.accessorSpies.getters.onNavigationStart;
        // routeStateServiceSpy.accessorSpies.getters.onNavigationEnd;
        routerEventsSubject.next(navigationEnd);

        then('should have the correct title', () => {
          expect(titleServiceSpy.setTitle).toHaveBeenCalledTimes(1);
          expect(titleServiceSpy.getTitle).toReturnWith(component.title);
        });
      });
    });
  });
});

/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data, NavigationEnd, Scroll } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { Observable, of } from 'rxjs';

import { randText } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { contentContainerSelector } from '@bcgov/shared/ui';

import { AppComponent } from './app.component';
import { RouteStateService } from './core/services/route-state.service';
import { UtilsService } from './core/services/utils.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let titleServiceSpy: Spy<Title>;
  let routeStateServiceSpy: Spy<RouteStateService>;
  let utilsServiceSpy: Spy<UtilsService>;

  let mockActivatedRoute: {
    fragment?: Observable<string | null>;
    data: Observable<Data>;
    snapshot: any;
  };

  beforeEach(() => {
    const data: Data = {
      title: randText(),
    };
    mockActivatedRoute = {
      data: of(data),
      snapshot: { data },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AppComponent,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Title),
        provideAutoSpy(RouteStateService),
        provideAutoSpy(UtilsService),
      ],
    });

    component = TestBed.inject(AppComponent);
    titleServiceSpy = TestBed.inject<any>(Title);
    routeStateServiceSpy = TestBed.inject<any>(RouteStateService);
    utilsServiceSpy = TestBed.inject<any>(UtilsService);
  });

  describe('INIT', () => {
    given('there is an initial route', () => {
      const navigationEnd = new NavigationEnd(0, '/', '/');
      routeStateServiceSpy.onNavigationEnd.nextOneTimeWith(navigationEnd);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(
        new Scroll(navigationEnd, [0, 0], null)
      );

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the title should be set using the route config', () => {
          expect(titleServiceSpy.setTitle).toHaveBeenCalledTimes(2);
          expect(titleServiceSpy.setTitle).toHaveBeenCalledWith(
            mockActivatedRoute.snapshot.data.title
          );
        });
      });
    });

    given('there is an initial route with no route fragment', () => {
      const navigationEnd = new NavigationEnd(0, '/', '/');
      routeStateServiceSpy.onNavigationEnd.nextOneTimeWith(navigationEnd);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(
        new Scroll(navigationEnd, [0, 0], null)
      );

      when('the component has been initialized', async () => {
        component.ngOnInit();
        await new Promise((x) => setTimeout(x, 500));

        then('the view should be scrolled to the top', () => {
          expect(utilsServiceSpy.scrollTop).toHaveBeenCalledTimes(2);
          expect(utilsServiceSpy.scrollTop).toHaveBeenCalledWith(
            contentContainerSelector
          );
        });
      });
    });

    given('there is an initial route with a route fragment', () => {
      const anchor = randText();
      mockActivatedRoute.fragment = of(anchor);
      const navigationEnd = new NavigationEnd(0, '/', '/');
      routeStateServiceSpy.onNavigationEnd.nextOneTimeWith(navigationEnd);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(
        new Scroll(navigationEnd, [0, 0], anchor)
      );

      when('the component has been initialized', async () => {
        component.ngOnInit();
        await new Promise((x) => setTimeout(x, 500));

        then('the view should be scrolled to an anchor', () => {
          expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledTimes(2);
          expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledWith(anchor);
        });
      });
    });
  });
});

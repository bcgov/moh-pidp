/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import {
  ActivatedRoute,
  Data,
  NavigationEnd,
  RouterModule,
  Scroll,
} from '@angular/router';

import { Observable, of } from 'rxjs';

import { randText } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { contentContainerSelector } from '@bcgov/shared/ui';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AppComponent } from './app.component';
import { RouteStateService } from './core/services/route-state.service';
import { SnowplowService } from './core/services/snowplow.service';
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
      imports: [RouterModule.forRoot([])],
      providers: [
        AppComponent,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(Title),
        provideAutoSpy(RouteStateService),
        provideAutoSpy(UtilsService),
        provideAutoSpy(SnowplowService),
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
        new Scroll(navigationEnd, [0, 0], null),
      );

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the title should be set using the route config', () => {
          // Trigger assertion after change detection
          setTimeout(() => {
            expect(titleServiceSpy.setTitle).toHaveBeenCalledTimes(1);
            expect(titleServiceSpy.setTitle).toHaveBeenCalledWith(
              mockActivatedRoute.snapshot.data.title,
            );
          }, 0);
        });
      });
    });

    given('there is an initial route with no route fragment', () => {
      const navigationEnd = new NavigationEnd(0, '/', '/');
      routeStateServiceSpy.onNavigationEnd.nextOneTimeWith(navigationEnd);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(
        new Scroll(navigationEnd, [0, 0], null),
      );

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the view should be scrolled to the top', () => {
          // Trigger assertion after change detection
          setTimeout(() => {
            expect(utilsServiceSpy.scrollTop).toHaveBeenCalledTimes(1);
            expect(utilsServiceSpy.scrollTop).toHaveBeenCalledWith(
              contentContainerSelector,
            );
          }, 0);
        });
      });
    });

    given('there is an initial route with a route fragment', () => {
      const anchor = randText();
      mockActivatedRoute.fragment = of(anchor);
      const navigationEnd = new NavigationEnd(0, '/', '/');
      routeStateServiceSpy.onNavigationEnd.nextOneTimeWith(navigationEnd);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(
        new Scroll(navigationEnd, [0, 0], anchor),
      );

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the view should be scrolled to an anchor', () => {
          // Trigger assertion after change detection
          setTimeout(() => {
            expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledTimes(1);
            expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledWith(anchor);
          }, 0);
        });
      });
    });
  });
});

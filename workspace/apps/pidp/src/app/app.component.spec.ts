/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data, NavigationEnd, Scroll } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { of } from 'rxjs';

import { randText } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { AppComponent } from './app.component';
import { RouteStateService } from './core/services/route-state.service';
import { UtilsService } from './core/services/utils.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let titleServiceSpy: Spy<Title>;
  let routeStateServiceSpy: Spy<RouteStateService>;
  let utilsServiceSpy: Spy<UtilsService>;

  let mockActivatedRoute: { data: any; snapshot: any };

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
        provideAutoSpy(RouteStateService),
        provideAutoSpy(UtilsService),
        provideAutoSpy(Title),
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

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the title should be set using the route config', () => {
          expect(titleServiceSpy.setTitle).toHaveBeenCalledTimes(1);
          expect(titleServiceSpy.setTitle).toBeCalledWith(
            mockActivatedRoute.snapshot.data.title
          );
        });
      });
    });

    given('there is an initial route with a route fragment', () => {
      const navigationEnd = new NavigationEnd(0, '/', '/');
      const anchor = randText();
      const scrollEvent = new Scroll(navigationEnd, [100, 100], anchor);
      routeStateServiceSpy.onScrollEvent.nextOneTimeWith(scrollEvent);

      when('the component has been initialized', () => {
        component.ngOnInit();

        then('the view should be scrolled to the anchor', () => {
          expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledTimes(1);
          expect(utilsServiceSpy.scrollToAnchor).toHaveBeenCalledWith(anchor);
        });
      });
    });

    // given('there is an initial route with no route fragment', () => {
    //   when('the component has been initialized', () => {
    //     then('the view should be scrolled to the top', () => {});
    //     // contentContainerSelector
    //   });
    // });
  });
});

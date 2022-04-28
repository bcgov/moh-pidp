import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { Spy, provideAutoSpy } from 'jest-auto-spies';

import { AppComponent } from './app.component';
import { RouteStateService } from './core/services/route-state.service';
import { UtilsService } from './core/services/utils.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let titleServiceSpy: Spy<Title>;
  let routeStateServiceSpy: Spy<RouteStateService>;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
        },
      },
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
  });

  describe('INIT', () => {
    given('component initializes', () => {
      component.ngOnInit();
      when('navigation ends and set title has been called', () => {
        routeStateServiceSpy.onNavigationEnd();

        then('should have the correct title', () => {
          expect(titleServiceSpy.setTitle).toHaveBeenCalledTimes(1);
          expect(titleServiceSpy.getTitle).toReturnWith(component.title);
        });
      });
    });
  });
});

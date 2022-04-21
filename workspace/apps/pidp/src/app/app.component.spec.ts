import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';

import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AppComponent,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(AppComponent);
  });

  describe('INIT', () => {
    given('component initializes', () => {
      when('navigation ends', () => {
        component.ngOnInit();

        then('should set the title', () => {
          expect(component.title).toBe('Provider Identity Portal');
        });
      });
    });
  });
});

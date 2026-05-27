import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { HelpPage } from './help.page';

describe('HelpPage', () => {
  let component: HelpPage;

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
      imports: [RouterModule.forRoot([])],
      providers: [
        HelpPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(HelpPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

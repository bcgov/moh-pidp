import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { MfaSetupPage } from './mfa-setup.page';

describe('MfaSetupPage', () => {
  let component: MfaSetupPage;

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
        MfaSetupPage,
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

    component = TestBed.inject(MfaSetupPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';

import { UserAccessAgreementPage } from './user-access-agreement.page';

describe('UserAccessAgreementPage', () => {
  let component: UserAccessAgreementPage;

  let mockActivatedRoute;

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
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        UserAccessAgreementPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(UserAccessAgreementPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { UserAccessAgreementPage } from './user-access-agreement.page';
import { AccessTokenService } from '@app/features/auth/services/access-token.service';

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
        provideAutoSpy(Router),
        provideAutoSpy(AccessTokenService)
      ],
    });

    component = TestBed.inject(UserAccessAgreementPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

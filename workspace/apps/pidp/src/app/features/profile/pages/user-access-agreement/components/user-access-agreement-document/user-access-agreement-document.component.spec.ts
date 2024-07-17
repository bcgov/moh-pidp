/* eslint-disable @typescript-eslint/no-explicit-any */import { TestBed } from '@angular/core/testing';
import { randTextRange } from '@ngneat/falso';

import { UserAccessAgreementDocumentComponent } from './user-access-agreement-document.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';

describe('UserAccessAgreementDocumentComponent', () => {
  let component: UserAccessAgreementDocumentComponent;
  let mockActivatedRoute: { snapshot: any; queryParams: any };

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
      queryParams: of({ param1: 'value1' }),
    };
    TestBed.configureTestingModule({
      providers: [UserAccessAgreementDocumentComponent,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(UserAccessAgreementDocumentComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

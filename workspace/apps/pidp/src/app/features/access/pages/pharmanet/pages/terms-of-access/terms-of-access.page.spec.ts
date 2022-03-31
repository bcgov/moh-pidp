import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { TermsOfAccessPage } from './terms-of-access.page';

describe('TermsOfAccessPage', () => {
  let component: TermsOfAccessPage;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
        routes: {
          root: '../../',
        },
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TermsOfAccessPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(TermsOfAccessPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

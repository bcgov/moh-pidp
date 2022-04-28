import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { TransactionsPage } from './transactions.page';

describe('TransactionsPage', () => {
  let component: TransactionsPage;

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
      providers: [
        TransactionsPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(TransactionsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

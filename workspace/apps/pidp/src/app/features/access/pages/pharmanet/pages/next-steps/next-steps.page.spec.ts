import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { NextStepsPage } from './next-steps.page';

describe('NextStepsPage', () => {
  let component: NextStepsPage;

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
        NextStepsPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(NextStepsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

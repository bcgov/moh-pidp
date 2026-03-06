import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { ComplianceTrainingPage } from './compliance-training.page';

describe('ComplianceTrainingPage', () => {
  let component: ComplianceTrainingPage;

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
      imports: [RouterModule.forRoot([])],
      providers: [
        ComplianceTrainingPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(ComplianceTrainingPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

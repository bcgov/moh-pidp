import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { SelfDeclarationPage } from './self-declaration.page';

describe('SelfDeclarationPage', () => {
  let component: SelfDeclarationPage;

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
        SelfDeclarationPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(SelfDeclarationPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

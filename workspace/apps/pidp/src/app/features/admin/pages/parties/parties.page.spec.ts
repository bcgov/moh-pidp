import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { AdminResource } from '../../shared/resources/admin-resource.service';
import { PartiesPage } from './parties.page';

describe('PartiesComponent', () => {
  let component: PartiesPage;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            auth: '/auth/admin',
          },
        },
      },
    };

    TestBed.configureTestingModule({
      providers: [
        PartiesPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(AdminResource),
      ],
    });

    component = TestBed.inject(PartiesPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

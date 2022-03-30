import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { SupportErrorPage } from './support-error.page';

describe('SupportErrorPage', () => {
  let component: SupportErrorPage;

  const mockActivatedRoute = {
    shellRoutes: {
      MODULE_PATH: '',
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SupportErrorPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(Router),
      ],
    });
    component = TestBed.inject(SupportErrorPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

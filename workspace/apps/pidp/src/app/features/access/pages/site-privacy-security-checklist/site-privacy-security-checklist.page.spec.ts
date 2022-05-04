import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

describe('SitePrivacySecurityChecklistPage', () => {
  let component: SitePrivacySecurityChecklistPage;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
        },
      },
    };

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        SitePrivacySecurityChecklistPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    });

    component = TestBed.inject(SitePrivacySecurityChecklistPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

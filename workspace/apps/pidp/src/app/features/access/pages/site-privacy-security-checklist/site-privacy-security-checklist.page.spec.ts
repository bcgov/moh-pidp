import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

describe('SitePrivacySecurityChecklistPage', () => {
  let component: SitePrivacySecurityChecklistPage;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
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

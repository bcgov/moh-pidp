import { TestBed } from '@angular/core/testing';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

describe('SitePrivacySecurityChecklistPage', () => {
  let component: SitePrivacySecurityChecklistPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SitePrivacySecurityChecklistPage],
    });

    component = TestBed.inject(SitePrivacySecurityChecklistPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

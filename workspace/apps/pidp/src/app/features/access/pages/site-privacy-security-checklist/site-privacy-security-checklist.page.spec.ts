import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SitePrivacySecurityChecklistPage } from './site-privacy-security-checklist.page';

describe('SitePrivacySecurityChecklistPage', () => {
  let component: SitePrivacySecurityChecklistPage;
  let fixture: ComponentFixture<SitePrivacySecurityChecklistPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SitePrivacySecurityChecklistPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SitePrivacySecurityChecklistPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

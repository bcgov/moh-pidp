import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SitePrivacySecurityChecklistComponent } from './site-privacy-security-checklist.component';

describe('SitePrivacySecurityChecklistComponent', () => {
  let component: SitePrivacySecurityChecklistComponent;
  let fixture: ComponentFixture<SitePrivacySecurityChecklistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SitePrivacySecurityChecklistComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SitePrivacySecurityChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

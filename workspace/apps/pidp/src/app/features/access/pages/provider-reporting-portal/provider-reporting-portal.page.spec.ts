import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderReportingPortalPage } from './provider-reporting-portal.page';

describe('ProviderReportingPortalPage', () => {
  let component: ProviderReportingPortalPage;
  let fixture: ComponentFixture<ProviderReportingPortalPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProviderReportingPortalPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProviderReportingPortalPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

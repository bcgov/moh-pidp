import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ProviderReportingPortalPage } from './provider-reporting-portal.page';
import { ActivatedRoute, Router } from '@angular/router';
import { PartyService } from '@app/core/party/party.service';
import { ProviderReportingPortalResource } from './provider-reporting-portal-resource.service';
import { LoggerService } from '@app/core/services/logger.service';
import { DocumentService } from '@app/core/services/document.service';

describe('ProviderReportingPortalPage', () => {
  let component: ProviderReportingPortalPage;
  let fixture: ComponentFixture<ProviderReportingPortalPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProviderReportingPortalPage ],
      providers: [
        provideAutoSpy(ActivatedRoute),
        provideAutoSpy(Router),
        provideAutoSpy(PartyService),
        provideAutoSpy(ProviderReportingPortalResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
      ]
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

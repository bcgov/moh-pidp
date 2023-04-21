import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';
import { ActivatedRoute, Router } from '@angular/router';
import { PartyService } from '@app/core/party/party.service';
import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';
import { LoggerService } from '@app/core/services/logger.service';
import { DocumentService } from '@app/core/services/document.service';

describe('PrescriptionRefillEformsPage', () => {
  let component: PrescriptionRefillEformsPage;
  let fixture: ComponentFixture<PrescriptionRefillEformsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrescriptionRefillEformsPage ],
      providers: [
        provideAutoSpy(ActivatedRoute),
        provideAutoSpy(Router),
        provideAutoSpy(PartyService),
        provideAutoSpy(PrescriptionRefillEformsResource),
        provideAutoSpy(LoggerService),
        provideAutoSpy(DocumentService),
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrescriptionRefillEformsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

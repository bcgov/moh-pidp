import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';
import { ActivatedRoute, Router } from '@angular/router';
import { PartyService } from '@app/core/party/party.service';
import { PrescriptionRefillEformsResource } from './prescription-refill-eforms-resource.service';
import { LoggerService } from '@app/core/services/logger.service';
import { DocumentService } from '@app/core/services/document.service';
import { randTextRange } from '@ngneat/falso';
import { SafePipe } from '@bcgov/shared/ui';

describe('PrescriptionRefillEformsPage', () => {
  let component: PrescriptionRefillEformsPage;
  let fixture: ComponentFixture<PrescriptionRefillEformsPage>;

  let mockActivatedRoute: { snapshot: any };

  beforeEach(async () => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
    };

    await TestBed.configureTestingModule({
      declarations: [ PrescriptionRefillEformsPage, SafePipe ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
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

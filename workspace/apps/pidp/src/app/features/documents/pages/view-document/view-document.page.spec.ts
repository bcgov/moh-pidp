import { Location } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import {
  DocumentService,
  DocumentType,
} from '@app/core/services/document.service';

import { ViewDocumentPage } from './view-document.page';

describe('ViewDocumentPage', () => {
  let component: ViewDocumentPage;

  const mockActivatedRoute = {
    snapshot: {
      params: { doctype: DocumentType.PIDP_COLLECTION_NOTICE },
      data: {
        title: randTextRange({ min: 1, max: 4 }),
        routes: {
          root: '../../',
        },
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        ViewDocumentPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(DocumentService),
        provideAutoSpy(Location),
      ],
    });

    component = TestBed.inject(ViewDocumentPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { Location } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { DocumentService } from '@app/core/services/document.service';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

describe('SignedOrAcceptedDocumentsPage', () => {
  let component: SignedOrAcceptedDocumentsPage;

  let mockActivatedRoute;

  beforeEach(() => {
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

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        SignedOrAcceptedDocumentsPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(DocumentService),
        provideAutoSpy(Location),
      ],
    });

    component = TestBed.inject(SignedOrAcceptedDocumentsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

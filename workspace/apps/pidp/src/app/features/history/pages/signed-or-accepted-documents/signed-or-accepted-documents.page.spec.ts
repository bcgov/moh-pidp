import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
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
      imports: [HttpClientTestingModule, RouterModule.forRoot([])],
      providers: [
        SignedOrAcceptedDocumentsPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(KeycloakService),
        provideAutoSpy(DocumentService),
      ],
    });

    component = TestBed.inject(SignedOrAcceptedDocumentsPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

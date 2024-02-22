import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { provideAutoSpy } from 'jest-auto-spies';

import { RouteUtils } from '@bcgov/shared/utils';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import {
  DocumentService,
  DocumentType,
} from '@app/core/services/document.service';

import { HistoryRoutes } from '../../history.routes';
import { ViewDocumentPage } from './view-document.page';

describe('ViewDocumentPage', () => {
  let component: ViewDocumentPage;
  let router: Router;

  let mockActivatedRoute;

  beforeEach(() => {
    mockActivatedRoute = {
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

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        ViewDocumentPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        provideAutoSpy(DocumentService),
        provideAutoSpy(RouteUtils),
        provideAutoSpy(Router),
      ],
    });

    router = TestBed.inject(Router);
    component = TestBed.inject(ViewDocumentPage);
  });

  describe('METHOD: onBack', () => {
    given('user wants to go back to previous page', () => {
      when('onBack is invoked', () => {
        component.onBack();

        then(
          'router should navigate to signed or accepted documents route',
          () => {
            expect(router.navigate).toHaveBeenCalledWith([
              HistoryRoutes.BASE_PATH,
              HistoryRoutes.SIGNED_ACCEPTED_DOCUMENTS,
            ]);
          },
        );
      });
    });
  });
});

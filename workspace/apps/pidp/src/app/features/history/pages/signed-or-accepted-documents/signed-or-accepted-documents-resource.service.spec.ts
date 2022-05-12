import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { PortalResource } from '@app/features/portal/portal-resource.service';

import { SignedOrAcceptedDocumentsResource } from './signed-or-accepted-documents-resource.service';

describe('SignedOrAcceptedDocumentsResource', () => {
  let service: SignedOrAcceptedDocumentsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SignedOrAcceptedDocumentsResource,
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(SignedOrAcceptedDocumentsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

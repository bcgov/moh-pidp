import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { PortalResource } from '@app/features/portal/portal-resource.service';

import { EdredEformsResource } from './edred-eforms-resource.service';

describe('EdredEformsResource', () => {
  let service: EdredEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    service = TestBed.inject(EdredEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

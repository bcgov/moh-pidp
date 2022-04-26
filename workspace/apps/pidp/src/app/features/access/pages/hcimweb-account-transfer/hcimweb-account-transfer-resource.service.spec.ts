import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { HcimwebAccountTransferResource } from './hcimweb-account-transfer-resource.service';

describe('HcimwebAccountTransferResource', () => {
  let service: HcimwebAccountTransferResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HcimwebAccountTransferResource,
        provideAutoSpy(ApiHttpClient),
      ],
    });

    service = TestBed.inject(HcimwebAccountTransferResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

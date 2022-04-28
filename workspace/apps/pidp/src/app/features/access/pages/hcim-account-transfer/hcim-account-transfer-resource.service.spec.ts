import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { HcimAccountTransferResource } from './hcim-account-transfer-resource.service';

describe('HcimAccountTransferResource', () => {
  let service: HcimAccountTransferResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HcimAccountTransferResource, provideAutoSpy(ApiHttpClient)],
    });

    service = TestBed.inject(HcimAccountTransferResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

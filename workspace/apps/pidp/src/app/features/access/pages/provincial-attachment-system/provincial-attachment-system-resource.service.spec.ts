import { TestBed } from '@angular/core/testing';

import { ProvincialAttachmentSystemResource } from './provincial-attachment-system-resource.service';

describe('ProvincialAttachmentSystemResource', () => {
  let service: ProvincialAttachmentSystemResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProvincialAttachmentSystemResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

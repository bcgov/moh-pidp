import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { ProvincialAttachmentSystemResource } from './provincial-attachment-system-resource.service';

describe('ProvincialAttachmentSystemResource', () => {
  let service: ProvincialAttachmentSystemResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ProvincialAttachmentSystemResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
      ],
    });
    service = TestBed.inject(ProvincialAttachmentSystemResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

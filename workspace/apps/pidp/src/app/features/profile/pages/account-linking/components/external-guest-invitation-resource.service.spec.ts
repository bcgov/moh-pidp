import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { ExternalGuestInvitationResource } from './external-guest-invitation-resource.service';

describe('ExternalGuestInvitationResource', () => {
  let service: ExternalGuestInvitationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });
    service = TestBed.inject(ExternalGuestInvitationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

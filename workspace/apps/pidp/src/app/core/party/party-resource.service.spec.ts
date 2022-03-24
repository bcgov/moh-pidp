import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ResourceUtilsService } from '@bcgov/shared/data-access';

import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';

import { ApiHttpClient } from '../resources/api-http-client.service';
import { PartyResource } from './party-resource.service';

describe('PartyResource', () => {
  let service: PartyResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        PartyResource,
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ResourceUtilsService),
        provideAutoSpy(AuthorizedUserService),
      ],
    });

    service = TestBed.inject(PartyResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

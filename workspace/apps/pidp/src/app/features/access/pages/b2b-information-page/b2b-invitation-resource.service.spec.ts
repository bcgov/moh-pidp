import { TestBed } from '@angular/core/testing';

import { B2bInvitationResource } from './b2b-invitation-resource.service';

describe('B2bInvitationResource', () => {
  let service: B2bInvitationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(B2bInvitationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

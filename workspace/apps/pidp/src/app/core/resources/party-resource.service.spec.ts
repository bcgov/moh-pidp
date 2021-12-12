import { TestBed } from '@angular/core/testing';

import { PartyResourceService } from './party-resource.service';

describe('PartyResourceService', () => {
  let service: PartyResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PartyResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

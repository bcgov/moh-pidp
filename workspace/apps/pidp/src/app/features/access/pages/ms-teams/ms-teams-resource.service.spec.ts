import { TestBed } from '@angular/core/testing';

import { MsTeamsResource } from './ms-teams-resource.service';

describe('MsTeamsResource', () => {
  let service: MsTeamsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsTeamsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

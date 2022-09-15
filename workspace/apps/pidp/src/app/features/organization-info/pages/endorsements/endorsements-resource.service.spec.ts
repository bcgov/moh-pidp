import { TestBed } from '@angular/core/testing';

import { EndorsementsResource } from './endorsements-resource.service';

describe('EndorsementsResourceService', () => {
  let service: EndorsementsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EndorsementsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { LookupResourceService } from './lookup-resource.service';

describe('LookupResourceService', () => {
  let service: LookupResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LookupResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

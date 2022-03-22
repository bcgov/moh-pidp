import { TestBed } from '@angular/core/testing';

import { SaEformsResource } from './sa-eforms-resource.service';

describe('SaEformsResource', () => {
  let service: SaEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SaEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

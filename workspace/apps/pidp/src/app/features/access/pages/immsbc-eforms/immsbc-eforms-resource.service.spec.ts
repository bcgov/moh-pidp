import { TestBed } from '@angular/core/testing';

import { ImmsBCEformsResource } from './immsbc-eforms-resource.service';

describe('ImmsBCEformsResource', () => {
  let service: ImmsBCEformsResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmsBCEformsResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

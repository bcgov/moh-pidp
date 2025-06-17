import { TestBed } from '@angular/core/testing';

import { IvfResource } from './ivf-resource.service';

describe('IvfResource', () => {
  let service: IvfResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IvfResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { ImmsBCResource } from './immsbc-resource.service';

describe('ImmscbcResource', () => {
  let service: ImmsBCResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmsBCResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

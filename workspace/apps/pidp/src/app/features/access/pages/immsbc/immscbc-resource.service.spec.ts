import { TestBed } from '@angular/core/testing';

import { ImmscbcResource } from './immscbc-resource.service';

describe('ImmscbcResource', () => {
  let service: ImmscbcResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmscbcResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

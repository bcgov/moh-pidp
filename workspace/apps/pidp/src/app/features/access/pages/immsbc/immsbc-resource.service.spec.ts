import { TestBed } from '@angular/core/testing';

import { ImmsbcResourceService } from './immsbc-resource.service';

describe('ImmsbcResourceService', () => {
  let service: ImmsbcResourceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImmsbcResourceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

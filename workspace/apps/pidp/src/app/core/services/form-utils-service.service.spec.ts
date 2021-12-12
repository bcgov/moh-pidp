import { TestBed } from '@angular/core/testing';

import { FormUtilsServiceService } from './form-utils-service.service';

describe('FormUtilsServiceService', () => {
  let service: FormUtilsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FormUtilsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

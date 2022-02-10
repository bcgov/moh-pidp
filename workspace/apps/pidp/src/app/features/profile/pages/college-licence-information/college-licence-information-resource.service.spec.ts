import { TestBed } from '@angular/core/testing';

import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';

describe('CollegeLicenceInformationResource', () => {
  let service: CollegeLicenceInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CollegeLicenceInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

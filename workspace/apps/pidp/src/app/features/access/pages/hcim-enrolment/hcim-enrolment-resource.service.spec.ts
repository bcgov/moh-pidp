import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { HcimEnrolmentResource } from './hcim-enrolment-resource.service';

describe('HcimEnrolmentResource', () => {
  let service: HcimEnrolmentResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        HcimEnrolmentResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    service = TestBed.inject(HcimEnrolmentResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

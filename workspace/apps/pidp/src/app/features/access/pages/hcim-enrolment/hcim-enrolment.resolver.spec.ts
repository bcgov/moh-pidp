import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { hcimEnrolmentResolver } from './hcim-enrolment.resolver';

describe('HcimEnrolmentResolver', () => {
  let resolver: HcimEnrolmentResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        hcimEnrolmentResolver,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });
    resolver = TestBed.inject(hcimEnrolmentResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});

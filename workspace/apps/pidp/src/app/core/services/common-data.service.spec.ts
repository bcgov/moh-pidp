import { TestBed } from '@angular/core/testing';
import { CommonDataService } from './common-data.service';

import { RouterTestingModule } from '@angular/router/testing';

describe('CommonDataService', () => {
  let service: CommonDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        CommonDataService,
      ],
    });

    service = TestBed.inject(CommonDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

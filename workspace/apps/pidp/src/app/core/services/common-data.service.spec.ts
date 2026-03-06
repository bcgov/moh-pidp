import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { CommonDataService } from './common-data.service';

describe('CommonDataService', () => {
  let service: CommonDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
      providers: [CommonDataService],
    });

    service = TestBed.inject(CommonDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';

import { RouteStateService } from './route-state.service';

describe('RouteStateService', () => {
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [RouterModule.forRoot([])],
    }),
  );

  it('should create', () => {
    const service: RouteStateService = TestBed.inject(RouteStateService);
    expect(service).toBeTruthy();
  });
});

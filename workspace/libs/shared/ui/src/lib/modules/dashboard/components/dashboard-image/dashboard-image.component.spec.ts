import { TestBed } from '@angular/core/testing';

import { DashboardImageComponent } from './dashboard-image.component';

describe('DashboardImageComponent', () => {
  let component: DashboardImageComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DashboardImageComponent],
    });

    component = TestBed.inject(DashboardImageComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

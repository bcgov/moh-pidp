import { TestBed } from '@angular/core/testing';

import { AdminDashboardComponent } from './admin-dashboard.component';

describe('AdminDashboardComponent', () => {
  let component: AdminDashboardComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminDashboardComponent],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardRouteMenuItemComponent } from './dashboard-route-menu-item.component';

describe('DashboardRouteMenuItemComponent', () => {
  let component: DashboardRouteMenuItemComponent;
  let fixture: ComponentFixture<DashboardRouteMenuItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DashboardRouteMenuItemComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardRouteMenuItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

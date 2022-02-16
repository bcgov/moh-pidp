import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalDashboardComponent } from './portal-dashboard.component';

describe('PortalDashboardComponent', () => {
  let component: PortalDashboardComponent;
  let fixture: ComponentFixture<PortalDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PortalDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PortalDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

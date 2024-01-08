import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortalAlertComponent } from './portal-alert.component';

describe('PortalAlertComponent', () => {
  let component: PortalAlertComponent;
  let fixture: ComponentFixture<PortalAlertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [],
    }).compileComponents();

    fixture = TestBed.createComponent(PortalAlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

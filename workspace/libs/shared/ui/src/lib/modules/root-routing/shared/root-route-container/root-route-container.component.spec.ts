import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RootRouteContainerComponent } from './root-route-container.component';

describe('RootRouteContainerComponent', () => {
  let component: RootRouteContainerComponent;
  let fixture: ComponentFixture<RootRouteContainerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({}).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RootRouteContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

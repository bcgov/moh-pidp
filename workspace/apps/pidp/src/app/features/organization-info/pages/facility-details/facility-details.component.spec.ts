import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FacilityDetailsComponent } from './facility-details.component';

describe('FacilityDetailsComponent', () => {
  let component: FacilityDetailsComponent;
  let fixture: ComponentFixture<FacilityDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FacilityDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FacilityDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollTargetComponent } from './scroll-target.component';

describe('ScrollTargetComponent', () => {
  let component: ScrollTargetComponent;
  let fixture: ComponentFixture<ScrollTargetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({}).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScrollTargetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

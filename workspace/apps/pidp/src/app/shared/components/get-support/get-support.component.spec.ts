import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetSupportComponent } from './get-support.component';

describe('GetSupportComponent', () => {
  let component: GetSupportComponent;
  let fixture: ComponentFixture<GetSupportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetSupportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

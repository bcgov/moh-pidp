import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicMemberFormComponent } from './clinic-member-form.component';

describe('ClinicMemberFormComponent', () => {
  let component: ClinicMemberFormComponent;
  let fixture: ComponentFixture<ClinicMemberFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClinicMemberFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClinicMemberFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

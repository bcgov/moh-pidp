import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkAndRoleInformationComponent } from './work-and-role-information.component';

describe('WorkAndRoleInformationComponent', () => {
  let component: WorkAndRoleInformationComponent;
  let fixture: ComponentFixture<WorkAndRoleInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WorkAndRoleInformationComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkAndRoleInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

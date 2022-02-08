import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkAndRoleInformationPage } from './work-and-role-information.page';

describe('WorkAndRoleInformationPage', () => {
  let component: WorkAndRoleInformationPage;
  let fixture: ComponentFixture<WorkAndRoleInformationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WorkAndRoleInformationPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkAndRoleInformationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

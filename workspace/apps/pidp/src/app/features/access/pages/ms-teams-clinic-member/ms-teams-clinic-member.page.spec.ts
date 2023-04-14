import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MsTeamsClinicMemberPage } from './ms-teams-clinic-member.page';

describe('MsTeamsClinicMemberPage', () => {
  let component: MsTeamsClinicMemberPage;
  let fixture: ComponentFixture<MsTeamsClinicMemberPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MsTeamsClinicMemberPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MsTeamsClinicMemberPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

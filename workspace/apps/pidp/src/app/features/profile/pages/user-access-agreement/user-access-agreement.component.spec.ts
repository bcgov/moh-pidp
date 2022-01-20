import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAccessAgreementComponent } from './user-access-agreement.component';

describe('UserAccessAgreementComponent', () => {
  let component: UserAccessAgreementComponent;
  let fixture: ComponentFixture<UserAccessAgreementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAccessAgreementComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAccessAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAccessAgreementPage } from './user-access-agreement.page';

describe('UserAccessAgreementPage', () => {
  let component: UserAccessAgreementPage;
  let fixture: ComponentFixture<UserAccessAgreementPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAccessAgreementPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAccessAgreementPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

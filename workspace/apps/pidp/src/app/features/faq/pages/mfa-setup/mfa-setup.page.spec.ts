import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MfaSetupPage } from './mfa-setup.page';

describe('MfaSetupPage', () => {
  let component: MfaSetupPage;
  let fixture: ComponentFixture<MfaSetupPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MfaSetupPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MfaSetupPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MfaSetupComponent } from './mfa-setup.component';

describe('MfaSetupComponent', () => {
  let component: MfaSetupComponent;
  let fixture: ComponentFixture<MfaSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MfaSetupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MfaSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

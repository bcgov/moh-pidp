import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailDomainWarningComponent } from './email-domain-warning.component';

describe('EmailDomainWarningComponent', () => {
  let component: EmailDomainWarningComponent;
  let fixture: ComponentFixture<EmailDomainWarningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailDomainWarningComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailDomainWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

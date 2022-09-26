import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailWithDomainValidationComponent } from './email-with-domain-validation.component';

describe('EmailWithDomainValidationComponent', () => {
  let component: EmailWithDomainValidationComponent;
  let fixture: ComponentFixture<EmailWithDomainValidationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailWithDomainValidationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailWithDomainValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

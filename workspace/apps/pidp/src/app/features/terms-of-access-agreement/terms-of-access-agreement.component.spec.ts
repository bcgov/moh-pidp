import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermsOfAccessAgreementComponent } from './terms-of-access-agreement.component';

describe('TermsOfAccessAgreementComponent', () => {
  let component: TermsOfAccessAgreementComponent;
  let fixture: ComponentFixture<TermsOfAccessAgreementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TermsOfAccessAgreementComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TermsOfAccessAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

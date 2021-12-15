import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermsOfAccessComponent } from './terms-of-access.component';

describe('TermsOfAccessComponent', () => {
  let component: TermsOfAccessComponent;
  let fixture: ComponentFixture<TermsOfAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TermsOfAccessComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TermsOfAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TermsOfAccessPage } from './terms-of-access.page';

describe('TermsOfAccessPage', () => {
  let component: TermsOfAccessPage;
  let fixture: ComponentFixture<TermsOfAccessPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TermsOfAccessPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TermsOfAccessPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

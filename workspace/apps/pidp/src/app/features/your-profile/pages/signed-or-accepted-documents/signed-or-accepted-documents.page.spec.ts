import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignedOrAcceptedDocumentsPage } from './signed-or-accepted-documents.page';

describe('SignedOrAcceptedDocumentsPage', () => {
  let component: SignedOrAcceptedDocumentsPage;
  let fixture: ComponentFixture<SignedOrAcceptedDocumentsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SignedOrAcceptedDocumentsPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignedOrAcceptedDocumentsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

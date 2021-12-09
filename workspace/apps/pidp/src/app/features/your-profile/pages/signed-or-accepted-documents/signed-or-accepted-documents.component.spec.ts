import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignedOrAcceptedDocumentsComponent } from './signed-or-accepted-documents.component';

describe('SignedOrAcceptedDocumentsComponent', () => {
  let component: SignedOrAcceptedDocumentsComponent;
  let fixture: ComponentFixture<SignedOrAcceptedDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SignedOrAcceptedDocumentsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignedOrAcceptedDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

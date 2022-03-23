import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDocumentPage } from './view-document.page';

describe('ViewDocumentPage', () => {
  let component: ViewDocumentPage;
  let fixture: ComponentFixture<ViewDocumentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewDocumentPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDocumentPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

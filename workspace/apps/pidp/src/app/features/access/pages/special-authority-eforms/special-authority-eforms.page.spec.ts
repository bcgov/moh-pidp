import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialAuthorityEformsPage } from './special-authority-eforms.page';

describe('SpecialAuthorityEformsPage', () => {
  let component: SpecialAuthorityEformsPage;
  let fixture: ComponentFixture<SpecialAuthorityEformsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SpecialAuthorityEformsPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecialAuthorityEformsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

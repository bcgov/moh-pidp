import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialAuthorityEformsComponent } from './special-authority-eforms.component';

describe('SpecialAuthorityEformsComponent', () => {
  let component: SpecialAuthorityEformsComponent;
  let fixture: ComponentFixture<SpecialAuthorityEformsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SpecialAuthorityEformsComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecialAuthorityEformsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

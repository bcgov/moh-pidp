import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrescriptionRefillEformsPage } from './prescription-refill-eforms.page';

describe('PrescriptionRefillEformsPage', () => {
  let component: PrescriptionRefillEformsPage;
  let fixture: ComponentFixture<PrescriptionRefillEformsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrescriptionRefillEformsPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrescriptionRefillEformsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

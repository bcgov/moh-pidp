import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportErrorPage } from './support-error.page';

describe('SupportErrorPage', () => {
  let component: SupportErrorPage;
  let fixture: ComponentFixture<SupportErrorPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SupportErrorPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportErrorPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

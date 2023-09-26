import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NeedHelpComponent } from './need-help.component';

describe('NeedHelpComponent', () => {
  let component: NeedHelpComponent;
  let fixture: ComponentFixture<NeedHelpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NeedHelpComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NeedHelpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

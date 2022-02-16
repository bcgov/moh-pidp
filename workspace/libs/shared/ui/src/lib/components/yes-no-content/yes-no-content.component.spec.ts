import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YesNoContentComponent } from './yes-no-content.component';

describe('YesNoContentComponent', () => {
  let component: YesNoContentComponent;
  let fixture: ComponentFixture<YesNoContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [YesNoContentComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(YesNoContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

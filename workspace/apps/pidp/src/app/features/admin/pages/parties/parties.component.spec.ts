import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartiesComponent } from './parties.component';

describe('PartiesComponent', () => {
  let component: PartiesComponent;
  let fixture: ComponentFixture<PartiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PartiesComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PartiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

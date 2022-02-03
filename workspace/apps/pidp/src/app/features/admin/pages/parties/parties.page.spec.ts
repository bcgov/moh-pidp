import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartiesPage } from './parties.page';

describe('PartiesComponent', () => {
  let component: PartiesPage;
  let fixture: ComponentFixture<PartiesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PartiesPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PartiesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

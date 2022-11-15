import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UciPage } from './uci.page';

describe('UciPage', () => {
  let component: UciPage;
  let fixture: ComponentFixture<UciPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UciPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UciPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

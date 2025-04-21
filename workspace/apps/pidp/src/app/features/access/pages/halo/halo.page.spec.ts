import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HaloPage } from './halo.page';

describe('HaloPage', () => {
  let component: HaloPage;
  let fixture: ComponentFixture<HaloPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HaloPage],
    }).compileComponents();

    fixture = TestBed.createComponent(HaloPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

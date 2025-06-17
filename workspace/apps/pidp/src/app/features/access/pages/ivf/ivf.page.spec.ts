import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IvfPage } from './ivf.page';

describe('IvfPage', () => {
  let component: IvfPage;
  let fixture: ComponentFixture<IvfPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IvfPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IvfPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

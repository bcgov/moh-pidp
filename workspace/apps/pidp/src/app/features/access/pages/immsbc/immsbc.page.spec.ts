import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImmsbcPage } from './immsbc.page';

describe('ImmsbcPage', () => {
  let component: ImmsbcPage;
  let fixture: ComponentFixture<ImmsbcPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsbcPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImmsbcPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

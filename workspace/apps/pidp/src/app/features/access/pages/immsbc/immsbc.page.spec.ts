import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImmsBCPage } from './immsbc.page';

describe('ImmsbcPage', () => {
  let component: ImmsBCPage;
  let fixture: ComponentFixture<ImmsBCPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImmsBCPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsBCPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

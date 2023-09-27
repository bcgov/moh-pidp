import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImmsBCEformsPage } from './immsbc-eforms.page';

describe('ImmsBCEformsPage', () => {
  let component: ImmsBCEformsPage;
  let fixture: ComponentFixture<ImmsBCEformsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ImmsBCEformsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImmsBCEformsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
